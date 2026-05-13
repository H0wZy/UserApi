using Microsoft.AspNetCore.Mvc;
using user_api.cs.Dto;
using user_api.cs.Services;
using user_api.cs.Shared;

namespace user_api.cs.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService service) : ControllerBase
{
    [HttpGet("GetAllUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<GenericResponse<IEnumerable<UserDto>>>> GetAllUsers()
    {
        var response = await service.GetAllAsync();
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpGet("GetUserById/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenericResponse<UserDto>>> GetUserById(Guid id)
    {
        var response = await service.GetByIdAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpGet("GetUserByEmail/{email}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenericResponse<UserDto>>> GetUserByEmail(string email)
    {
        var response = await service.GetUserByEmailAsync(email);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpGet("GetUserByUsername/{username}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenericResponse<UserDto>>> GetUserByUsername(string username)
    {
        var response = await service.GetUserByUsernameAsync(username);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPost("CreateUser")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<GenericResponse<UserDto>>> CreateUser([FromBody] CreateUserDto dto)
    {
        var response = await service.CreateAsync(dto);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPut("UpdateUserById/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenericResponse<UserDto>>> UpdateUserById(Guid id, [FromBody] UpdateUserDto dto)
    {
        var response = await service.UpdateByIdAsync(id, dto);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPatch("UpdatePassword/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenericResponse<bool>>> UpdatePassword(Guid id, [FromBody] UpdatePasswordDto dto)
    {
        var response = await service.UpdateUserPasswordAsync(id, dto);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPatch("UpdateLastLogin/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenericResponse<bool>>> UpdateLastLogin(Guid id)
    {
        var response = await service.UpdateUserLastLoginAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpDelete("DeleteUser/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenericResponse<bool>>> DeleteUser(Guid id)
    {
        var response = await service.DeleteByIdAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }
}