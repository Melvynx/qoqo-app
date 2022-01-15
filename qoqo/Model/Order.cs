using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace qoqo.Model;

[Index(nameof(OfferId), IsUnique = true)]
public class Order
{
    public int OrderId { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int OfferId { get; set; }
    public Offer Offer { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}

[Serializable]
[JsonConverter(typeof(StringEnumConverter))]
public enum OrderStatus
{
    PENDING,
    DELIVERED,
    CANCELLED
}