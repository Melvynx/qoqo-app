using qoqo.Model;

namespace qoqo.DataTransferObjects;

public class OrderDto
{
   public OfferOrderDto Offer { get; set; }
   public ClickOrderDto Click { get; set; }
   public OrderStatus Status { get; set; }
}