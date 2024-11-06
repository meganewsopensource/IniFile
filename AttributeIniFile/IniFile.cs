using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace IniFile;

public class IniFile
{
    public static string ObjectToIni(object serializableObject, IIniWriter? iniWriter = null)
    {
        iniWriter ??= new IniWriter();
         
        
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

        return iniWriter.ToString();
    }

    private static string GetSectionNameFromSectionAttribute(Type objectType)
    {
        var sessionAttribute = objectType.GetCustomAttribute<IniSectionAttribute>();
        return sessionAttribute != null ? sessionAttribute.Name : objectType.Name;
    }
    
    private static void WriteToIni(IIniWriter iniWriter,object serializableObject, Section? section, Section? parentSection)
    {
        var fields = serializableObject.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
           .Where(e => e.GetCustomAttributes(typeof(IniPropertyAttribute)).Any());

        foreach (var property in fields)
        {
            var iniPropertyAttribute = property.GetCustomAttribute<IniPropertyAttribute>();
            var propertyName = iniPropertyAttribute?.Name;
            var propertyValue = property.GetValue(serializableObject) ?? iniPropertyAttribute?.DefaulValue;
            var propertyType = property.FieldType;

            if (IsValidProperty(serializableObject.GetType().Name, property.Name, propertyValue, iniPropertyAttribute) == false)
            {
                return;
            }
            
            if (iniPropertyAttribute!.Required || (!iniPropertyAttribute!.Required && (propertyValue?.ToString() != iniPropertyAttribute?.DefaulValue.ToString())))
            {
                Section? newSection;
                switch (propertyType)
                {
                    case var _ when IsEnumType(propertyType) :
                        section?.AddItem(propertyName, IniEnumField(propertyValue));
                        break;
                    case var _ when IsFloatType(propertyType) :
                        section?.AddItem(propertyName, IniDoubleField(property, serializableObject, iniPropertyAttribute?.DefaulValue));
                        break; 
                    case var _ when IsDateTimeType(propertyType) :
                        section?.AddItem(propertyName, IniDateTimeField(property, serializableObject, iniPropertyAttribute?.DefaulValue));
                        break;
                    case var _ when IsBoolType(propertyType) : 
                        section?.AddItem(propertyName, IniBoolField(property, serializableObject, iniPropertyAttribute?.DefaulValue));
                        break;
                    case var _ when IsSimpleType(propertyType) :
                        section?.AddItem(propertyName, propertyValue?.ToString());
                        break;
                    case var _ when IsEnumerable(propertyType) :
                        if (propertyValue != null)
                        {
                            var listIndexFormatAttribute = property.GetCustomAttribute<ListIndexFormatAttribute>();
                            var listIndexFormat = listIndexFormatAttribute != null && !string.IsNullOrEmpty(listIndexFormatAttribute.DisplayFormat) ? listIndexFormatAttribute.DisplayFormat : ListIndexFormatAttribute.DefaultDisplayFormat;
                          
                            foreach (var itemSerializableObject in (IEnumerable)propertyValue)
                            {
                                newSection = iniWriter.CreateListItemSection(propertyName,listIndexFormat, parentSection);
                                WriteToIni(iniWriter,itemSerializableObject, newSection, newSection);
                            }
                        }
                        break;
                    case var _ when IsClass(propertyType) :
                        newSection = iniWriter.CreateSection(propertyName);
                        WriteToIni(iniWriter,propertyValue,newSection, parentSection);
                        break;
                    default:
                        throw new NotImplementedException("Property " + property.Name + " is not implemented");
                } 
            }
        }
    }

 
    private static bool ContainsSimpleType(IEnumerable<FieldInfo> fields)
    {
        var existsSimpleTypes = fields.Where(e => !IsEnumerable(e.FieldType) && !IsClass(e.FieldType));
   
        return existsSimpleTypes.Count() > 0;
    }

    private static string IniDateTimeField(FieldInfo property, object? obj, object? defaultValue)
    {
        var valueStr = "";
        var propertyValue = property.GetValue(obj) ?? defaultValue;

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

    private static string IniBoolField(FieldInfo property, object? obj, object? defaultValue)
    {
        var propertyValue = property.GetValue(obj) ?? defaultValue;
        if (propertyValue != null)
        {
            return ((bool)propertyValue).ToString();
        }

        return "";
    }
    

    private static string IniDoubleField(FieldInfo property, object obj, object? defaultValue)
    {
        var valueStr = "0";

        var propertyValue = property.GetValue(obj) ?? defaultValue;

        if (propertyValue != null)
        {
            var formatAttribute = property.GetCustomAttribute<FormatNumericAttribute>();
            if (formatAttribute != null)
            {
                valueStr = formatAttribute.Format((double)propertyValue);
            }
            else
            {
                valueStr = ((double)propertyValue).ToString("N2", CultureInfo.CreateSpecificCulture("en-US"));
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
        object? defaultValue = propertyAttibute?.DefaulValue;

        if (propertyAttibute.Required && propertyValue == null && defaultValue == null)
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

    private static bool IsEnumType(Type type)
    {
        return (Nullable.GetUnderlyingType(type) ?? type).IsEnum;
    }

    private static bool IsDateTimeType(Type type)
    {
        return type == typeof(DateTime) || type == typeof(DateTime?);
    }

    private static bool IsSimpleType(Type type)
    {
        return type.IsPrimitive || 
               type.IsEnum ||    
               type == typeof(string)  || 
               type == typeof(decimal) || type == typeof(decimal?) ||
               type == typeof(DateTime) || type == typeof(DateTime?) ||
               IsIntegerType(type) ||
               type == typeof(bool) || type == typeof(bool?) ||
               type == typeof(Guid) ||  type == typeof(Guid?); 
        
    }
    
    private static bool IsIntegerType(Type type)
    {
        Type underlyingType = Nullable.GetUnderlyingType(type) ?? type;

        return underlyingType == typeof(byte) || underlyingType == typeof(sbyte) ||
               underlyingType == typeof(short) || underlyingType == typeof(ushort) ||
               underlyingType == typeof(int) || underlyingType == typeof(uint) ||
               underlyingType == typeof(long) || underlyingType == typeof(ulong);
    }

    private static bool IsEnumerable(Type type)
    {
        return typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);
    }

    private static bool IsClass(Type type)
    {
        return type.IsClass && type != typeof(string);
    }

    private static bool IsBoolType(Type type)
    {
        return type == typeof(bool) || type == typeof(bool?);
    }
    
}

public class IniFileSerializer
{
    public string ToIniFile(IIniWriter? writer = null)
    {
        return IniFile.ObjectToIni(this,writer);
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
