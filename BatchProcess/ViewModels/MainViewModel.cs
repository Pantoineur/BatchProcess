using System;
using BatchProcess.Data;
using BatchProcess.Factories;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BatchProcess.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly PageFactory _pageFactory;

    #region Observable Properties
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HomePageActive))]
    [NotifyPropertyChangedFor(nameof(MapCreatorPageActive))]
    private PageViewModel _currentPage;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Title))]
    [NotifyPropertyChangedFor(nameof(Width))]
    private bool _menuIsCollapsed;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PinIcon))]
    private bool _isPinned = true;
    
    #endregion
    
    public MainViewModel(PageFactory pageFactory)
    {
        _pageFactory = pageFactory;
        CurrentPage = _pageFactory.CreatePage(PageName.Home);
    }
    
    public bool HomePageActive => CurrentPage.PageName == PageName.Home;
    public bool MapCreatorPageActive => CurrentPage.PageName == PageName.MapCreator;
    public bool ThemeCreatorPageActive => CurrentPage.PageName == PageName.ThemeCreator;

    #region Commands

    [RelayCommand]
    public void OpenHomePage()
    {
        CurrentPage = _pageFactory.CreatePage(PageName.Home);
    }

    [RelayCommand]
    public void OpenMapCreatorPage()
    {
        CurrentPage = _pageFactory.CreatePage(PageName.MapCreator);
    }

    [RelayCommand]
    public void OpenThemeCreatorPage()
    {
        CurrentPage = _pageFactory.CreatePage(PageName.ThemeCreator);
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