using qoqo.DataTransferObjects;
using qoqo.Ressources;

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
    public DateTime? StartAt { get; set; } 
    public DateTime? EndAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public List<Click> Clicks { get; set; }
    public List<Order> Orders { get; set; }

    public int Id => OfferId;

    public static Offer FromOfferBody(OfferBody offerBody)
    {
        return new Offer
        {
            Title = offerBody.Title,
            Description = offerBody.Description,
            BarredPrice = offerBody.BarredPrice,
            Price = offerBody.Price,
            ClickObjective = offerBody.ClickObjective,
            SpecificationText = offerBody.SpecificationText,
            ImageUrl = offerBody.ImageUrl,
            StartAt = offerBody.StartAt,
            EndAt = offerBody.EndAt
        };
    }

    public List<string> Validate()
    {
        var errors = new List<string>();
        if (StartAt == null)
        {
            errors.Add(StringRes.OfferStartAtDateRequired);
        }

        if (EndAt == null)
        {
            errors.Add(StringRes.OfferEndAtDateRequired);
        }

        if (EndAt < StartAt)
        {
            errors.Add(StringRes.OfferEndAtBeforeStartAt);
        }

        if (ClickObjective < 10)
        {
            errors.Add(StringRes.OfferClickObjectiveMustBeUpperThan10);
        }

        return errors;
    }
}
