namespace qoqo.DataTransferObjects;

public class OfferDto
{
    public int OfferId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public double BarredPrice { get; set; } = 0.0;
    public double Price { get; set; } = 0.0;
    public int ClickObjective { get; set; } = 0;
    public string SpecificationText { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public bool IsDraft { get; set; } = true;
    public bool IsOver { get; set; } = false;
    public string? WinnerText { get; set; } = "";
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class OfferClickDto
{
    public int Click { get; set; }
    public int RemainingTime { get; set; }
    public int UserId { get; set; }
}

public class OfferOrderDto
{
    public int OfferId { get; set; }
    public string Title { get; set; }
}

public class DashboardDto
{
    public int OfferId { get; set; }
    public string OfferTitle { get; set; }
    public bool IsOver { get; set; }
    public int? ClickCount { get; set; }
    public int? ClickObjective { get; set; }
    public int? CountOfActiveUser { get; set; }
    public DateTime? EndAt { get; set; }
    public DateTime? StartAt { get; set; }
    public OrderDashboardDto? Order { get; set; }
    public bool IsNextOffer { get; set; }

    public static DashboardDto FromOfferDto(OfferDto offer, bool isNextOffer)
    {
        return new DashboardDto
        {
            OfferId = offer.OfferId,
            OfferTitle = offer.Title,
            IsOver = offer.IsOver,
            EndAt = offer.EndAt,
            StartAt = offer.StartAt,
            ClickObjective = offer.ClickObjective,
            IsNextOffer = isNextOffer
        };
    }
}

public class OfferIndexDto
{
    public int OfferId { get; set; }
    public string Title { get; set; }
    public int ClickObjective { get; set; }
    public bool IsOver { get; set; }
    public bool IsLive { get; set; }
    public bool IsDraft { get; set; }
}

public class OfferBody
{
    public string Title { get; set; }
    public string Description { get; set; }
    public double BarredPrice { get; set; }
    public double Price { get; set; }
    public int ClickObjective { get; set; }
    public string SpecificationText { get; set; }
    public string ImageUrl { get; set; }
    public bool IsDraft { get; set; }
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
}