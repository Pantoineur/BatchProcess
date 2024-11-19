using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;

namespace BatchProcess.Controls;

public class ButtonIcon : TemplatedControl
{
    #region Text
    public static readonly StyledProperty<string> TextProperty = AvaloniaProperty.Register<ButtonIcon, string>(
        nameof(Text));

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    #endregion
    
    #region Icon
    public static readonly StyledProperty<string> IconProperty = AvaloniaProperty.Register<ButtonIcon, string>(
        nameof(Icon));

    public string Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
    #endregion
    
    #region IconOnly
    public static readonly StyledProperty<bool> IconOnlyProperty = AvaloniaProperty.Register<ButtonIcon, bool>(
        nameof(IconOnly));

    public bool IconOnly
    {
        get => GetValue(IconOnlyProperty);
        set => SetValue(IconOnlyProperty, value);
    }
    #endregion
    
    #region Orientation
    public static readonly StyledProperty<Orientation> OrientationProperty = AvaloniaProperty.Register<ButtonIcon, Orientation>(
        nameof(Orientation));

    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }
    #endregion
}