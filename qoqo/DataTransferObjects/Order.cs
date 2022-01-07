using qoqo.Model;

namespace qoqo.DataTransferObjects;

public class OrderDto
{
    public int OrderId { get; set; }
    public OrderStatus Status { get; set; }
    public UserDto user { get; set; }
    public OfferOrderDto offer { get; set; }

    public static OrderDto FromOrder(Order order)
    {
        return new OrderDto
        {
            OrderId = order.Id,
            Status = order.Status,
            user = UserDto.FromUser(order.User),
            offer = new OfferOrderDto
            {
                OfferId = order.OfferId,
                Title = order.Offer.Title
            }
        };
    }
}

public class OrderViewDto
{
    public int OrderId { get; set; }
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

public class OrderBody
{
    public OrderStatus Status { get; set; }
}