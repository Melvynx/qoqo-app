namespace qoqo.Model;

public class Offer
{
    public int OfferId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public double BarredPrice { get; set; } = 0.0;
    public double Price { get; set; } = 0.0;
    public int ClickObjective { get; set; } = 0;
    public string SpecificationText { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public string? WinnerText { get; set; }
    public bool IsOver { get; set; } = false;
    public bool IsDraft { get; set; } = true;
    public DateTime StartAt { get; set; } 
    public DateTime EndAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public List<Click> Clicks { get; set; }
    public List<Order> Orders { get; set; }

    public int Id => OfferId;
}
