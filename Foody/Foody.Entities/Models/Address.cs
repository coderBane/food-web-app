using System;

namespace Foody.Entities.Models;

public class Address : BaseEntity
{
    [Required]
    [Column(Order = 1)]
    public string Street { get; set; } = null!;

    [Column(Order = 2)]
    public State State { get; set; } = State.FCT;

    [MaxLength(8)]
    [Column(Order = 3)]
    public string? PostCode { get; set; }

    [Column(Order = 4)]
    public Country Country { get; set; } = Country.Nigeria;
}

public enum State { FCT }

public enum Country { Nigeria }
