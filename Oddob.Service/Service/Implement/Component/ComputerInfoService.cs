using System.Management;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using Microsoft.Win32;
using Oddob.Service.Model.Models;

namespace Oddob.Service.Service.Implement.Component;

/// <summary>
/// 提供获取计算机硬件和系统信息的服务。
/// </summary>
[SingletonDI(typeof(IComputerInfoService))]
internal sealed class ComputerInfoService : IComputerInfoService
{
    /// <summary>
    /// 静态对象，用于存储计算过的计算机信息，避免重复计算。
    /// </summary>
    private static readonly ComputerInfo _computerInfo = new();

    /// <summary>
    /// 获取当前计算机的详细信息，包括CPU序列号、主板序列号、系统Guid、D盘序列号（经过运算）和网卡物理地址。
    /// </summary>
    /// <returns><see cref="ComputerInfo"/>对象，包含了计算机的详细信息。</returns>
    public ComputerInfo GetComputerInfo()
    {
        // 如果D盘序列号为空，计算并设置
        if (string.IsNullOrWhiteSpace(_computerInfo.SerialNumberDCal))
        {
            _computerInfo.SerialNumberDCal = GetSerialNumberDCal();
        }

        // 如果CPU序列号为空，计算并设置
        if (string.IsNullOrWhiteSpace(_computerInfo.CpuProcessorId))
        {
            _computerInfo.CpuProcessorId = GetCpuProcessorId();
        }

        // 如果主板序列号为空，计算并设置
        if (string.IsNullOrWhiteSpace(_computerInfo.MotherBoardSerialNumber))
        {
            _computerInfo.MotherBoardSerialNumber = GetMotherBoardSerialNumber();
        }

        // 如果系统Guid为空，计算并设置
        if (string.IsNullOrWhiteSpace(_computerInfo.SystemGuid))
        {
            _computerInfo.SystemGuid = GetSystemGuid();
        }

        // 如果硬盘驱动序列号为空，计算并设置
        if (string.IsNullOrWhiteSpace(_computerInfo.DriveSerialNumber))
        {
            _computerInfo.DriveSerialNumber = GetDriveSerialNumber();
        }

        // 如果网卡物理地址为空，计算并设置
        if (string.IsNullOrWhiteSpace(_computerInfo.NetworkPhysicalAddress))
        {
            _computerInfo.NetworkPhysicalAddress = GetNetworkPhysicalAddress();
        }

        return _computerInfo;
    }

