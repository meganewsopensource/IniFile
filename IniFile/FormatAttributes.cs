using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace IniFile;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class FormatNumericAttribute : DisplayFormatAttribute
{
    private readonly string _culture;
    
    public FormatNumericAttribute(int DecimalPlaces, string CultureInfo = "en-US")
    {
        DataFormatString = "{0:N" + DecimalPlaces.ToString() + "}";
        _culture = CultureInfo;
    }

    public string Format(double value)
    {
        if (!string.IsNullOrEmpty(_culture))
            return value.ToString(DataFormatString, CultureInfo.CreateSpecificCulture(_culture));
        else return value.ToString(DataFormatString);
    }
    public string Format(decimal value)
    {
        if (!string.IsNullOrEmpty(_culture))
            return value.ToString(DataFormatString, CultureInfo.CreateSpecificCulture(_culture));
        else return value.ToString(DataFormatString);
    }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class FormatDateAttribute : DisplayFormatAttribute
{
    public FormatDateAttribute()
    {
        DataFormatString = "{0:dd/MM/yyy}";
    }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class FormatTimeAttribute : DisplayFormatAttribute
{
    public FormatTimeAttribute()
    {
        DataFormatString = "c";
    }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class FormatDateAndTimeAttribute : DisplayFormatAttribute
{
    public FormatDateAndTimeAttribute()
    {
        DataFormatString = "{0:dd/MM/yyy HH:mm:ss}";
    }
}
