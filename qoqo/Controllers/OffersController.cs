using Microsoft.AspNetCore.Mvc;
using qoqo.DataTransferObjects;
using qoqo.Model;
using qoqo.Providers;
using qoqo.Ressources;
using qoqo.Services;

namespace qoqo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OffersController : ControllerBase
{
    private readonly QoqoContext _context;
    private readonly OfferProvider _offerProvider;
    private readonly ITokenService _tokenService;

    public OffersController(QoqoContext qoqoContext, OfferProvider offerProvider, ITokenService tokenService)
    {
        _context = qoqoContext;
        _offerProvider = offerProvider;
        _tokenService = tokenService;
    }

    [HttpGet]
    public async Task<List<OfferIndexDto>> Get()
    {
        return await _offerProvider.GetOffers();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OfferDto?>> Get(string id)
    {
        if (id == "current")
        {
            var currentOffer = await _offerProvider.GetCurrentOffer();
            return currentOffer == null ? ErrorService.BadRequest("No current offer") : currentOffer;
        }

        if (!int.TryParse(id, out var offerId)) return BadRequest();

        var user = _tokenService.GetUser(HttpContext, _context);
        var offer = await _offerProvider.GetOffer(offerId);

        if (offer == null) return ErrorService.BadRequest(StringRes.OfferNotFound);

        if (user is {IsAdmin: false} && (offer.IsDraft || offer.StartAt < DateTime.Now)) return Unauthorized();

        return offer;
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Patch(int id, [FromBody] OfferBody offer)
    {
        return await _offerProvider.UpdateOffer(id, offer);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] OfferBody offer)
    {
        return await _offerProvider.CreateOffer(offer);
    }
}