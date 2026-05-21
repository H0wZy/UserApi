using AutoMapper;
using UserApi.Dto;
using UserApi.Models;
using UserApi.Utils;

namespace UserApi.Mappings;

public class UserProfile : Profile
{
    /// <summary>
    /// Responsabilidades da arquitetura:
    /// <list type="bullet">
    /// <item><description>DTO → transporte de dados</description></item>
    /// <item><description>VO → invariantes e regras de domínio</description></item>
    /// <item><description>Service → orquestra regras de negócio</description></item>
    /// <item><description>AutoMapper → hidrata dados simples</description></item>
    /// <item><description>EF Core → persistência</description></item>
    /// </list>
    /// </summary>
    public UserProfile()
    {
        // TODO FIXME: Preciso mapear LoginDto e TokenDto?
        // TODO FIXME: Preciso mapear campos IsOnline e LoginMethod da AccountEntity?

        // CreateUserDto → User
        CreateMap<CreateUserDto, User>()
            // Ignore obrigatório: DTO tem mesmo nome, mas são VOs setados manualmente
            .ForMember(dest => dest.Email, opt => opt.Ignore())
            .ForMember(dest => dest.Cpf, opt => opt.Ignore())
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.BirthDate, opt => opt.Ignore())
            // Source members sem correspondência válida no destino
            .ForSourceMember(src => src.Email, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.Cpf, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.Password, opt => opt.DoNotValidate());

        // UpdateUserDto → User (só atualiza campos não-nulos)
        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.Email, opt => opt.Ignore())
            .ForSourceMember(src => src.Email, opt => opt.DoNotValidate())
            .ForAllMembers(opt => opt.Condition((_, _, srcMember) => srcMember is not null));

        // User → UserDto (convenção automática, só mapeia o que difere)
        CreateMap<User, UserDto>()
            .ForCtorParam(nameof(UserDto.Email), opt => opt.MapFrom(src => src.Email.Value))
            .ForCtorParam(nameof(UserDto.Cpf), opt => opt.MapFrom(src => src.Cpf.Value))
            .ForCtorParam(nameof(UserDto.UserType), opt => opt.MapFrom(src => src.UserType.ToDescription()));
    }
}