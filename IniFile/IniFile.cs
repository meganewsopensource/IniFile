using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace IniFile;

public class IniFile
{
    public static string ObjectToIni(object serializableObject)
    {
        IniWriter iniWriter = new();

        var fields = serializableObject.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
          .Where(e => e.GetCustomAttributes(typeof(IniPropertyAttribute)).Any());

        if (serializableObject.GetType().GetCustomAttribute<IniSectionAttribute>() == null && ContainsSimpleType(fields))
        {
            throw new Exception($"A classe {serializableObject.GetType()} não contém o atributo IniSection");
        }

        Section? section = null;
        if (ContainsSimpleType(fields))
        {
            var sectionName = GetSectionNameFromSectionAttribute(serializableObject.GetType());
            section = iniWriter.CreateSection(sectionName);
        }

        WriteToIni(iniWriter,serializableObject, section, null);

        var content = iniWriter.ToString();
        return content;
    }

    private static string GetSectionNameFromSectionAttribute(Type objectType)
    {
        var sessionAttribute = objectType.GetCustomAttribute<IniSectionAttribute>();
        return sessionAttribute != null ? sessionAttribute.Name : objectType.Name;
    }
    
    private static void WriteToIni(IniWriter iniWriter,object serializableObject, Section? section, Section? parentSection)
    {
        var fields = serializableObject.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
           .Where(e => e.GetCustomAttributes(typeof(IniPropertyAttribute)).Any());

        foreach (var property in fields)
        {
            var iniPropertyAttribute = property.GetCustomAttribute<IniPropertyAttribute>();
            var propertyName = iniPropertyAttribute?.Name;
            var propertyValue = property.GetValue(serializableObject);
            var propertyType = property.FieldType;

            if (IsValidProperty(serializableObject.GetType().Name, property.Name, propertyValue, iniPropertyAttribute) == false)
            {
                return;
            }


            if (propertyType.IsEnum)
            {
                section?.AddItem(propertyName, IniEnumField(propertyValue));
            }
            else if (IsFloatType(propertyType))
            {
                section?.AddItem(propertyName, IniDoubleField(property, serializableObject));
            }
            else if (IsDateTimeType(propertyType))
            {
                section?.AddItem(propertyName, IniDateTimeField(property, serializableObject));
            }
            else if (IsSimpleType(propertyType))
            {
                if (!iniPropertyAttribute.Required && propertyValue!.ToString()! == iniPropertyAttribute.Default)
                {
                    continue;
                }
                section?.AddItem(propertyName, propertyValue?.ToString());
            }
            else if (IsEnumerable(propertyType))
            {
                if (propertyValue != null)
                {
                    var listIndexFormat = "0000";
                    var listIndexFormatAttribute = property.GetCustomAttribute<ListIndexFormatAttribute>();
                    if (listIndexFormatAttribute != null)
                    {
                        if (string.IsNullOrEmpty(listIndexFormatAttribute.DisplayFormat) == false)
                            listIndexFormat = listIndexFormatAttribute.DisplayFormat;
                    }

                    foreach (var itemSerializableObject in (IEnumerable)propertyValue)
                    {
                        var newSection = iniWriter.CreateListItemSection(propertyName,listIndexFormat, parentSection);
                        WriteToIni(iniWriter,itemSerializableObject, newSection, newSection);
                    }
                }
            }
            else
            {
                var newSection = iniWriter.CreateSection(propertyName);
                 WriteToIni(iniWriter,propertyValue,newSection, parentSection);
            }

        }
    }

    private static bool ContainsSimpleType(IEnumerable<FieldInfo> fields)
    {
        var existsSimpleTypes = fields.Where(e => e.FieldType.IsEnum ||

          IsFloatType(e.FieldType) ||

          IsDateTimeType(e.FieldType) ||

          IsSimpleType(e.FieldType)

       );

        return existsSimpleTypes.Count() > 0;
    }

    private static string IniDateTimeField(FieldInfo property, object? obj)
    {
        var valueStr = "";
        var propertyValue = property.GetValue(obj);

        if (propertyValue != null)
        {
            DisplayFormatAttribute? formatAttribute = null;
            formatAttribute = property.GetCustomAttribute<FormatDateAttribute>();
            if (formatAttribute == null)
                formatAttribute = property.GetCustomAttribute<FormatTimeAttribute>();

            if (formatAttribute == null)
                formatAttribute = property.GetCustomAttribute<FormatDateAndTimeAttribute>();

          
            if (formatAttribute != null)
            {
                valueStr = string.Format(formatAttribute.DataFormatString, propertyValue);
            }
            else
            {
                valueStr = string.Format("{0:dd/MM/yyy HH:mm:ss}", propertyValue);
            }
        }

        return valueStr;
    }

    private static string IniField(string propertyName, string propertyValue)
    {
        return $"{propertyName}={propertyValue}";
    }

    private static string IniDoubleField(FieldInfo property, object obj)
    {
        var valueStr = "0";
        var propertyValue = property.GetValue(obj);

        if (propertyValue != null)
        {
            var formatAttribute = property.GetCustomAttribute<FormatNumericAttribute>();
            if (formatAttribute != null)
            {
                valueStr = formatAttribute.Format((double)propertyValue);
            }
            else
            {
                valueStr = ((double)propertyValue).ToString("0:N2", CultureInfo.CreateSpecificCulture("en-US"));
            }
        }
        return valueStr;
    }


    private static string IniEnumField(object? value)
    {
        return value != null ? ((int)value).ToString() : "0";
    }
    

    private static bool IsValidProperty(string objectName, string propertyName, object? propertyValue, IniPropertyAttribute? propertyAttibute)
    {
        if (propertyAttibute.Required && propertyValue == null)
        {
            throw new ArgumentNullException($"Property value of \"{objectName}.{propertyName}\" is required");
        }
      
        return true;
    }
    
    private static bool IsFloatType(Type type)
    {
        return type == typeof(decimal) || type == typeof(decimal?) ||
                type == typeof(double) || type == typeof(double?) ||
                type == typeof(float) || type == typeof(float?);
    }

    private static bool IsDateTimeType(Type type)
    {
        return type == typeof(DateTime) || type == typeof(DateTime?);
    }

    private static bool IsSimpleType(Type type)
    {
        return type.IsPrimitive || 
               type.IsEnum ||    
               type == typeof(string) ||
               type == typeof(decimal) || type == typeof(decimal?) ||
               type == typeof(DateTime) || type == typeof(DateTime?) ||
               type == typeof(Guid) ||  type == typeof(Guid?); 
    }

    private static bool IsEnumerable(Type type)
    {
        return typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);
    }
    
    public class IniFileSerializer<T> where T : class
    {
        public string ToIniFile()
        {
            return ObjectToIni(this);
        }

        public void SaveToIniFile(string filePath)
        {

            if (File.Exists(filePath))
                File.Delete(filePath);
            else
            {
                var directory = Path.GetDirectoryName(filePath);
                if (directory != null)
                  Directory.CreateDirectory(directory);
            }

            File.WriteAllText(filePath, ToIniFile());
        }

        public override string ToString()
        {
            return ToIniFile();
        }
    }
    
}
