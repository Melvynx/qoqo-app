using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using qoqo.DataTransferObjects;
using qoqo.Model;
using Xunit;

namespace qoqo_test.integration_test;

public class OrdersControllerTest : IClassFixture<IntegrationFixtures>
{
    private readonly IntegrationFixtures _fixtures;

    public OrdersControllerTest(IntegrationFixtures fixtures)
    {
        _fixtures = fixtures;
    }

    [Fact]
    public async Task GetAllOrder()
    {
        var client = _fixtures.Setup();
        var response = await client.GetAsync("/api/orders");
        response.EnsureSuccessStatusCode();
        var orders = TestHelpers.GetBody<List<OrderDto>>(response);
        await using var context = _fixtures.Context;

        Assert.Equal(context.Orders.Count(), orders?.Count);
    }

    [Fact]
    public async Task GetAllOrderForUserId1()
    {
        var client = _fixtures.Setup();
        _fixtures.Authenticate(client);
        var response = await client.GetAsync("/api/orders/users/1");
        response.EnsureSuccessStatusCode();
        var orders = TestHelpers.GetBody<List<OrderDto>>(response);
        await using var context = _fixtures.Context;

        Assert.Equal(context.Orders.Count(o => o.UserId == 1), orders?.Count);
    }


    [Fact]
    public async Task GetAllOrderForUserUnauthentificated()
    {
        var client = _fixtures.Setup();
        var response = await client.GetAsync("/api/orders/users/1");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetOrderById()
    {
        var client = _fixtures.Setup();
        var response = await client.GetAsync("/api/orders/1");
        response.EnsureSuccessStatusCode();
        var order = TestHelpers.GetBody<OrderDto>(response);
        await using var context = _fixtures.Context;

        Assert.Equal(context.Orders.First(o => o.OrderId == 1).OrderId, order?.OrderId);
    }

    [Fact]
    public async Task PutOrderById()
    {
        var client = _fixtures.Setup();

        var order = new OrderBody
        {
            Status = OrderStatus.CANCELLED
        };
        var response = await client.PutAsJsonAsync("/api/orders/1", order);
        response.EnsureSuccessStatusCode();
    }
}