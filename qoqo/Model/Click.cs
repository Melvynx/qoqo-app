namespace qoqo.Model;

public class Click
{
    public int ClickId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public int OfferId { get; set; }
    public Offer Offer { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
    
    public List<Order> Orders { get; set; }

    public int Id => ClickId;
}