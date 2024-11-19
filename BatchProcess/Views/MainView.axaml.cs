using Avalonia.Controls;
using Avalonia.Input;
using BatchProcess.ViewModels;

namespace BatchProcess;

public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();
    }

    private void SideMenu_PointerEvent(object? sender, PointerEventArgs e)
    {
        (DataContext as MainViewModel)?.ToggleMenuCollapse();
    }
}