    public string GetMacId()
    {
        var computerInfo = GetComputerInfo();
        List<string> strList =
        [
            computerInfo.CpuProcessorId, computerInfo.DriveSerialNumber,
            computerInfo.MotherBoardSerialNumber,
        ];
        // 将字符串拼接在一起
        var combinedStr = string.Join("|", strList);

        // 创建SHA256实例
        using var sha256 = SHA256.Create();
        // 计算哈希值
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedStr));

        // 将哈希值转换为十六进制字符串
        StringBuilder sb = new();
        foreach (var t in hashBytes)
        {
            sb.Append(t.ToString("x2"));
        }

        return sb.ToString().Substring(0, sb.Length > 50 ? 50 : sb.Length);
    }

    /// <summary>
    /// 从系统管理类中获取CPU序列号。
    /// </summary>
    /// <returns>CPU序列号，如果无法获取则返回空字符串。</returns>
    private string GetCpuProcessorId()
    {
        try
        {
            // 使用ManagementClass查询win32_processor类的所有实例
            using var mc = new ManagementClass("win32_processor");
            using var moc = mc.GetInstances();

            // 遍历实例，获取第一个可用的CPU ID或序列号
            foreach (var mo in moc.OfType<ManagementObject>())
            {
                var cpuProcessorId = mo.Properties["processorID"].Value.ToString();
                if (!string.IsNullOrWhiteSpace(cpuProcessorId))
                {
                    return cpuProcessorId;
                }
            }
        }
        catch
        {
            // 忽略错误
        }

        return "";
    }

    /// <summary>
    /// 从注册表中获取系统的全局唯一标识符（Guid）。
    /// </summary>
    /// <returns>系统Guid，如果无法获取则返回空字符串。</returns>
    private string GetSystemGuid()
    {
        try
        {
            // 打开注册表的指定键
            using var rk = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default)
                .OpenSubKey(@"SOFTWARE\Microsoft\Cryptography");

            // 获取MachineGuid值
            if (rk?.GetValue("MachineGuid") is string guid && !string.IsNullOrEmpty(guid))
            {
                return guid;
            }
        }
        catch
        {
            // 忽略错误
        }

        return "";
    }

    /// <summary>
    /// 通过WMI查询获取主板序列号。
    /// </summary>
    /// <returns>主板序列号，如果无法获取则返回空字符串。</returns>
    private string GetMotherBoardSerialNumber()
    {
        var motherBoardSerialNumber = "";
        try
        {
            // 使用ManagementObjectSearcher查询Win32_BaseBoard类
            using var searcher = new ManagementObjectSearcher($"""SELECT * FROM Win32_BaseBoard""");
            foreach (var mo in searcher.Get().OfType<ManagementObject>())
            {
                motherBoardSerialNumber = mo["SerialNumber"]?.ToString()?.Trim() ?? "";
                break; // 只取第一个匹配项
            }
        }
        catch
        {
            // 忽略错误
        }

        return motherBoardSerialNumber;
    }

    /// <summary>
    /// 获取所有硬盘驱动器的序列号，通常返回第一个找到的硬盘序列号。
    /// </summary>
    /// <returns>硬盘驱动序列号，如果无法获取则返回空字符串。</returns>
    private string GetDriveSerialNumber()
    {
        var driveSerialNumber = "";
        try
        {
            // 使用ManagementObjectSearcher查询Win32_PhysicalMedia类
            using var searcher =
                new ManagementObjectSearcher($"""SELECT * FROM Win32_PhysicalMedia""");
            foreach (var mo in searcher.Get().OfType<ManagementObject>())
            {
                driveSerialNumber = mo["SerialNumber"]?.ToString()?.Trim() ?? "";
                break; // 只取第一个匹配项
            }
        }
        catch
        {
            // 忽略错误
        }

        return driveSerialNumber;
    }

    /// <summary>
    /// 获取D盘的序列号，并对其进行特定的计算（可能用于加密或验证）。
    /// </summary>
    /// <returns>经过计算的D盘序列号，如果无法获取或计算失败则返回空字符串。</returns>
    private string GetSerialNumberDCal()
    {
        try
        {
            // 使用ManagementObject查询D盘（逻辑磁盘）
            using var disk = new ManagementObject("win32_logicaldisk.deviceid=\"D:\"");
            disk.Get();

            // 获取卷序列号并转换为16进制整数
            var number16 = disk.GetPropertyValue("VolumeSerialNumber").ToString();
            var snD = Convert.ToInt32(number16, 16);

            // 进行特定的数学运算
            var code =
                Convert.ToInt64(Math.Floor(Math.Pow((Math.Abs(snD) % 1e8 + 17426.0) * 1742647821,
                    0.617) % 1e8));
            return code.ToString();
        }
        catch
        {
            // 忽略错误
        }

        return "";
    }

    /// <summary>
    /// 通过网络接口信息获取网卡的物理地址（MAC地址）。
    /// </summary>
    /// <returns>网卡物理地址，如果无法获取则返回空字符串。</returns>
    private string GetNetworkPhysicalAddress()
    {
        var macAddress = "";
        try
        {
            // 遍历所有网络接口
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                macAddress = ni.GetPhysicalAddress().ToString();
                break; // 只取第一个匹配项
            }
        }
        catch
        {
            // 忽略错误
        }

        return macAddress;
    }
}