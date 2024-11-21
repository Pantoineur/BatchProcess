using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BatchProcess.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    #region Observable Properties
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HomePageActive))]
    [NotifyPropertyChangedFor(nameof(ProcessPageActive))]
    private ViewModelBase _currentPage;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Title))]
    [NotifyPropertyChangedFor(nameof(Width))]
    private bool _menuIsCollapsed;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PinIcon))]
    private bool _isPinned = true;
    
    #endregion

    private HomePageViewModel _homePage = new();
    private ProcessPageViewModel _processPage = new();
    
    public MainViewModel()
    {
        CurrentPage = _homePage;
    }
    
    public bool HomePageActive => CurrentPage == _homePage;
    public bool ProcessPageActive => CurrentPage == _processPage;

    #region Commands

    [RelayCommand]
    public void OpenHomePage()
    {
        CurrentPage = _homePage;
    }

    [RelayCommand]
    public void OpenProcessPage()
    {
        CurrentPage = _processPage;
    }

    [RelayCommand]
    public void ToggleMenuCollapse()
    {
        if(IsPinned) return;
        MenuIsCollapsed = !MenuIsCollapsed;
    }

    [RelayCommand]
    public void PinSideMenu()
    {
        IsPinned = !IsPinned;
    }
    
    #endregion
    
    public string PinIcon => IsPinned ? "\ue65c" : "\ue3e2";
    public string Title => MenuIsCollapsed ? "BP" : "Batch Process";
    public int Width => MenuIsCollapsed ? 80 : 220;
}