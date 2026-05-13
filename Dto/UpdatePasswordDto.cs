using System.ComponentModel.DataAnnotations;

namespace user_api.cs.Dto;

public record UpdatePasswordDto(
    [Required] string CurrentPassword,
    [Required, MinLength(8)] string NewPassword
);