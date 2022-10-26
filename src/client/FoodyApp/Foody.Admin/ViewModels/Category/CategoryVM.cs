namespace Foody.Admin.ViewModels.Category;

public partial class CategoryVM : BaseViewModel
{
    public CategoryVM(IDataManager dataManager) : base(dataManager)
    {
        Title = nameof(Category);
        Columns = new[]{ "ID", "Name", "Status" };

        MessagingCenter.Subscribe<CategoryVM>(this, "refresh", async (sender) => await CategoryList());
        MessagingCenter.Subscribe<CatogoryModifyVM>(this, "refresh", async (sender) =>
        {
            await CategoryList();
            await GoBack();
        });

        Task.Run(CategoryList);
    }

    #region Properties
    [ObservableProperty]
    Models.Category selectedCategory; 

    public ObservableCollection<Models.Category> Categories { get; } = new();
    #endregion

    #region Commands
    [ICommand]
    async Task CategoryList()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            IsRefreshing = true;

            var result = await dataManager.Category.AllAsync();

            if (result is null)
                throw new NullReferenceException("Could not retrive results");

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
            IsRefreshing = false;
        }
    }

    [ICommand]
    async Task Create()
    {
        await Shell.Current.GoToAsync("categorymod", true,
    	new Dictionary<string, object>()
        {
            ["Category"] = new Models.Category(),
            ["IsNew"] = true
        });
    }

    [ICommand]
    async Task Tapped(Models.Category category)
    {
        SelectedCategory = category;

        string action = await Shell.Current.DisplayActionSheet("Choose Option", "Cancel",
            "Delete", "Edit", "View");

        switch (action)
        {
            case "Edit": await Get(0x65) ; break;
            case "Delete": await Delete(); break;
            case "View": await Get(0x76); break;
            default: break;
        }
    }
    #endregion

    #region Actions
    async Task Get(int command)
    {
        if (SelectedCategory is null) return;

        try
        {
            var result = await dataManager.Category.GetAsync(SelectedCategory.Id);

            if (result is null)
                throw new NullReferenceException("Could not retrieve result");

            var navigationParameter = new Dictionary<string, object>()
            {
                ["Category"] = result.Content,
                ["IsNew"] = false
            };

            switch (command)    
            {
                case 0x65:
                    await Shell.Current.GoToAsync("categorymod",true, navigationParameter);
                    break;

                case 0x76:
                    await Shell.Current.GoToAsync("categorydeet", true, navigationParameter);
                    break;

                default:
                    break;
            }

            MainThread.BeginInvokeOnMainThread(() => SelectedCategory = null);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Console.WriteLine(ex);
        }        
    }

    async Task Delete()
    {
        if (SelectedCategory is null) return;

        bool answer = await Shell.Current.DisplayAlert("Confirm",
           $"Are you sure you want to delete '{SelectedCategory.Name}'", "Yes", "Cancel");

        if (!answer) return;

        try
        {
            var result = await dataManager.Category.DeleteAsync(SelectedCategory.Id);

            if (result is not null)
                Debug.WriteLine($"{result.Error.Title} : {result.Error.Message}");

            MessagingCenter.Send(this, "refresh");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
    #endregion
}

