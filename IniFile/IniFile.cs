using System.Reflection;
using System.Text;

namespace IniFile;


public class IniFile
{
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IniProperty (string name, string @default = "", bool required = true) : Attribute
    {
    
        internal string Name { get; } = name;
        internal bool Required { get; } = required;
        internal string Default { get; } = @default;
        
    }
    
    [AttributeUsage(AttributeTargets.Class)]
    public class IniSession(string name) : Attribute
    {
        internal string Name { get; } = name;
    }

    public static string ObjectToIni(object obj)
    {
        var properties = obj.GetType().GetFields( BindingFlags.NonPublic | BindingFlags.Instance);
        
        var iniString = new StringBuilder();
        
        var atributo = obj.GetType().GetCustomAttribute<IniSession>();
        
        if (atributo != null)
        {
            iniString.AppendLine("["+atributo.Name+"]");
        }

        foreach (var property in properties)
        {
            var iniNameAttribute = property.GetCustomAttribute<IniProperty>();
            
            if (iniNameAttribute == null) continue;
            
            var value = property.GetValue(obj);
                
            if (!iniNameAttribute.Required &&  value!.ToString()! == iniNameAttribute.Default)
            {
                continue;
            }
                
            iniString.AppendLine($"{iniNameAttribute.Name}={value}");
        }

        return iniString.ToString();
    }
    
}
