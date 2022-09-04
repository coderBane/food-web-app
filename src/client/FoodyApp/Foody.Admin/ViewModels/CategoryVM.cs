namespace Foody.Admin.ViewModels;

public partial class CategoryVM : BaseViewModel
{
    public CategoryVM(IDataManager dataManager) : base(dataManager)
    {
        this.Title = nameof(Category);

        Task.Run(CategoryList);
    }

    public ObservableCollection<Category> Categories { get; } = new();

    [ICommand]
    async Task CategoryList()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            var result = await dataManager.Category.AllAsync(string.Empty);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Categories.Clear();

                foreach (var c in result.Content)
                    Categories.Add(c);
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            IsBusy = false;
        }
    }
}

