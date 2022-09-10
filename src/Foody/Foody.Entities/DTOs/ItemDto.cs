#nullable disable

using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Foody.Entities.DTOs;

public class ItemDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool IsActive { get; set; }
}

public class ItemModDto
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

public class CategoryModDto : ItemModDto { }

public class CategoryDetailDto : ItemDetailDto { }


public class ProductDto : ItemDto
{
    public string Category { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public string Description { get; set; }
}

public class ProductDetailDto : ProductDto
{
    public string ImageUri { get; set; }

    public byte[] ImageData { get; set; }

    public DateTime AddedOn { get; set; }

    public DateTime Updated { get; set; }
}

public class ProductModDto : ItemModDto
{
    [Required]
    public int Category { get; set; }

    [DefaultValue(0)]
    public int Quantity { get; set; }

    [Required]
    public decimal Price { get; set; }

    [DataType(DataType.MultilineText)]
    public string Description { get; set; }
}