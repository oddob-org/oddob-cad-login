namespace Oddob.Service.Service.Interface.Component;

/// <summary>
/// 拖盘接口
/// </summary>
public interface ITrayItemService
{
    /// <summary>
    /// show气泡通知
    /// </summary>
    /// <param name="second">秒数</param>
    /// <param name="title">标题</param>
    /// <param name="text1">内容1</param>
    /// <param name="hyperText">链接文本</param>
    /// <param name="hyperLink">链接地址</param>
    /// <param name="text2">内容2</param>
    void ShowBubbleWindow(int second, string title, string text1, string? hyperText = null, string? hyperLink = null, string? text2 = null);
}