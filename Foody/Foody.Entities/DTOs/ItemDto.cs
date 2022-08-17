#nullable disable

using System.ComponentModel;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Foody.Entities.DTOs;

public class ItemDto
{
    [Required]
    public string Name { get; set; }

    [DefaultValue(false)]
    public bool IsActive { get; set; }

    [JsonIgnore]
    public IFormFile ImageUpload { get; set; }
}

public class ItemDetailDto : ItemDto
{
    public string  ImageUri { get; set; }

    public byte[] ImageData { get; set; }

    public DateTime AddedOn { get; set; }

    public DateTime Updated { get; set; }
}

public class CategoryDto : ItemDto { }

public class CategoryDetailDto : ItemDetailDto { }
