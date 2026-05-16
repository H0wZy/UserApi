using AutoMapper;
using user_api.cs.Dto;
using user_api.cs.Models;
using user_api.cs.Repositories;
using user_api.cs.Shared;
using user_api.cs.Utils;
using user_api.cs.ValueObjects;

namespace user_api.cs.Services;

public class UserService(IUserRepository repository, IMapper mapper) : GenericService<User, UserDto, CreateUserDto, UpdateUserDto>(repository, mapper), IUserService
{
    private readonly IMapper _mapper = mapper;

    public override async Task<GenericResponse<UserDto>> CreateAsync(CreateUserDto dto)
    {
        if (!dto.AcceptTerms)
            return GenericResponse<UserDto>.BadRequest("O usuário deve aceitar os termos para se cadastrar.");

        var cpfResult = Cpf.Create(dto.Cpf);
        if (cpfResult.IsFailure) return GenericResponse<UserDto>.BadRequest(cpfResult.Error!);

        var passwordResult = Password.Create(dto.Password);
        if (passwordResult.IsFailure) return GenericResponse<UserDto>.BadRequest("Não foi possível criar o usuário.", passwordResult.Errors!);

        if (await repository.GetCpfExistenceAsync(dto.Cpf))
            return GenericResponse<UserDto>.Conflict("Cpf já cadastrado.");

        if (await repository.GetEmailExistenceAsync(dto.Email))
            return GenericResponse<UserDto>.Conflict("Email já cadastrado.");
            
        if (await repository.GetUsernameExistenceAsync(dto.Username))
            return GenericResponse<UserDto>.Conflict("Nome de usuário já cadastrado.");

        var user = _mapper.Map<User>(dto);

        user.Cpf = cpfResult.Data!;
        user.Password = passwordResult.Data!;

        user.AcceptedTerms = true;
        user.AcceptedTermsAt = DateTime.UtcNow;

        var created = await repository.CreateAsync(user);
        return GenericResponse<UserDto>.Created(_mapper.Map<UserDto>(created));
    }

    public async Task<GenericResponse<UserDto>> GetUserByEmailAsync(string email)
    {
        var user = await repository.GetByEmailAsync(email);
        return user is null ? GenericResponse<UserDto>.NotFound() : GenericResponse<UserDto>.Ok(_mapper.Map<UserDto>(user), "Busca por email realizada com sucesso!");
    }

    public async Task<GenericResponse<UserDto>> GetUserByUsernameAsync(string username)
    {
        var user = await repository.GetByUsernameAsync(username);
        return user is null ? GenericResponse<UserDto>.NotFound() : GenericResponse<UserDto>.Ok(_mapper.Map<UserDto>(user), "Busca por username realizada com sucesso!");
    }

    public async Task<GenericResponse<UserDto>> GetUserByCpfAsync(string cpf)
    {
        var cpfResult = Cpf.Create(cpf);
        if (cpfResult.IsFailure) return GenericResponse<UserDto>.BadRequest(cpfResult.Error!);

        var user = await repository.GetByCpfAsync(cpfResult.Data!.Value);
        return user is null
            ? GenericResponse<UserDto>.NotFound()
            : GenericResponse<UserDto>.Ok(_mapper.Map<UserDto>(user), "Busca por cpf realizada com sucesso!");
    }

    public async Task<GenericResponse<bool>> UpdateUserPasswordAsync(Guid id, UpdatePasswordDto dto)
    {
        var user = await repository.GetByIdAsync(id);
        if (user is null) return GenericResponse<bool>.NotFound();

        var passwordResult = Password.Create(dto.NewPassword);
        if (passwordResult.IsFailure) return GenericResponse<bool>.BadRequest("Não foi possível atualizar senha.", passwordResult.Errors!);

        if (!user.Password.Verify(dto.CurrentPassword))
            return GenericResponse<bool>.BadRequest("Senha atual inválida.");
        if (dto.CurrentPassword == dto.NewPassword)
            return GenericResponse<bool>.BadRequest("A nova senha não pode ser igual à senha atual.");

        user.Password = passwordResult.Data!;

        await repository.UpdateAsync(user);
        return GenericResponse<bool>.Ok(true, "Senha atualizada com sucesso!");
    }

    public async Task<GenericResponse<bool>> UpdateUserLastLoginAsync(Guid id)
    {
        var user = await repository.GetByIdAsync(id);
        if (user is null) return GenericResponse<bool>.NotFound();

        user.LastLoginAt = DateTime.UtcNow;
        await repository.UpdateAsync(user);
        return GenericResponse<bool>.Ok(true);
    }
}
