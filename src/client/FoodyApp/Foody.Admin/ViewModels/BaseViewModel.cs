using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Foody.Admin.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(IsNotBusy))]
    bool isBusy;

    [ObservableProperty]
    string title;

    protected readonly IDataManager dataManager;
    //protected readonly IConnectivity connectivity;

    public BaseViewModel(IDataManager dataManager)
    {
        this.dataManager = dataManager;
    }

    public bool IsNotBusy => !IsBusy;
}

