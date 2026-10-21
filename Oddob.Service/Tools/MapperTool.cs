namespace Oddob.Service.Tools;

/// <summary>
/// 映射工具类
/// </summary>
internal static class MapperTool
{
    private static readonly IMapperService _mapper = AppHelper.Get<IMapperService>();

    /// <summary>
    /// 映射到
    /// </summary>
    /// <param name="source">源</param>
    /// <param name="destiny">目标</param>
    /// <returns>目标类型</returns>
    public static object MapTo(this object source, object destiny)
    {
        return _mapper.MapTo(source, destiny);
    }

    /// <summary>
    /// 映射为
    /// </summary>
    /// <param name="obj">源</param>
    /// <typeparam name="TDestiny">目标类型</typeparam>
    /// <returns>目标对象</returns>
    public static TDestiny MapTo<TDestiny>(this object obj)
    {
        return _mapper.MapTo<TDestiny>(obj);
    }
}