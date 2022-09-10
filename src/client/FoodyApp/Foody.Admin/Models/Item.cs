using System.Text.Json.Serialization;

namespace Foody.Admin.Models;

public class Item : Base
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool IsActive { get; set; }
}

public class Category : Item { }

public class CategoryDetail : Item
{
    public string ImageUri { get; set; }

    public byte[] ImageData { get; set; }

    public DateTime AddedOn { get; set; }

    public DateTime Updated { get; set; }
}

