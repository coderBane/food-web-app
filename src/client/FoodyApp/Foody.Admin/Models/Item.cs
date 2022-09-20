using System.Text.Json.Serialization;

namespace Foody.Admin.Models;

public class Image
{
    public int Id { get; set; }

    public long FileSize { get; set; }

    public string Content { get; set; }
}

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

    public Image Image { get; set; }

    public DateTime AddedOn { get; set; }

    public DateTime Updated { get; set; }
}

public class CategoryMod
{
    public string Name { get; set; }

    public bool IsActive { get; set; }
}

