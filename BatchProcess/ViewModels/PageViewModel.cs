using BatchProcess.Data;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BatchProcess.ViewModels;

public partial class PageViewModel : ViewModelBase
{
    [ObservableProperty]
    private PageName _pageName;
}