using System.ComponentModel;

namespace user_api.cs.Enum;

public enum UserType
{
    [Description("PF")]
    Individual = 1,
    [Description("PJ")]
    Company = 2,
}
