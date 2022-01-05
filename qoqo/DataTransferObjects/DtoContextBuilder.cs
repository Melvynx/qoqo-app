using qoqo.Model;
using SQLitePCL;

namespace qoqo.DataTransferObjects;

public class DtoContextBuilder
{
    private readonly QoqoContext _context;
    public readonly OfferDtoBuilder Offer;

    public DtoContextBuilder(QoqoContext context)
    {
        _context = context;
        Offer = new OfferDtoBuilder(context);
    }

}

public class OfferDtoBuilder
{
    private readonly QoqoContext _context;

    public OfferDtoBuilder(QoqoContext context)
    {
        _context = context;
    }

    public IQueryable<OfferDto> Default => _context.Offers.Select(o => new OfferDto
    {
        OfferId = o.OfferId,
        Title = o.Title,
        Description = o.Description,
        IsDraft = o.IsDraft,
        IsOver = o.IsOver,
        ClickObjective = o.ClickObjective,
        SpecificationText = o.SpecificationText,
        StartAt = o.StartAt,
        EndAt = o.EndAt,
        CreatedAt = o.CreatedAt,
        ImageUrl = o.ImageUrl,
        Price = o.Price,
        BarredPrice = o.BarredPrice,
        WinnerText = o.WinnerText
    });

    public IQueryable<OfferIndexDto> Index
    {
        get
        {
            var today = DateTime.Now;
            return _context.Offers.Select(o => new OfferIndexDto
            {
                OfferId = o.OfferId,
                Title = o.Title,
                IsDraft = o.IsDraft,
                IsOver = o.IsOver,
                IsLive = o.StartAt <= today && o.EndAt >= today && !o.IsDraft,
                ClickObjective = o.ClickObjective
            });
        }
    }
}