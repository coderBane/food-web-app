using System;

namespace Foody.Entities.Models;

public class AppUser : BaseEntity
{
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(50)]
    public string? MiddleName { get; set; }

    [Display(Name = "")]
    public string FullName => FirstName + " " + LastName;

    [DataType(DataType.Date)]
    public DateTime DoB { get; set; }

    public Address Address { get; set; }
}

