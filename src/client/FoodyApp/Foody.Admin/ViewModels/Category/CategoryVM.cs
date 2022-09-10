using Foody.Admin.Pages.Category;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Foody.Admin.ViewModels.Category;

public partial class CategoryVM : BaseViewModel
{
    public CategoryVM(IDataManager dataManager) : base(dataManager)
    {
        this.Title = nameof(Category);

        Task.Run(CategoryList);
    }

    public ObservableCollection<Models.Category> Categories { get; } = new();

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
    async Task GetCategory(Models.Category category)
    {
        if (category is null) return;

        try
        {
            var result = await dataManager.Category.GetAsync(category.Id);

            if (result is null)
                throw new NullReferenceException("Could not retrieve result");

            var navigationParameter = new Dictionary<string, object>()
            {
                ["category"] = ToObject<CategoryDetail>(result.Content)
            };

            await Shell.Current.GoToAsync("categorydeet", true, navigationParameter);

            //MainThread.BeginInvokeOnMainThread(() => SelectedCategory = null);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Console.WriteLine(ex);
        }        
    }

    //[ICommand]
    //async Task AddCategory()
    //{
    //    //MediaPicker.PickPhotoAsync()
    //}
    #endregion
}

