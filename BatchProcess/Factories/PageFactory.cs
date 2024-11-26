using System;
using BatchProcess.Data;
using BatchProcess.ViewModels;

namespace BatchProcess.Factories;

public class PageFactory
{
    private readonly Func<PageName, PageViewModel>? _pageViewModelFactory;

    public PageFactory()
    {
        
    }
    
    public PageFactory(Func<PageName, PageViewModel> pageViewModelFactory)
    {
        _pageViewModelFactory = pageViewModelFactory;
    }

    public PageViewModel CreatePage(PageName pageName)
    {
        if (_pageViewModelFactory == null)
        {
            return pageName switch
            {
                PageName.Home => new HomePageViewModel(),
                PageName.MapCreator => new MapCreatorPageViewModel(),
                PageName.ThemeCreator => new ThemeCreatorPageViewModel(),
                _ => throw new ArgumentOutOfRangeException(nameof(pageName), pageName, null)
            };
        }
        
        return _pageViewModelFactory.Invoke(pageName);
    } 
}