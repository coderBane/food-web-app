namespace Foody.Admin.Models;

public class Item : Base
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool IsActive { get; set; }
}

public class Category : Item { }

