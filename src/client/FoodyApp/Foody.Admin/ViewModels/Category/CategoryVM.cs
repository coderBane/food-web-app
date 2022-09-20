using Foody.Admin.Pages.Category;
using Microsoft.Toolkit.Mvvm.ComponentModel;


namespace Foody.Admin.ViewModels.Category;

public partial class CategoryVM : BaseViewModel
{
    public CategoryVM(IDataManager dataManager) : base(dataManager)
    {
        this.Title = nameof(Category);

        MessagingCenter.Subscribe<CategoryVM>(this, "refresh", async (sender) => await CategoryList());

        Task.Run(CategoryList);
    }

    #region Properties
    [ObservableProperty]
    bool isnew;

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

            var result = await dataManager.Category.AllAsync(string.Empty);

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
    async Task Tapped(Models.Category category)
    {
        SelectedCategory = category;

        string action = await Shell.Current.DisplayActionSheet("Choose Option", "Cancel",
            "Delete", "Edit", "View");

        switch (action)
        {
            case "Edit": break;
            case "Delete": await Delete(); break;
            case "View": await Get(); break;
            default: break;
        }
    }
    #endregion

    #region Actions
    async Task Get()
    {
        if (SelectedCategory is null) return;

        try
        {
            var result = await dataManager.Category.GetAsync(SelectedCategory.Id);

            if (result is null)
                throw new NullReferenceException("Could not retrieve result");

            var navigationParameter = new Dictionary<string, object>()
            {
                ["category"] = ToObject<CategoryDetail>(result.Content)
            };

            await Shell.Current.GoToAsync("categorydeet", true, navigationParameter);

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
           $"Are you sure yo wnat to delete '{SelectedCategory.Name}'", "Yes", "Cancel");

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

