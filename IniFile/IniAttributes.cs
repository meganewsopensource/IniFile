using System.Xml.Linq;

namespace IniFile;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class IniPropertyAttribute(string name, object? defaultValue = null, bool required = true) : Attribute
{
    internal string Name { get; } = name;
    internal bool Required { get; } = required;
    internal object DefaulValue { get; } = defaultValue;
}
    

[AttributeUsage(AttributeTargets.Class)]
public class IniSectionAttribute(string name) : Attribute
{
    internal string Name { get; } = name;
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class)]
public class ListIndexFormatAttribute(string displayFormat) : Attribute
{
    public const string DefaultDisplayFormat = "000";
    
    internal string DisplayFormat { get; } = displayFormat;

   
}

