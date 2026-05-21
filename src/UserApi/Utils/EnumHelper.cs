using System.ComponentModel;

namespace UserApi.Utils;

public static class EnumHelper
{
    public static string ToDescription(this System.Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
        return attribute?.Description ?? value.ToString();
    }
}