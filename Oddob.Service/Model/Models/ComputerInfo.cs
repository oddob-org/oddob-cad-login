namespace Oddob.Service.Model.Models;

/// <summary>
/// 计算机信息
/// </summary>
public class ComputerInfo
{
    /// <summary>
    /// Cpu 序列号
    /// </summary>
    public string CpuProcessorId { get; set; } = "";

    /// <summary>
    /// 主板序列号
    /// </summary>
    public string MotherBoardSerialNumber { get; set; } = "";

    /// <summary>
    /// 系统Guid
    /// </summary>
    public string SystemGuid { get; set; } = "";

    /// <summary>
    /// 硬盘驱动序列号
    /// </summary>
    public string DriveSerialNumber { get; set; } = "";

    /// <summary>
    /// 网卡物理地址
    /// </summary>
    public string NetworkPhysicalAddress { get; set; } = "";

    /// <summary>
    /// 运算后的D盘序列号
    /// </summary>
    public string SerialNumberDCal { get; set; } = "";
}