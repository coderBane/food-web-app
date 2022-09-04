namespace Foody.Admin.Rest.Services;

public class CategoryService : RestService<Category>
{
    public CategoryService()
    {
        this.url = Address.Category.BaseAddress;
    }
}

