namespace Oddob.Service.View.Converter;

/// <summary>
/// 通过bool值选择字符串
/// </summary>
public class Bool2StringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue
            && parameter is string str)
        {
            var arr = str.Split(';');
            if (arr.Length > 1)
            {
                return boolValue ? arr[1] : arr[0];
            }
        }

        return value ?? "";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}