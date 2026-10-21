namespace Oddob.Service.View.WpfTheme;

public partial class PanuonUiTheme : ObservableObject
{
    public static PanuonUiTheme Instance { get; } = new();

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(ThemeUri))]
    private bool _isLightTheme = true;

    public Uri ThemeUri => new Uri("pack://application:,,,/Oddob.Service;component/View/WpfTheme/PanuonUiThemeDefaultResource.xaml");
}

public static class WpfUiThemeEx
{
    /// <summary>
    /// 使用过主题的控件对象
    /// </summary>
    private static readonly HashSet<ContentControl> _contentControls = [];

    public static void LoadWpfUiTheme(this ContentControl contentControl)
    {
        // 刷新主题
        contentControl.Loaded -= ContentControlOnLoaded;
        contentControl.Loaded += ContentControlOnLoaded;
        _contentControls.Add(contentControl);
    }

    private static void ContentControlOnLoaded(object sender, RoutedEventArgs e)
    {
        ((ContentControl)sender).ResetWpfUiTheme();
    }

    private static void ResetWpfUiTheme(this ContentControl contentControl)
    {
        var collection = contentControl.Resources.MergedDictionaries;
        for (var i = collection.Count - 1; i >= 0; i--)
        {
            var resourcesMergedDictionary = collection[i];
            // 删除原来的主题资源
            if (resourcesMergedDictionary.Source.ToString().Contains("PanuonUiTheme"))
            {
                contentControl.Resources.MergedDictionaries.RemoveAt(i);
            }
        }

        // 添加新的主题资源
        contentControl.Resources.MergedDictionaries.Add(new()
        {
            Source = PanuonUiTheme.Instance.ThemeUri,
        });
    }

    /// <summary>
    /// 修改主题
    /// </summary>
    public static void ChangeWpfUiTheme(this ContentControl contentControl)
    {
        // 先修改反转主题bool
        PanuonUiTheme.Instance.IsLightTheme ^= true;
        // 建个列表，用于装已经失效的控件
        List<ContentControl> removeList = new();
        contentControl.ResetWpfUiTheme();
        // 每个循环修改
        foreach (var item in _contentControls)
        {
            try
            {
                ResetWpfUiTheme(item);
            }
            catch
            {
                removeList.Add(item);
            }
        }

        // 最后把失效的干掉
        foreach (var element in removeList)
        {
            _contentControls.Remove(element);
        }
    }
}