using System;
using System.ComponentModel.DataAnnotations;

namespace Foody.Auth.DTOs
{
    public class LogInDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}

