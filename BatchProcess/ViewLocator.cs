using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using BatchProcess.ViewModels;

namespace BatchProcess;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param)
    {
        var viewName = param?.GetType().FullName?.Replace("ViewModel", "View");
        if (viewName is null)
            return null;
        
        var viewType = Type.GetType(viewName) ?? throw new Exception($"Could not load view: {viewName}");

        if (Activator.CreateInstance(viewType) is not Control control)
            return null;
        
        control.DataContext = param;
        return control;
    }

    public bool Match(object? data) => data is ViewModelBase;
}