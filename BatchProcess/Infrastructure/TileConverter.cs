using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace BatchProcess.Infrastructure;

public class TileConverter : IValueConverter
{
    private static Dictionary<int, Bitmap> _cache;

    public TileConverter()
    {
        
    }
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return GetCache();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private Dictionary<int, Bitmap> GetCache()
    {
        EnsureCache();

        return _cache;
    }

    private void EnsureCache()
    {
        _cache = new Dictionary<int, Bitmap>();
        // TODO: Read file
    }
}