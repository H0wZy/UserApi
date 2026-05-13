using AutoMapper;
using user_api.cs.Dto;
using user_api.cs.Models;
using user_api.cs.ValueObjects;

namespace user_api.cs.Mappings.Resolvers;

public class CpfResolver : IValueResolver<CreateUserDto, User, Cpf>
{
    public Cpf Resolve(CreateUserDto source, User destination, Cpf destMember, ResolutionContext context)
    {
        var result = Cpf.Create(source.Cpf);
        return !result.Success ? throw new AutoMapperMappingException(result.Error) : result.Value!;
    }
}
