using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace qoqo.Model;

public class Order
{
    public int OrderId { get; set; }
    public OrderStatus Status  { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public int ClickId { get; set; }
    public Click Click { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }

    public int Id => OrderId;
}

[Serializable]
[JsonConverter(typeof(StringEnumConverter))]
public enum OrderStatus
{
    PENDING,
    DELIVERED,
    CANCELLED
}