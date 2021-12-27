using qoqo.Model;

namespace qoqo.DataTransferObjects;

public class OrderDto
{
   public OfferOrderDto Offer { get; set; }
   public OrderStatus Status { get; set; }
   public DateTime CreatedAt { get; set; }
}