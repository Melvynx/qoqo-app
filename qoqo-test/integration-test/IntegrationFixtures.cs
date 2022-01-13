using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using qoqo_test.mock;
using qoqo.Model;
using qoqo.Services;
using Xunit;

namespace qoqo_test.integration_test;

public class IntegrationFixtures : WebApplicationFactory<Program>
{
    private readonly WebApplicationFactory<Program> _factory;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("Environment", "Test");
        builder.ConfigureServices(services =>
        {
            services.Replace(ServiceDescriptor.Transient<ITokenService, TokenServiceMock>());
        });
    }

    public HttpClient Setup()
    {
        var client = CreateClient();
        SetupFixture();
        return client;
    }
    
    // use WithWebHostBuilder from https://github.com/dotnet/AspNetCore.Docs/issues/12303
    // public IntegrationFixtures(WebApplicationFactory<Program> factory)
    // {
    //     _factory = factory.WithWebHostBuilder(builder =>
    //     {
    //         builder.UseSetting("Environment", "Test");
    //         builder.ConfigureServices(services =>
    //         {
    //             services.Replace(ServiceDescriptor.Transient<ITokenService, TokenServiceMock>());
    //         });
    //     });
    //     httpClient = _factory.CreateClient();
    //     SetupFixture();
    // }

    private void SetupFixture()
    {
        var user = new User
        {
            UserName = "Jean",
            PasswordHash = "$2a$11$mttOYFwAIsxonQM.DWIMA.swdPep2o/ABHhUcycgbO68FOY9gpLZO", // 123456
            FirstName = "Jean",
            LastName = "Didier",
            Email = "jean@gmail.com",
            Street = "Small cat Street",
            City = "Gland",
            Npa = 1900
        };
        var adminUser = new User
        {
            UserName = "Admin",
            PasswordHash = "$2a$11$ss.H3F2abvWfm6TnZAgKT.Lu6ihbxJbL/Khqlyzi2V/bFtbztzItC", // abcdef
            FirstName = "Admin",
            LastName = "Admin",
            Email = "admin@gmail.com",
            Street = "Admin street",
            City = "Admin city",
            Npa = 1700
        };
        var now = DateTime.Now;
        var liveOffer = new Offer
        {
            Title = "Offer 1",
            Description = "Offer 1 description",
            StartAt = now,
            EndAt = now.AddDays(2),
            BarredPrice = 100,
            Price = 0,
            ClickObjective = 100,
            SpecificationText = "Specification text",
            ImageUrl = "https://img.com",
            IsOver = false,
            IsDraft = false
        };
        using var context = Context;
       
        context.Offers.Add(liveOffer);
        context.Users.Add(user);
        context.Users.Add(adminUser);
        context.SaveChanges();
    }

    public QoqoContext Context => Services.CreateScope().ServiceProvider.GetRequiredService<QoqoContext>();


    public void Authentificate(HttpClient httpClient, int userId = 1)
    {
        using var context = Context;
        var token = context.Tokens.Add(Token.GenerateToken(userId));
        context.SaveChanges();
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Entity.Value);
    }
}
