using Foody.Admin.Rest.Services;

namespace Foody.Admin.Manager;

public interface IDataManager
{
    CategoryService Category { get; }
}

