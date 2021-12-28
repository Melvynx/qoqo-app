using qoqo.Model;

namespace qoqo.DataTransferObjects;

public class OfferClickDto
{
    public int Click { get; set; }
    public int RemainingTime { get; set; }
    public int UserId { get; set; }
}

public class OfferOrderDto
{
    public string Title { get; set; }
    public int Id { get; set; }
}

public class DashboardDto
{
    public int OfferId { get; set; }
    public string OfferTitle { get; set; }
    public bool IsOver { get; set; }
    public int ClickCount { get; set; }
    public int ClickObjective { get; set; }
    public int CountOfActiveUser { get; set; }
    public DateTime EndAt { get; set; }
    public OrderDashboardDto? Order { get; set; }
}