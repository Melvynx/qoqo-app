using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qoqo.DataTransferObjects;
using qoqo.Model;
using qoqo.Ressources;
using qoqo.Services;

namespace qoqo.Providers;

public class OfferProvider
{
    private readonly QoqoContext _context;

    public OfferProvider(QoqoContext context)
    {
        _context = context;
    }

    public async Task<List<OfferIndexDto>> GetOffers()
    {
        var today = DateTime.Now;
        return await _context.Offers.Select(o => new OfferIndexDto
        {
            OfferId = o.OfferId,
            Title = o.Title,
            IsDraft = o.IsDraft,
            IsOver = o.IsOver,
            IsLive = o.StartAt <= today && o.EndAt >= today && !o.IsDraft,
            ClickObjective = o.ClickObjective
        }).ToListAsync();
    }

    public async Task<OfferDto?> GetOffer(int id)
    {
        return await GetOfferDto().FirstOrDefaultAsync(o => o.OfferId == id);
    }

    public async Task<OfferDto?> GetCurrentOffer()
    {
        var today = DateTime.Today;
        var offer = await GetOfferDto().FirstOrDefaultAsync(o =>
            o.StartAt <= today && o.EndAt >= today && !o.IsDraft);

        return offer;
    }

    public async Task<DashboardDto?> GetDashboard()
    {
        var offer = await GetCurrentOffer();
        if (offer == null)
        {
            var nextOffer = await GetOfferDto().FirstOrDefaultAsync(o =>
                o.StartAt > DateTime.Today && !o.IsDraft);
            return nextOffer == null ? null : DashboardDto.FromOfferDto(nextOffer, true);
        }


        var clickCount = await _context.Clicks.Where(o => o.OfferId == offer.OfferId).CountAsync();
        var activeUserCount =
            await _context.Clicks.Where(o => o.OfferId == offer.OfferId).GroupBy(c => c.UserId).CountAsync();
        var order = await _context.Orders
            .Where(o => o.OfferId == offer.OfferId)
            .Include(o => o.User)
            .Select(o => OrderDashboardDto.FromOrder(o))
            .FirstOrDefaultAsync();

        var dashboard = DashboardDto.FromOfferDto(offer, false);
        dashboard.ClickCount = clickCount;
        dashboard.Order = order;
        dashboard.CountOfActiveUser = activeUserCount;

        return dashboard;
    }

    public async Task<ActionResult> CreateOffer(OfferBody offerBody)
    {
        var offer = Offer.FromOfferBody(offerBody);
        var errors = Validate(offer);
        if (errors.Any()) return ErrorService.BadRequest(string.Join(", ", errors));

        var entity = await _context.Offers.AddAsync(offer);
        await _context.SaveChangesAsync();
        return new OkObjectResult(entity.Entity);
    }

    // update offer
    public async Task<ActionResult> UpdateOffer(int id, OfferBody offerBody)
    {
        var offer = await _context.Offers.FindAsync(id);
        if (offer == null) return ErrorService.BadRequest(StringRes.ErrorDuringOfferUpdate);

        offer.Title = offerBody.Title;
        offer.Description = offerBody.Description;
        offer.IsDraft = offerBody.IsDraft;
        offer.Price = offerBody.Price;
        offer.BarredPrice = offerBody.BarredPrice;
        offer.ClickObjective = offerBody.ClickObjective;
        offer.ImageUrl = offerBody.ImageUrl;
        offer.EndAt = offerBody.EndAt;
        offer.StartAt = offerBody.StartAt;
        offer.SpecificationText = offerBody.SpecificationText;

        var errors = Validate(offer);
        if (errors.Any()) return ErrorService.BadRequest(string.Join(", ", errors));

        await _context.SaveChangesAsync();
        return SuccessService.Ok(StringRes.OfferUpdated);
    }

    private List<string> Validate(Offer offer)
    {
        if (offer.IsDraft) return new List<string>();

        var errors = offer.Validate();
        var sameTimeOffer = _context.Offers.FirstOrDefault(o =>
            o.OfferId != offer.OfferId && !o.IsDraft &&
            (offer.StartAt >= o.StartAt && offer.StartAt <= o.EndAt ||
             offer.EndAt >= o.StartAt && offer.StartAt <= o.EndAt)
        );
        if (sameTimeOffer != null)
            errors.Add($"{StringRes.OfferSameTime}( {sameTimeOffer.Id}: {sameTimeOffer.Title} )");

        return errors;
    }

    private IQueryable<OfferDto> GetOfferDto()
    {
        return _context.Offers.Select(o => new OfferDto
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
    }
}