namespace Oddob.Service.Service.Interface.Common;

public interface IStringService
{
    /// <summary>
    /// 判断输入字符串是否与给定的模式匹配。
    /// </summary>
    /// <param name="pattern">模式字符串，其中的*字符表示任意长度的任意字符。</param>
    /// <param name="input">要匹配的输入字符串。</param>
    /// <returns>如果输入字符串与模式匹配，则返回true；否则返回false。</returns>
    bool IsMatch(string pattern, string input);
}