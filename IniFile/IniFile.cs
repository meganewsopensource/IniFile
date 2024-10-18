using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace IniFile;


public class IniFile
{
   
  
   

    public static string ObjectToIni(object obj)
    {
        return ObjectToIniSection(obj);
    }

    private static string GetSectionName(Type objectType, string? initialSectionName)
    {
        if (initialSectionName != null)
            return initialSectionName;
        
        var sessionAttribute = objectType.GetCustomAttribute<IniSectionAttribute>();
        return sessionAttribute != null ? sessionAttribute.Name : objectType.Name;
    }
    
    private static string ObjectToIniSection(object obj, string? iniSectionName = null)
    {

        var iniString = new StringBuilder();
        var sectionName = GetSectionName(obj.GetType(),iniSectionName);


        var fields = obj.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(e => e.GetCustomAttributes(typeof(IniPropertyAttribute)).Any());

        if (fields.Any())
        {
            iniString.AppendLine($"[{sectionName}]");
        }

        foreach (var property in fields)
        {
            var iniPropertyAttribute = property.GetCustomAttribute<IniPropertyAttribute>();
            var propertyName = iniPropertyAttribute?.Name;
            var propertyValue = property.GetValue(obj);
            var propertyType = property.FieldType;

            if (IsValidProperty(obj.GetType().Name, property.Name, propertyValue, iniPropertyAttribute) == false)
            {
                return "";
            }


            if (propertyType.IsEnum)
            {
                iniString.AppendLine(IniEnumField(propertyName, propertyValue));
            }
            else if (IsFloatType(propertyType))
            {
                iniString.AppendLine(IniDoubleField(property, propertyName, obj));
            }
            else if (IsDateTimeType(propertyType))
            {
                iniString.AppendLine(IniDateTimeField(property, propertyName, obj));
            }
            else if (IsSimpleType(propertyType))
            {
                if (!iniPropertyAttribute.Required && propertyValue!.ToString()! == iniPropertyAttribute.Default)
                {
                    continue;
                }

                iniString.AppendLine(IniField(propertyName, propertyValue?.ToString()));
            }
            else if (IsEnumerable(propertyType))
            {
                if (propertyValue != null)
                {
                    var index = 0;

                    foreach (var item in (IEnumerable)propertyValue)
                    {
                        index++;

                        var nestedSectionName = ($"{propertyName}{index:D4}");
                        var nestedSection = ObjectToIniSection(item, nestedSectionName);

                        iniString.AppendLine(nestedSection);
                    }
                }
            }
            else
            {
                var nestedSectionName = ($"{propertyName}");
                var nestedSection = ObjectToIniSection(propertyValue, nestedSectionName);
                iniString.AppendLine(nestedSection);
            }

        }

        return iniString.ToString().TrimEnd();
    }

    private static string IniDateTimeField(FieldInfo property, string propertyName, object? obj)
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

        return IniField(propertyName,valueStr);
    }

    private static string IniField(string propertyName, string propertyValue)
    {
        return $"{propertyName}={propertyValue}";
    }

    private static string IniDoubleField(FieldInfo property, string propertyName, object obj)
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
        return IniField(propertyName, valueStr);
    }


    private static string IniEnumField(string propertyName, object? value)
    {
        return IniField(propertyName, value != null ? ((int)value).ToString() : "0");
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
        public IniFileSerializer()
        {
            if (!typeof(T).GetCustomAttributes(typeof(IniSectionAttribute), inherit: false).Any())
            {
                throw new InvalidOperationException($"A classe {typeof(T).Name} deve ter o atributo IniSection.");
            }
        }

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
