using System.Xml.Linq;

namespace IniFile;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class IniPropertyAttribute(string name, string @default = "", bool required = true) : Attribute
{
    internal string Name { get; } = name;
    internal bool Required { get; } = required;
    internal string Default { get; } = @default;
}
    

[AttributeUsage(AttributeTargets.Class)]
public class IniSectionAttribute(string name) : Attribute
{
    internal string Name { get; } = name;
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class)]
public class ListIndexFormatAttribute(string displayFormat) : Attribute
{
    internal string DisplayFormat { get; } = displayFormat;
}

