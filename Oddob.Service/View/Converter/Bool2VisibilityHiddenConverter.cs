namespace Oddob.Service.View.Converter;

public class Bool2VisibilityHiddenConverter: IValueConverter
{
    
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool b)
        {
            return DependencyProperty.UnsetValue;
        }
        return b? Visibility.Visible : Visibility.Hidden;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}