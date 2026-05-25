using AutoMapper;
using UserApi.Constants;
using UserApi.Dto;
using UserApi.Models;
using UserApi.Repositories;
using UserApi.Shared;
using UserApi.ValueObjects;

namespace UserApi.Services;

public class UserService(IUserRepository repository, IMapper mapper)
    : GenericService<User, UserDto, CreateUserDto, UpdateUserDto>(repository, mapper), IUserService
{
    private readonly IMapper _mapper = mapper;

    public override async Task<GenericResponse<UserDto>> CreateAsync(CreateUserDto dto)
    {
        if (!dto.AcceptedTerms)
            return GenericResponse<UserDto>.BadRequest("O usuário deve aceitar os termos para se cadastrar.");

        var cpfResult = Cpf.Create(dto.Cpf);
        if (cpfResult.IsFailure) return GenericResponse<UserDto>.BadRequest(cpfResult.Error!);

        var passwordResult = Password.Create(dto.Password);
        if (passwordResult.IsFailure)
            return GenericResponse<UserDto>.BadRequest(UserResponse.CreationFailed, passwordResult.Errors!);

        var usernameResult = Username.Create(dto.Username);
        if (usernameResult.IsFailure)
            return GenericResponse<UserDto>.BadRequest(UserResponse.CreationFailed, usernameResult.Errors!);

        var emailResult = Email.Create(dto.Email);
        if (emailResult.IsFailure)
            return GenericResponse<UserDto>.BadRequest(UserResponse.CreationFailed, emailResult.Errors!);

        var nameResult = Name.Create(dto.FirstName, dto.LastName);
        if (nameResult.IsFailure)
            return GenericResponse<UserDto>.BadRequest(UserResponse.CreationFailed, nameResult.Errors!);

        var cpf = cpfResult.Data!;
        var password = passwordResult.Data!;
        var username = usernameResult.Data!;
        var email = emailResult.Data!;
        var name = nameResult.Data!;

        if (await repository.GetCpfExistenceAsync(cpf.Value))
            return GenericResponse<UserDto>.Conflict("Cpf já cadastrado.");

        if (await repository.GetEmailExistenceAsync(email.Value))
            return GenericResponse<UserDto>.Conflict("Email já cadastrado.");

        if (await repository.GetUsernameExistenceAsync(username.Value))
            return GenericResponse<UserDto>.Conflict("Nome de usuário já cadastrado.");

        // Cria usuário apenas com campos/VO required/init only;
        var user = new User
        {
            Username = username,
            Email = email,
            Name = name,
            Cpf = cpf,
            Password = password,
            BirthDate = dto.BirthDate,
            AcceptedTerms = true,
            AcceptedTermsAt = DateTime.UtcNow,
        };

        // Os demais são mapeados depois para aproveitar as validações do AutoMapper
        _mapper.Map(dto, user);

        var createdUser = await repository.CreateAsync(user);
        return GenericResponse<UserDto>.Created(_mapper.Map<UserDto>(createdUser), UserResponse.CreationSuccess);
    }

    public override async Task<GenericResponse<UserDto>> UpdateByIdAsync(Guid id, UpdateUserDto dto)
    {
        var user = await repository.GetByIdAsync(id);
        if (user is null) return GenericResponse<UserDto>.NotFound(UserResponse.NotFound);

        dto = new UpdateUserDto(
            Username: string.IsNullOrWhiteSpace(dto.Username) ? null : dto.Username.Trim(),
            Email: string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim(),
            FirstName: string.IsNullOrWhiteSpace(dto.FirstName) ? null : dto.FirstName.Trim(),
            LastName: string.IsNullOrWhiteSpace(dto.LastName) ? null : dto.LastName.Trim());

        if (dto.Username is null && dto.Email is null && dto.FirstName is null && dto.LastName is null)
            return GenericResponse<UserDto>.BadRequest("Nenhum campo foi informado para atualização.");

        if (dto.FirstName is not null || dto.LastName is not null)
        {
            var nameResult = Name.Create(dto.FirstName ?? user.Name.FirstName, dto.LastName ?? user.Name.LastName);
            if (nameResult.IsFailure)
                return GenericResponse<UserDto>.BadRequest(UserResponse.UpdateFailed, nameResult.Errors!);
            user.Name = nameResult.Data!;
        }

        if (dto.Username is not null)
        {
            var usernameResult = Username.Create(dto.Username);
            if (usernameResult.IsFailure)
                return GenericResponse<UserDto>.BadRequest(UserResponse.UpdateFailed, usernameResult.Errors!);

            var validatedUsername = usernameResult.Data!;

            if (user.Username == validatedUsername)
                return GenericResponse<UserDto>.Conflict(
                    "O nome de usuário informado é igual ao nome de usuário  atual.");

            if (await repository.GetUsernameExistenceAsync(validatedUsername.Value))
                return GenericResponse<UserDto>.Conflict("Nome de usuário já cadastrado.");

            user.Username = validatedUsername;
        }

        if (dto.Email is not null)
        {
            var emailResult = Email.Create(dto.Email);
            if (emailResult.IsFailure)
                return GenericResponse<UserDto>.BadRequest(UserResponse.UpdateFailed, emailResult.Errors!);

            var validatedEmail = emailResult.Data!;

            if (user.Email == validatedEmail)
                return GenericResponse<UserDto>.Conflict("O email informado é igual ao email atual.");

            if (await repository.GetEmailExistenceAsync(validatedEmail.Value))
                return GenericResponse<UserDto>.Conflict("Email já cadastrado.");

            user.Email = validatedEmail;
        }

        _mapper.Map(dto, user);

        await repository.UpdateAsync(user);
        return GenericResponse<UserDto>.Ok(_mapper.Map<UserDto>(user), UserResponse.UpdateSuccess);
    }

    public async Task<GenericResponse<UserDto>> GetUserByEmailAsync(string email)
    {
        var user = await repository.GetByEmailAsync(email);
        return user is null
            ? GenericResponse<UserDto>.NotFound()
            : GenericResponse<UserDto>.Ok(_mapper.Map<UserDto>(user), "Busca por email realizada com sucesso!");
    }

    public async Task<GenericResponse<UserDto>> GetUserByUsernameAsync(string username)
    {
        var user = await repository.GetByUsernameAsync(username);
        return user is null
            ? GenericResponse<UserDto>.NotFound()
            : GenericResponse<UserDto>.Ok(_mapper.Map<UserDto>(user), "Busca por username realizada com sucesso!");
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
        if (passwordResult.IsFailure)
            return GenericResponse<bool>.BadRequest("Não foi possível atualizar senha.", passwordResult.Errors!);

        if (!user.Password.Verify(dto.CurrentPassword))
            return GenericResponse<bool>.BadRequest("Senha atual inválida.");
        if (dto.CurrentPassword == dto.NewPassword)
            return GenericResponse<bool>.BadRequest("A nova senha não pode ser igual à senha atual.");

        user.Password = passwordResult.Data!;

        await repository.UpdateAsync(user);
        return GenericResponse<bool>.Ok(true, "Senha atualizada com sucesso!");
    }

    public async Task<GenericResponse<bool>> UpdateUserLastLoginAsync(Guid id, string loginMethod)
    {
        var user = await repository.GetByIdAsync(id);
        if (user is null) return GenericResponse<bool>.NotFound();

        user.LoginMethod = loginMethod;
        user.IsOnline = true;
        user.LastLoginAt = DateTime.UtcNow;

        await repository.UpdateAsync(user);
        return GenericResponse<bool>.Ok(true);
    }

    public async Task<GenericResponse<bool>> UpdateUserLastLogoutAsync(Guid id)
    {
        var user = await repository.GetByIdAsync(id);
        if (user is null) return GenericResponse<bool>.NotFound(UserResponse.NotFound);

        user.LastLogoutAt = DateTime.UtcNow;
        await repository.UpdateAsync(user);
        return GenericResponse<bool>.Ok(true);
    }
}