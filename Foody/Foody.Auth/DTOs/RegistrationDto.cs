using Foody.Auth.Models;
using System.ComponentModel.DataAnnotations;


namespace Foody.Auth.DTOs;

public class RegistrationDto
{
    public string? UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = null!;
}
    