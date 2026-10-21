namespace Oddob.Service.Service.Interface;

/// <summary>
/// 定义对象映射服务的接口，提供对象之间的映射功能
/// </summary>
internal interface IMapperService
{
    /// <summary>
    /// 将源对象映射到目标对象
    /// </summary>
    /// <param name="source">源对象</param>
    /// <param name="destiny">目标对象</param>
    /// <returns>映射后的目标对象</returns>
    object MapTo(object source, object destiny);

    /// <summary>
    /// 将对象映射到指定类型的对象
    /// </summary>
    /// <typeparam name="TDestiny">目标对象的类型</typeparam>
    /// <param name="obj">源对象</param>
    /// <returns>映射后的目标对象</returns>
    TDestiny MapTo<TDestiny>(object obj);
}
