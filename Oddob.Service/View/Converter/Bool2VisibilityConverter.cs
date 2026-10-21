namespace Oddob.Service.View.Converter;

/// <summary>
/// bool值转为可见性
/// </summary>
public class Bool2VisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool b)
        {
            return DependencyProperty.UnsetValue;
        }
        return b? Visibility.Visible : Visibility.Collapsed;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}