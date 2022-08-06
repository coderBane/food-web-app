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

    public string FullName => MiddleName is not null ?
        FirstName + " " + MiddleName + " " + LastName : FirstName + " " + LastName;

    public DateTime DoB { get; set; }

    public int? AddressId { get; set; }

    public Address Address { get; set; }
}

