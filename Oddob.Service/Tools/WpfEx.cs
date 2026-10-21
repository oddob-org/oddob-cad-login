using System.Windows.Interop;
using System.Windows.Media;

namespace Oddob.Service.Tools;

internal static class WpfEx
{
    /// <summary>
    /// 是非模态窗口
    /// </summary>
    /// <param name="window">窗日</param>
    /// <returns>是则返回<c>true</c></returns>
    public static bool IsModelessWindow(this Window window)
    {
        var windowInteropHelper = new WindowInteropHelper(window);
        return windowInteropHelper.Owner == IntPtr.Zero;
    }

    /// <summary>
    /// 设置窗体的DialogResult<br/>
    /// 模态窗体设置DialogResult会触发窗体的Close函数<br/>
    /// 非模态窗体不会设置并返回<c>false</c>
    /// </summary>
    /// <param name="window">窗口</param>
    /// <param name="value">DialogResult值</param>
    /// <returns>设置成功返回<c>true</c></returns>
    public static bool SetDialogResult(this Window window, bool value)
    {
        if (!window.IsModelessWindow())
            return false;
        window.DialogResult = value;
        return true;
    }

    /// <summary>
    /// 查找指定控件的视觉父控件。
    /// </summary>
    /// <typeparam name="T">要查找的父控件类型。</typeparam>
    /// <param name="child">子控件。</param>
    /// <returns>返回找到的父控件，如果未找到则返回null。</returns>
    public static T? FindVisualParent<T>(DependencyObject child) where T : DependencyObject
    {
        // 获取父级控件
        var parentObject = VisualTreeHelper.GetParent(child);

        // 如果父级控件为空，则返回默认值
        if (parentObject == null)
            return null;

        // 检查父级控件是否是我们想要的类型
        if (parentObject is T parentOfType)
        {
            return parentOfType;
        }

        // 继续向上查找
        return FindVisualParent<T>(parentObject);
    }

}