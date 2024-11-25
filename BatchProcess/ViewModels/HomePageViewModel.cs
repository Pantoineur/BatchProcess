using BatchProcess.Data;

namespace BatchProcess.ViewModels;

public partial class HomePageViewModel : PageViewModel
{
    public HomePageViewModel()
    {
        PageName = PageName.Home;
    }
    public string Test => "toto";
}