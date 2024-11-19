using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BatchProcess.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public string Title => MenuIsCollapsed ? "BP" : "Batch Process";
    public int Width => MenuIsCollapsed ? 80 : 220;

    public bool IsPinned
    {
        get;
        set;
    }
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Title))]
    [NotifyPropertyChangedFor(nameof(Width))]
    private bool _menuIsCollapsed = true;

    [RelayCommand]
    public void ToggleMenuCollapse()
    {
        if(IsPinned) return;
        MenuIsCollapsed = !MenuIsCollapsed;
    }
}