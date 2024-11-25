using System;
using BatchProcess.Data;
using BatchProcess.ViewModels;

namespace BatchProcess.Factories;

public class PageFactory
{
    private readonly Func<PageName, PageViewModel> _pageViewModelFactory;

    public PageFactory(Func<PageName, PageViewModel> pageViewModelFactory)
    {
        _pageViewModelFactory = pageViewModelFactory;
    }
    
    public PageViewModel CreatePage(PageName pageName) => _pageViewModelFactory(pageName);
}