using Mapster;
using MapsterMapper;

namespace Oddob.Service.Service.Implement;

/// <summary>
/// MapperService类实现了IMapperService接口，提供对象之间的映射功能
/// 使用SingletonDI属性标记此类型为单例模式，确保应用程序中只有一个实例
/// </summary>
[SingletonDI(typeof(IMapperService))]
internal sealed class MapperService : IMapperService
{
    /// <summary>
    /// 构造函数初始化MapperService实例
    /// </summary>
    public MapperService()
    {
        // 创建TypeAdapterConfig实例，用于配置映射器
        var typeAdapterConfig = new TypeAdapterConfig();
        // 扫描指定的程序集数组，自动发现和注册映射关系
        typeAdapterConfig.Scan(Initialize.Assemblies);
        // 使用配置创建Mapper实例
        _mapper = new Mapper(typeAdapterConfig);
    }

    /// <summary>
    /// 存储IMapper接口的实例，用于执行映射操作
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// 将源对象映射到目标对象
    /// </summary>
    /// <param name="source">源对象</param>
    /// <param name="destiny">目标对象</param>
    /// <returns>映射后的目标对象</returns>
    public object MapTo(object source, object destiny)
    {
        return _mapper.Map(source, destiny, source.GetType(), destiny.GetType());
    }

    /// <summary>
    /// 将对象映射到指定类型的对象
    /// </summary>
    /// <typeparam name="TDestiny">目标对象的类型</typeparam>
    /// <param name="obj">源对象</param>
    /// <returns>映射后的目标对象</returns>
    public TDestiny MapTo<TDestiny>(object obj)
    {
        return _mapper.Map<TDestiny>(obj);
    }
}
