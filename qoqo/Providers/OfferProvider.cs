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

    public async Task<List<OfferIndex>> GetOffers()
    {
        var today = DateTime.Now;
        return await _context.Offers.Select(o => new OfferIndex
        {
            Id = o.OfferId,
            Title = o.Title,
            IsDraft = o.IsDraft,
            IsOver = o.IsOver,
            IsLive = o.StartAt <= today && o.EndAt >= today && !o.IsDraft,
            ClickObjective = o.ClickObjective
        }).ToListAsync();
    }

    public async Task<OfferDto?> GetOffer(int id)
    {
        return await _context.Offers.Select(o => new OfferDto
        {
            Id = o.OfferId,
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
        }).FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<OfferDto?> GetCurrentOffer()
    {
        var today = DateTime.Today;
        var offer = await _context.Offers.Select(o => new OfferDto
        {
            Id = o.OfferId,
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
        }).FirstOrDefaultAsync(o =>
            o.StartAt <= today && o.EndAt >= today && !o.IsDraft);

        return offer;
    }

    public async Task<DashboardDto?> GetDashboard()
    {
        var offer = await GetCurrentOffer();
        if (offer == null)
        {
            return null;
        }


        var clickCount = await _context.Clicks.Where(o => o.OfferId == offer.Id).CountAsync();
        var activeUserCount =
            await _context.Clicks.Where(o => o.OfferId == offer.Id).GroupBy(c => c.UserId).CountAsync();
        var order = await _context.Orders
            .Where(o => o.OfferId == offer.Id)
            .Include(o => o.User)
            .Select(o => OrderDashboardDto.FromOrder(o))
            .FirstOrDefaultAsync();

        var dashboard = new DashboardDto
        {
            OfferId = offer.Id,
            OfferTitle = offer.Title,
            IsOver = offer.IsOver,
            ClickObjective = offer.ClickObjective,
            ClickCount = clickCount,
            CountOfActiveUser = activeUserCount,
            EndAt = offer.EndAt,
            Order = order
        };

        return dashboard;
    }

    public async Task<ActionResult> CreateOffer(OfferBody offerBody)
    {
        var offer = Offer.FromOfferBody(offerBody);
        var errors = Validate(offer);
        if (errors.Any())
        {
            return ErrorService.BadRequest(string.Join(", ", errors));
        }

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
        if (errors.Any())
        {
            return ErrorService.BadRequest(string.Join(", ", errors));
        }

        await _context.SaveChangesAsync();
        return SuccessService.Ok(StringRes.OfferUpdated);
    }

    private List<string> Validate(Offer offer)
    {
        if (offer.IsDraft)
        {
            return new List<string>();
        }

        var errors = offer.Validate();
        var sameTimeOffer = _context.Offers.FirstOrDefault(o => o.StartAt < offer.StartAt && o.EndAt < offer.StartAt ||
                                                                o.StartAt > offer.EndAt && o.EndAt > offer.EndAt);
        if (sameTimeOffer != null)
        {
            errors.Add($"{StringRes.OfferSameTime}( {sameTimeOffer.Id}: {sameTimeOffer.Title} )");
        }

        return errors;
    }
}