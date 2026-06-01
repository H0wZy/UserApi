using System.ComponentModel;

namespace UserApi.Enum;

public enum Role
{
    [Description("Administrador")] Admin = 1,
    [Description("Usuário comum")] CommonUser = 2
}