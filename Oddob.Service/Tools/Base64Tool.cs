using System.Windows.Media.Imaging;

namespace Oddob.Service.Tools;

internal static class Base64Tool
{
    public static BitmapImage? ConvertBase64ToBitmapImage(this string base64String)
    {
        try
        {
            base64String = base64String.Replace("data:image/png;base64,", "");
            BitmapImage bitmapImage = new();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            var bytes = Convert.FromBase64String(base64String);
            using var memoryStream = new MemoryStream(bytes);
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.EndInit();
            return bitmapImage;
        }
        catch (Exception)
        {
            return null;
        }
    }
}