using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using qoqo.DataTransferObjects;
using qoqo.Ressources;
using qoqo.Services;
using Xunit;

namespace qoqo_test.integration_test;

public class ClicksControllerTest : IClassFixture<IntegrationFixtures>
{
    private readonly IntegrationFixtures _fixtures;

    public ClicksControllerTest(IntegrationFixtures fixtures)
    {
        _fixtures = fixtures;
    }
    
    [Fact]
    public async Task GetClick()
    {
        var client = _fixtures.Setup();
        _fixtures.Authenticate(client, 1);

        await using var context = _fixtures.Context;
        
        var response = await client.GetAsync("/api/clicks");
        response.EnsureSuccessStatusCode();
        
        var clicks = TestHelpers.GetBody<List<UserClick>>(response);
        Assert.Equal(context.Clicks.Count(), clicks?.Count);
        Assert.Equal(context.Clicks.Where(u => u.UserId == 1).GroupBy(c => c.OfferId).Count(), clicks?.Count);
    }

    [Fact]
    public async Task GetOfferClicks()
    {
        var client = _fixtures.Setup();
        _fixtures.Authenticate(client, 1);

        await using var context = _fixtures.Context;
        
        var response = await client.GetAsync("/api/clicks/offers/1");
        response.EnsureSuccessStatusCode();

        var click = TestHelpers.GetBody<OfferClickDto>(response);
        
        Assert.NotEqual(0, click?.RemainingTime);
        Assert.Equal(context.Clicks.Count(c => c.OfferId == 1), click?.Click);
        
        // go 10 minutes later
        var lastClick = context.Clicks.OrderBy(c => c.ClickId).LastOrDefault(c => c.UserId == 1 && c.OfferId == 1);
        Assert.NotNull(lastClick);
        lastClick.CreatedAt = lastClick.CreatedAt.AddSeconds(-10);
        await context.SaveChangesAsync();
        
        var response2 = await client.GetAsync("/api/clicks/offers/1");
        response2.EnsureSuccessStatusCode();
        var click2 = TestHelpers.GetBody<OfferClickDto>(response2);
        Assert.Equal(0, click2?.RemainingTime);
    }
    
    [Fact]
    public async Task GetOfferClicksUnAuthenticate()
    {
        var client = _fixtures.Setup();

        await using var context = _fixtures.Context;
        
        var response = await client.GetAsync("/api/clicks/offers/1");
        response.EnsureSuccessStatusCode();
        var click = TestHelpers.GetBody<OfferClickDto>(response);

        Assert.Equal(0, click?.RemainingTime);
    }

    [Fact]
    public async Task GetOfferClicksWithNoCurrentOffer()
    {
        var client = _fixtures.Setup();

        var response = await client.GetAsync("/api/clicks/offers/99999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AddOfferClickUnAuthenticate()
    {
        var client = _fixtures.Setup();
        var response = await client.PostAsync("api/clicks/offers/1", null);
        var msg = TestHelpers.GetBody<RequestMessage>(response);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal(StringRes.NeedToBeLoggedToClick, msg?.Message);
    }
    
    
    [Fact]
    public async Task AddOfferClickUnCurrentOffer()
    {
        var client = _fixtures.Setup();
        _fixtures.Authenticate(client, 1);

        var response = await client.PostAsync("api/clicks/offers/2", null);
        var msg = TestHelpers.GetBody<RequestMessage>(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(StringRes.OfferNotFound, msg?.Message);
    }
    
    // by default the user just click in the fixture
    [Fact]
    public async Task AddOfferClickToFast()
    {
        var client = _fixtures.Setup();
        _fixtures.Authenticate(client, 1);

        var response = await client.PostAsync("api/clicks/offers/1", null);
        var msg = TestHelpers.GetBody<RequestMessage>(response);
        Assert.Equal(StringRes.ClickMinimum10Seconds, msg?.Message);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    // by default the user just click in the fixture
    [Fact]
    public async Task AddOfferClickEnoughTime()
    {
        var client = _fixtures.Setup();
        _fixtures.Authenticate(client, 2);
        
        await using var context = _fixtures.Context;
        context.Offers.Find(1)!.ClickObjective = context.Clicks.Count(c => c.OfferId == 1);
        await context.SaveChangesAsync();

        var response = await client.PostAsync("api/clicks/offers/1", null);
        var msg = TestHelpers.GetBody<RequestMessage>(response);
        Assert.Equal(StringRes.OfferClickEnoughTime, msg?.Message);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddFinishClick()
    {
        var client = _fixtures.Setup();
        _fixtures.Authenticate(client, 2);
        
        await using var context = _fixtures.Context;
        context.Offers.Find(1)!.ClickObjective = context.Clicks.Count(c => c.OfferId == 1) + 1;
        await context.SaveChangesAsync();
        
        var orderCount = context.Orders.Count(o => o.OfferId == 1);
        
        var response = await client.PostAsync("api/clicks/offers/1", null);

        var clickEvent = TestHelpers.GetBody<ClickEventResult>(response);
        Assert.True(clickEvent?.Confetti);
        
        var orderCountAfter = context.Orders.Count(o => o.OfferId == 1);
        Assert.Equal(orderCount + 1, orderCountAfter);
    }
    
    [Fact]
    public async Task AddClick()
    {
        var client = _fixtures.Setup();
        _fixtures.Authenticate(client, 2);
        
        var response = await client.PostAsync("api/clicks/offers/1", null);

        var clickEvent = TestHelpers.GetBody<ClickEventResult>(response);
        Assert.False(clickEvent?.Confetti);
    }
}