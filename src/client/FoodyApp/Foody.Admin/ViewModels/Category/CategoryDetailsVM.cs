namespace Foody.Admin.ViewModels.Category;

[QueryProperty("Category","Category")]
public partial class CategoryDetailsVM : BaseViewModel
{
    public CategoryDetailsVM(IDataManager dataManager) : base(dataManager)
    {
        Title = "Details";
    }

    [ObservableProperty]
    Models.Category category;

    [ICommand]
    async Task Edit()
    {
        await Shell.Current.GoToAsync("categorymod", true, new Dictionary<string, object>()
        {
            ["Category"] = Category
        });
    }
}

