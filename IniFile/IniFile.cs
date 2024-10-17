using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

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

        iniString.AppendLine($"[{sectionName}]");
        
        var fields = obj.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(e => e.GetCustomAttributes(typeof(IniPropertyAttribute)).Any());

        foreach (var property in fields)
        {
            var iniPropertyAttribute = property.GetCustomAttribute<IniPropertyAttribute>();
            var propertyValue = property.GetValue(obj);
            var propertyType = property.FieldType;

            if (IsValidProperty(obj.GetType().Name, property.Name, propertyValue, iniPropertyAttribute) == false)
            {
                return "";
            }


            if (propertyType.IsEnum)
            {
                iniString.AppendLine($"{iniPropertyAttribute.Name}={(int)propertyValue}");
            }
            else if (IsSimpleType(propertyType))
            {
                if (!iniPropertyAttribute.Required && propertyValue!.ToString()! == iniPropertyAttribute.Default)
                {
                    continue;
                }

                iniString.AppendLine($"{iniPropertyAttribute.Name}={propertyValue}");
            }
            else if (IsEnumerable(propertyType))
            {
                if (propertyValue != null)
                {
                    var index = 0;

                    foreach (var item in (IEnumerable)propertyValue)
                    {
                        index++;

                        var nestedSectionName = ($"{iniPropertyAttribute.Name}{index:D4}");
                        var nestedSection = ObjectToIniSection(item, nestedSectionName);

                        iniString.AppendLine(nestedSection);
                    }
                }
            }
            else
            {
                var nestedSectionName = ($"{iniPropertyAttribute.Name}");
                var nestedSection = ObjectToIniSection(propertyValue, nestedSectionName);
                iniString.AppendLine(nestedSection);
            }

        }

        return iniString.ToString();
    }

    private static bool IsValidProperty(string objectName, string propertyName, object? propertyValue, IniPropertyAttribute? propertyAttibute)
    {
        if (propertyAttibute.Required && propertyValue == null)
        {
            throw new ArgumentNullException($"Property value of \"{objectName}.{propertyName}\" is required");
        }
      
        return true;
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
            File.WriteAllText(filePath, ToIniFile());
        }

        public override string ToString()
        {
            return ToIniFile();
        }
    }
    
}
