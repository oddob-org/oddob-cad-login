namespace Oddob.Service.View.Converter;

/// <summary>
/// 枚举转描述
/// </summary>
public class Enum2DescriptionConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
            return DependencyProperty.UnsetValue;
        return GetEnumDescription(value);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter,
        CultureInfo culture)
    {
        return Binding.DoNothing;
    }

    private string GetEnumDescription(object obj)
    {
        var name = obj.ToString() ?? "";
        var fi = obj.GetType().GetField(name);
        var customAttribute = fi?.GetCustomAttribute<DescriptionAttribute>();
        return customAttribute?.Description ?? name;
    }
}