using Microsoft.AspNetCore.Mvc;
using user_api.cs.Dto;
using user_api.cs.Enum;
using user_api.cs.Services;
using user_api.cs.Shared;
using user_api.cs.Utils;

namespace user_api.cs.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UserController(IUserService service, IAuthService authService) : ControllerBase
{
    [HttpGet("get-all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<GenericResponse<IEnumerable<UserDto>>>> GetAllUsers() =>
        await ExecuteAsync(service.GetAllAsync);

    [HttpGet("id/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenericResponse<UserDto>>> GetUserById(Guid id) =>
        await ExecuteAsync(() => service.GetByIdAsync(id));

    [HttpGet("email/{email}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenericResponse<UserDto>>> GetUserByEmail(string email) =>
        await ExecuteAsync(() => service.GetUserByEmailAsync(email));

    [HttpGet("username/{username}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenericResponse<UserDto>>> GetUserByUsername(string username) =>
        await ExecuteAsync(() => service.GetUserByUsernameAsync(username));

    [HttpGet("cpf/{cpf}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenericResponse<UserDto>>> GetUserByCpf(string cpf) =>
        await ExecuteAsync(() => service.GetUserByCpfAsync(cpf));

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<GenericResponse<UserDto>>> CreateUser([FromBody] CreateUserDto dto) =>
        await ExecuteAsync(() => service.CreateAsync(dto));

    [HttpPatch("{id:guid}/update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenericResponse<UserDto>>> UpdateUserById(Guid id, [FromBody] UpdateUserDto dto) =>
        await ExecuteAsync(() => service.UpdateByIdAsync(id, dto));

    [HttpPatch("{id:guid}/change-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenericResponse<bool>>> UpdatePassword(Guid id, [FromBody] UpdatePasswordDto dto) =>
        await ExecuteAsync(() => service.UpdateUserPasswordAsync(id, dto));

    [HttpPatch("{id:guid}/last-logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenericResponse<bool>>> UpdateLastLogout(Guid id) =>
        await ExecuteAsync(() => service.UpdateUserLastLogoutAsync(id));

    [HttpDelete("{id:guid}/delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenericResponse<bool>>> DeleteUserById(Guid id) =>
        await ExecuteAsync(() => service.DeleteByIdAsync(id));

    [HttpPatch("{id:guid}/disable")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenericResponse<bool>>> DisableUserById(Guid id) =>
        await ExecuteAsync(() => service.DisableByIdAsync(id));

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GenericResponse<TokenDto>>> Login(LoginDto dto) =>
        await ExecuteAsync(() => authService.LoginAsync(dto));

    [HttpGet("types")]
    public IActionResult GetUserTypes()
    {
        var types = System.Enum.GetValues<UserType>()
            .Select(t => new UserTypeOptionDto(
                Value: (int)t,
                Label: t.ToDescription()));
        return Ok(types);
    }

    private async Task<ActionResult> ExecuteAsync<T>(Func<Task<GenericResponse<T>>> action)
    {
        var response = await action();
        return StatusCode((int)response.StatusCode, response);
    }
}