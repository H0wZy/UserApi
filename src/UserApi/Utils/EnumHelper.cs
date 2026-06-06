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

    public static T ParseEnum<T>(string value, Dictionary<string, T> map)
    {
        return map.TryGetValue(value, out var type)
            ? type
            : throw new InvalidOperationException($"Valor '{value}' não é válido para '{typeof(T).Name}'.");
    }
}