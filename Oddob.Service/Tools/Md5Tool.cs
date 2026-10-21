namespace Oddob.Service.Tools;

/// <summary>
/// MD5工具类
/// </summary>
internal static class Md5Tool
{
    /// <summary>
    /// 获取字符串的MD5值
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <returns>MD5值</returns>
    public static string GetMd5Hash(this string input)
    {
        using var md5 = System.Security.Cryptography.MD5.Create();
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = md5.ComputeHash(inputBytes);
        var sb = new StringBuilder();
        foreach (var b in hashBytes)
        {
            sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }
}