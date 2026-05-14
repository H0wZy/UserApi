using AutoMapper;
using user_api.cs.Dto;
using user_api.cs.Mappings.Resolvers;
using user_api.cs.Models;
using user_api.cs.Utils;

namespace user_api.cs.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // CreateUserDto → User
        CreateMap<CreateUserDto, User>()
            // Ignorados, pois o sistema controla, não o cliente
            .ForMember(dest => dest.HashPassword, opt => opt.Ignore())
            .ForMember(dest => dest.SaltPassword, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDisabled, opt => opt.Ignore())
            .ForMember(dest => dest.LastLoginAt, opt => opt.Ignore())
            .ForMember(dest => dest.LastLogoutAt, opt => opt.Ignore())
            .ForMember(dest => dest.AcceptedTerms, opt => opt.Ignore())
            .ForMember(dest => dest.AcceptedTermsAt, opt => opt.Ignore())

            // Mapeados
            .ForMember(dest => dest.Cpf, opt => opt.MapFrom<CpfResolver>())
            .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType));

        // UpdateUserDto → User (só atualiza campos não-nulos)
        CreateMap<UpdateUserDto, User>()
            .ForAllMembers(opt => opt.Condition((_, _, srcMember) => srcMember is not null));

        // User → UserDto (convenção automática, só mapeia o que difere)
        CreateMap<User, UserDto>()
            .ForCtorParam(nameof(UserDto.Cpf), opt => opt.MapFrom(src => src.Cpf.Value))
            .ForCtorParam(nameof(UserDto.UserType), opt => opt.MapFrom(src => src.UserType.ToDescription()));
    }
}