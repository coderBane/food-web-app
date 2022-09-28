namespace Foody.Admin.ViewModels.Category;

[QueryProperty("Category","Category")]
public partial class CategoryDetailsVM : BaseViewModel
{
    public CategoryDetailsVM(IDataManager dataManager) : base(dataManager)
    {
        Title = Category is null ? "Details" : $"Details: {Category.Name}";
    }

    [ObservableProperty]
    Models.Category category;
}

