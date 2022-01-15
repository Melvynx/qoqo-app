using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using qoqo.DataTransferObjects;
using qoqo.Model;
using qoqo.Ressources;
using qoqo.Services;
using Xunit;

namespace qoqo_test.integration_test;

public class OffersControllerTest : IClassFixture<IntegrationFixtures>
{
    private readonly IntegrationFixtures _fixtures;

    public OffersControllerTest(IntegrationFixtures fixtures)
    {
        _fixtures = fixtures;
    }

    [Fact]
    public async Task GetAllOffers()
    {
        var client = _fixtures.Setup();
        var response = await client.GetAsync("/api/offers");
        response.EnsureSuccessStatusCode();
        var offers = TestHelpers.GetBody<List<Offer>>(response);

        await using var context = _fixtures.Context;
        Assert.Equal(context.Offers.Count(), offers?.Count);
    }

    [Fact]
    public async Task GetOfferById()
    {
        var client = _fixtures.Setup();
        var response = await client.GetAsync("/api/offers/1");
        response.EnsureSuccessStatusCode();
        var offer = TestHelpers.GetBody<Offer>(response);

        await using var context = _fixtures.Context;

        Assert.Equal(context.Offers.First().OfferId, offer?.OfferId);
    }

    // getCurrentOffer
    [Fact]
    public async Task GetCurrentOffer()
    {
        var client = _fixtures.Setup();
        var response = await client.GetAsync("/api/offers/current");
        response.EnsureSuccessStatusCode();
        var currentOffer = TestHelpers.GetBody<Offer>(response);

        await using var context = _fixtures.Context;

        var today = DateTime.Today;
        var offer = await context.Offers.FirstOrDefaultAsync(o =>
            o.StartAt <= today && o.EndAt >= today && !o.IsDraft);
        Assert.Equal(offer?.OfferId, currentOffer?.OfferId);
    }

    [Fact]
    public async Task PostOffer()
    {
        var client = _fixtures.Setup();

        await using var context = _fixtures.Context;
        var count = context.Offers.Count();

        var offerBody = new OfferBody
        {
            BarredPrice = 100,
            Price = 0,
            ClickObjective = 40,
            IsDraft = false,
            Title = "Test",
            Description = "Description",
            SpecificationText = "Specification",
            ImageUrl = "https://prout.png",
            StartAt = DateTime.Today,
            EndAt = DateTime.Today.AddDays(1)
        };

        var response = await client.PostAsJsonAsync("/api/offers", offerBody);
        var offer = TestHelpers.GetBody<Offer>(response);

        var afterCount = context.Offers.Count();
        Assert.Equal(count + 1, afterCount);
        Assert.Equal(offer?.Title, offerBody.Title);
        Assert.Equal(offer?.Description, offerBody.Description);
        Assert.Equal(offer?.SpecificationText, offerBody.SpecificationText);
        Assert.Equal(offer?.ImageUrl, offerBody.ImageUrl);
        Assert.Equal(offer?.StartAt, offerBody.StartAt);
        Assert.Equal(offer?.EndAt, offerBody.EndAt);
        Assert.Equal(offer?.Price, offerBody.Price);
        Assert.Equal(offer?.BarredPrice, offerBody.BarredPrice);
        Assert.Equal(offer?.ClickObjective, offerBody.ClickObjective);
        // always true on create
        Assert.True(offer?.IsDraft);
    }

    [Fact]
    public async Task PutOffer()
    {
        var client = _fixtures.Setup();

        await using var context = _fixtures.Context;

        var offerBody = new OfferBody
        {
            BarredPrice = 100,
            Price = 0,
            ClickObjective = 40,
            IsDraft = false,
            Title = "Test",
            Description = "Description",
            SpecificationText = "Specification",
            ImageUrl = "https://prout.png",
            StartAt = DateTime.Today.AddDays(100),
            EndAt = DateTime.Today.AddDays(102)
        };

        var response = await client.PutAsJsonAsync("/api/offers/2", offerBody);

        var msg = TestHelpers.GetBody<RequestMessage>(response);
        Assert.Equal(msg?.Message, StringRes.OfferUpdated);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PutOfferWithDateSameAsOtherOffer()
    {
        var client = _fixtures.Setup();

        await using var context = _fixtures.Context;

        var offerBody = new OfferBody
        {
            BarredPrice = 100,
            Price = 0,
            ClickObjective = 40,
            IsDraft = false,
            Title = "Test",
            Description = "Description",
            SpecificationText = "Specification",
            ImageUrl = "https://prout.png",
            StartAt = DateTime.Today,
            EndAt = DateTime.Today.AddDays(1)
        };

        var response = await client.PutAsJsonAsync("/api/offers/2", offerBody);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PutOfferWithWrongDate()
    {
        var client = _fixtures.Setup();

        await using var context = _fixtures.Context;

        var offerBody = new OfferBody
        {
            BarredPrice = 100,
            Price = 0,
            ClickObjective = 40,
            IsDraft = false,
            Title = "Test",
            Description = "Description",
            SpecificationText = "Specification",
            ImageUrl = "https://prout.png",
            StartAt = DateTime.Today.AddDays(6),
            EndAt = DateTime.Today.AddDays(4)
        };

        var response = await client.PutAsJsonAsync("/api/offers/2", offerBody);
        var msg = TestHelpers.GetBody<RequestMessage>(response);

        Assert.Equal(msg?.Message, StringRes.OfferEndAtBeforeStartAt);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PutOfferWithNoDate()
    {
        var client = _fixtures.Setup();

        await using var context = _fixtures.Context;

        var offerBody = new OfferBody
        {
            BarredPrice = 100,
            Price = 0,
            ClickObjective = 40,
            IsDraft = false,
            Title = "Test",
            Description = "Description",
            SpecificationText = "Specification",
            ImageUrl = "https://prout.png",
            StartAt = null,
            EndAt = DateTime.Today.AddDays(6)
        };

        var response = await client.PutAsJsonAsync("/api/offers/2", offerBody);
        var msg = TestHelpers.GetBody<RequestMessage>(response);

        Assert.Equal(msg?.Message, StringRes.OfferEndAtDateRequired);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}