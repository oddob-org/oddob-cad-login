using Oddob.Service.Model.Models;

namespace Oddob.Service.Service.Interface.Component;

/// <summary>
/// 计算机信息服务中心接口
/// </summary>
internal interface IComputerInfoService
{
    /// <summary>
    /// 获取计算机信息
    /// </summary>
    /// <returns>返回计算机信息对象</returns>
    ComputerInfo GetComputerInfo();

    /// <summary>
    /// 计算计算机信息的SHA256哈希值
    /// </summary>
    /// <returns>返回SHA256哈希值字符串</returns>
    string GetMacId();
}
