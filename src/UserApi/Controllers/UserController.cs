using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using UserApi.Dto;
using UserApi.Services;
using UserApi.Enum;
using UserApi.Shared;
using Microsoft.AspNetCore.Authorization;
using UserApi.Utils;

namespace UserApi.Controllers;

[ApiController]
[Route("api/v1/users")]
[Authorize]
public class UserController(IUserService userService, IAuthService authService) : ControllerBase
{
    [HttpGet("get-all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GenericResponse<IEnumerable<UserDto>>>> GetAllUsers() =>
        await ExecuteAsync(userService.GetAllAsync);

    [HttpGet("id/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenericResponse<UserDto>>> GetUserById(Guid id) =>
        await ExecuteAsync(() => userService.GetByIdAsync(id));

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("me")]
    public async Task<ActionResult<GenericResponse<UserDto>>> GetMe()
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(id, out var guid)) return Unauthorized();
        return await ExecuteAsync(() => userService.GetByIdAsync(guid));
    }

    [HttpGet("email/{email}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenericResponse<UserDto>>> GetUserByEmail(string email) =>
        await ExecuteAsync(() => userService.GetUserByEmailAsync(email));

    [HttpGet("username/{username}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenericResponse<UserDto>>> GetUserByUsername(string username) =>
        await ExecuteAsync(() => userService.GetUserByUsernameAsync(username));

    [HttpGet("cpf/{cpf}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenericResponse<UserDto>>> GetUserByCpf(string cpf) =>
        await ExecuteAsync(() => userService.GetUserByCpfAsync(cpf));

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [AllowAnonymous]
    public async Task<ActionResult<GenericResponse<UserDto>>> CreateUser([FromBody] CreateUserDto dto) =>
        await ExecuteAsync(() => userService.CreateAsync(dto));

    [HttpPatch("{id:guid}/update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<ActionResult<GenericResponse<UserDto>>> UpdateUserById(Guid id, [FromBody] UpdateUserDto dto) =>
        await ExecuteAsync(() => userService.UpdateByIdAsync(id, dto));

    [HttpPatch("{id:guid}/change-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<ActionResult<GenericResponse<bool>>> UpdatePassword(Guid id, [FromBody] UpdatePasswordDto dto) =>
        await ExecuteAsync(() => userService.UpdateUserPasswordAsync(id, dto));

    [HttpDelete("{id:guid}/delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GenericResponse<bool>>> DeleteUserById(Guid id) =>
        await ExecuteAsync(() => userService.DeleteByIdAsync(id));

    [HttpPatch("{id:guid}/disable")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<ActionResult<GenericResponse<bool>>> DisableUserById(Guid id) =>
        await ExecuteAsync(() => userService.DisableByIdAsync(id));

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public async Task<ActionResult<GenericResponse<TokenDto>>> Login(LoginDto dto) =>
        await ExecuteAsync(() => authService.LoginAsync(dto));

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GenericResponse<bool>>> Logout()
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(id, out var guid)) return Unauthorized();
        return await ExecuteAsync(() => authService.LogoutAsync(guid));
    }

    [HttpGet("account-types")]
    [AllowAnonymous]
    public IActionResult GetAccTypes()
    {
        var types = System.Enum.GetValues<AccType>()
            .Select(t => new AccTypeOptionDto(
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