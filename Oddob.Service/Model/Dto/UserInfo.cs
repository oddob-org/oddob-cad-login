namespace Oddob.Service.Model.Dto;

public class UserInfo
{
    [JsonProperty("uid")]
    public string Uid { get; set; } = "";

    [JsonProperty("type")]
    public int Type { get; set; }

    [JsonProperty("username")]
    public string Username { get; set; } = "";

    [JsonProperty("macId")]
    public string MacId { get; set; } = "";

    [JsonProperty("tel")]
    public string Tel { get; set; } = "";

    [JsonProperty("token_expire_time")]
    public long TokenExpireTime { get; set; }

    [JsonProperty("created_at")]
    public string CreatedAt { get; set; } = "";

    [JsonProperty("updated_at")]
    public string UpdatedAt { get; set; } = "";

    [JsonProperty("is_delete")]
    public int IsDelete { get; set; }

    [JsonProperty("ip")]
    public string Ip { get; set; } = "";

    [JsonProperty("role")]
    public string Role { get; set; } = "";

    [JsonProperty("email")]
    public string Email { get; set; } = "";

    [JsonProperty("litpic")]
    public string Litpic { get; set; } = "";

    [JsonProperty("money")]
    public string Money { get; set; } = "";

    [JsonProperty("sex")]
    public int Sex { get; set; }

    [JsonProperty("about")]
    public string About { get; set; } = "";

    [JsonProperty("login_time")]
    public DateTime LoginTime { get; set; }

    [JsonProperty("level")]
    public int Level { get; set; }

    [JsonProperty("score")]
    public int Score { get; set; }

    [JsonProperty("gid")]
    public int Gid { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; } = "";

    [JsonProperty("products")]
    public List<Product> ProductsJson { get; set; } = [];

    [JsonProperty("current_product")]
    public Product? CurrentProduct { get; set; }

    // [JsonIgnore]
    public List<Product> Products { get; set; } = [];
}

public class Product
{
    [JsonProperty("code")]
    public string Code { get; set; } = "";

    [JsonProperty("name")]
    public string Name { get; set; } = ""; // 产品名称

    [JsonProperty("logo")]
    public string Logo { get; set; } = ""; // 产品图片

    [JsonProperty("price")]
    public string Price { get; set; } = ""; // 产品价格

    [JsonProperty("expire_day")]
    public int ExpireDay { get; set; } // 产品购买有效期（天）

    [JsonProperty("overtime")]
    public string Overtime { get; set; } = ""; // 有效日期

    [JsonIgnore]
    public string OvertimeDate => string.IsNullOrWhiteSpace(Overtime) ? "长期有效" : Overtime;

    [JsonProperty("expire")]
    public long Expire { get; set; } // 有效期时间戳

    [JsonProperty("isover")]
    public bool IsOver { get; set; } = true; // 是否过期

    [JsonIgnore]
    public DateTime ExpireTime => DateTimeOffset.FromUnixTimeSeconds(Expire).DateTime;
}