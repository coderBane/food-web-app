namespace Foody.Admin.ViewModels.Category;

[QueryProperty("Category","category")]
public partial class CategoryDetailsVM : BaseViewModel
{
    public CategoryDetailsVM(IDataManager dataManager) : base(dataManager)
    {
        Title = Category is null ? "Details" : $"Details: {Category.Name}";
    }

    [ObservableProperty]
    CategoryDetail category;

    [ICommand]
    async void GoBack() => await Shell.Current.GoToAsync("..", true);
}

