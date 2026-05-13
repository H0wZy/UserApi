using AutoMapper;
using user_api.cs.Dto;
using user_api.cs.Models;

namespace user_api.cs.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // CreateUserDto → User
        // ignora campos que o sistema controla, não o cliente
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.HashPassword, opt => opt.Ignore())
            .ForMember(dest => dest.SaltPassword, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.LastLoginAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDisabled, opt => opt.Ignore());

        // UpdateUserDto → User (só atualiza campos não-nulos)
        CreateMap<UpdateUserDto, User>()
            .ForAllMembers(opt => opt.Condition((_, _, srcMember) => srcMember is not null));

        // User → UserDto (convenção automática, só mapeia o que difere)
        CreateMap<User, UserDto>();
    }
}