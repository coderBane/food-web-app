namespace Foody.Admin.ViewModels.Contact;

public partial class ContactVM : BaseViewModel
{
    public ContactVM(IDataManager dataManager) : base(dataManager)
    {
        Title = nameof(Inquiry);

        Task.Run(GetAll);
    }

    #region Properties
    [ObservableProperty]
    Inquiry selectedInquiry;

    public ObservableCollection<Inquiry> Inquiries { get; } = new();
    #endregion

    #region Commands
    [ICommand]
    async Task GetAll()
    {
        if (!IsBusy)
        {
            try
            {
                IsBusy = true;
                IsRefreshing = true;

                var result = await dataManager.Inquiries.AllAsync();

                if (result is null)
                    throw new NullReferenceException("Could not retrive results");

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Inquiries.Clear();

                    foreach (var c in result.Content)
                        Inquiries.Add(c);
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
    }

    [ICommand]
    void ReadMessage(Inquiry inquiry)
    {
        if (inquiry is null) return;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            SelectedInquiry = null;

            SelectedInquiry = inquiry;
        });
    }
    #endregion
}

