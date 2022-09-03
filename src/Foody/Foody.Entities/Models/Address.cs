namespace Foody.Entities.Models
{
    public class Address : BaseEntity
    {
        public string? HouseNo { get; set; }

        [Required]
        [StringLength(80)]
        public string Street { get; set; } = null!;

        [StringLength(30)]
        public string? City { get; set; } = null!;

        public State State { get; set; } = State.FCT;

        [Display(Name = "Post Code")]
        public string? ZipCode { get; set; } = null!;

        [Required]
        public string Country { get; set; } = "Nigeria";
    }

    public enum State { FCT }
}