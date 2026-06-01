using System.ComponentModel;

namespace UserApi.Enum;

public enum AccType
{
    [Description("PF")] Individual = 1,
    [Description("PJ")] Company = 2,
}
