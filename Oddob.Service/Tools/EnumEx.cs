namespace Oddob.Service.Tools;

/// <summary>
/// 枚举扩展
/// </summary>
public static class EnumEx
{
    /// <summary>
    /// 获取枚举的描述内容
    /// </summary>
    /// <remarks>不按位运算的情况下,直接获取比较快捷</remarks>
    /// <param name="e"></param>
    /// <returns></returns>
    public static string GetDescription(this Enum e)
    {
        return GetDescription(e.GetType(), e.ToString());
    }

    /// <summary>
    /// 获取字段的描述内容
    /// </summary>
    /// <param name="type"></param>
    /// <param name="field"></param>
    /// <returns></returns>
    public static string GetDescription(this Type type, string field)
    {
        var memberInfo = type.GetMember(field);
        var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
        // 如果没有定义描述,就把当前枚举值的对应名称返回
        return attributes.Length != 1
            ? field
            : ((DescriptionAttribute)attributes.Single()).Description;
    }
}