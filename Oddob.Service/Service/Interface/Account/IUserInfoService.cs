namespace Oddob.Service.Service.Interface.Account;

internal interface IUserInfoService
{
    bool IsLogin { get; }
    Task<bool> GetIsLogin();

    /// <summary>
    /// 检测产品
    /// </summary>
    /// <param name="productName">名字</param>
    /// <returns>成功返回 <c>true</c></returns>
    bool CheckProduct(string productName);

    /// <summary>
    /// 刷新用户信息
    /// </summary>
    void Refresh();

    /// <summary>
    /// 刷新用户信息
    /// </summary>
    Task RefreshAsync();

    /// <summary>
    /// 获取产品列表
    /// </summary>
    List<Product> GetProducts();
}