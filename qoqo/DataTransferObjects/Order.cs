using qoqo.Model;

namespace qoqo.DataTransferObjects;

public class OrderDto
{
   public OfferOrderDto Offer { get; set; }
   public OrderStatus Status { get; set; }
   public DateTime CreatedAt { get; set; }
}

public class OrderDashboardDto
{
   public int OrderId { get; set; }
   public OrderStatus Status { get; set; }
   public UserDto user { get; set; }
   public DateTime CreatedAt { get; set; }

   public static OrderDashboardDto FromOrder(Order order)
   {
      return new OrderDashboardDto
      {
         OrderId = order.Id,
         Status = order.Status,
         user = UserDto.FromUser(order.User),
         CreatedAt = order.CreatedAt
      };
   }
}