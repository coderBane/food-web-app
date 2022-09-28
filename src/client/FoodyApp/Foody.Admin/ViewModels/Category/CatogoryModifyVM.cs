using Foody.Admin.Utilities;
using System.Net.Http.Headers;


namespace Foody.Admin.ViewModels.Category;

[QueryProperty("Category","Category")]
[QueryProperty("IsNew", "IsNew")]
public partial class CatogoryModifyVM : BaseViewModel
{
    public CatogoryModifyVM(IDataManager dataManager) : base(dataManager)
    {
    }

    [ObservableProperty]
    bool isNew;

    [ObservableProperty]
    Models.Category category;

    [ObservableProperty]
    FileResult selectedFile;

    //[ObservableProperty]
    //ImageSource image;

    //async Task LoadImage()
    //{
    //    if (SelectedFile is not null)
    //    {
    //        using var stream = await SelectedFile.OpenReadAsync();
    //        Image = ImageSource.FromStream(() => stream);
    //    }
    //    else if (Category.Image is not null)
    //    {
    //        Image = ImageSource.FromStream(() =>
    //            new MemoryStream(Convert.FromBase64String(Category.Image.Content)));
    //    }
    //    else
    //    {
    //        Image = null;
    //    }
    //}

    [ICommand]
    async Task Upload()
    {
        SelectedFile = await FilePicker.Default.PickAsync();

        if (SelectedFile is not null)
        {
            Category.ImageUri = SelectedFile.FileName;
        }

        //await LoadImage();
    }

    [ICommand]
    async Task Save()
    {
        if (Category is null) return;

        try
        {
            if (SelectedFile is not null)
            {
                Category.ImageUpload = new(File.ReadAllBytes(SelectedFile.FullPath));
                Category.ImageUpload.Headers.ContentType = MediaTypeHeaderValue.Parse($"image/{SelectedFile.ContentType}");
                Category.ImageUpload.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = SelectedFile.FileName,
                    Name = nameof(Category.ImageUpload)
                };
            }

            var result = await dataManager.Category.SaveDataAsync(Category, IsNew);

            if (result is not null)
            {
                if (!result.Success)
                {
                    Debug.WriteLine($"{result.Error.Title} : {result.Error.Message}");
                    await Shell.Current.DisplayAlert(result.Error.Title, result.Error.Message, "Try Again");
                    return;
                }
            }

            Debug.WriteLine("Saved");
            await Shell.Current.DisplayAlert("Success", "", "OK");
            MessagingCenter.Send(this, "refresh");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}

