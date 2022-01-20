using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using qoqo_test.mock;
using qoqo.Model;
using qoqo.Services;

namespace qoqo_test.integration_test;

public class IntegrationFixtures : WebApplicationFactory<Program>
{
    public QoqoContext Context => Services.CreateScope().ServiceProvider.GetRequiredService<QoqoContext>();

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
        using (var context = Context)
        {
            context.Database.EnsureDeleted();
        }

        var client = CreateClient();
        SetupFixture();
        return client;
    }

    private void SetupFixture()
    {
        using var context = Context;
        context.Database.EnsureCreated();
        var testFixtures = new TestFixtures(context);
        testFixtures.Setup();
        context.SaveChanges();
    }

    public QoqoContext GetContext()
    {
        return Services.CreateScope().ServiceProvider.GetRequiredService<QoqoContext>();
    }

    public void Authenticate(HttpClient httpClient, int userId = 1)
    {
        using var context = Context;
        var token = context.Tokens.Add(Token.GenerateToken(userId));
        context.SaveChanges();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Entity.Value);
    }
}