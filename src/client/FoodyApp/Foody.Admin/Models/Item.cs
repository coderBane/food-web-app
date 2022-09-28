namespace Foody.Admin.Models;

public class Image
{
    public int Id { get; set; }

    public long FileSize { get; set; }

    public string Content { get; set; }
}

public abstract class Item : Base
{
    public string Name { get; set; }

    public bool IsActive { get; set; }

    public string ImageUri { get; set; }

    public Image Image { get; set; }

    public ByteArrayContent ImageUpload { get; set; }
}

public sealed class Category : Item { }

