using System.Windows.Media;

namespace Oddob.Service.Model.Models;

internal sealed partial class UserInfoModel : ObservableObject
{
    [ObservableProperty]
    public partial ImageSource? HeadImage { get; set; }

    [ObservableProperty]
    public partial string Litpic { get; set; } = "";

    [NotifyPropertyChangedFor(nameof(TypeString))]
    [ObservableProperty]
    public partial int Type { get; set; }

    public string TypeString
    {
        get
        {
            return Type switch
            {
                1 => "企业主账号",
                2 => "企业子账号",
                _ => "个人版",
            };
        }
    }

    [ObservableProperty]
    public partial string Username { get; set; } = "";

    [ObservableProperty]
    public partial int Score { get; set; }

    [ObservableProperty]
    public partial int Level { get; set; }

    [ObservableProperty]
    public partial string Money { get; set; } = "0.00";
}