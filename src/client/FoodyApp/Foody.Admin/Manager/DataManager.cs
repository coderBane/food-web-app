using Foody.Admin.Rest.Interfaces;
using Foody.Admin.Rest.Services;

namespace Foody.Admin.Manager;

public class DataManager : IDataManager
{
    public DataManager()
    {
        Category = new CategoryService();
    }

    public CategoryService Category { get; private set; }
}

