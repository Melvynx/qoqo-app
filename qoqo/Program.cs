using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using qoqo.Authorization;
using qoqo.Hubs;
using qoqo.Model;
using qoqo.Providers;
using qoqo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.ContractResolver = new DefaultContractResolver
            {NamingStrategy = new CamelCaseNamingStrategy()};
        options.SerializerSettings.Formatting = Formatting.Indented;
        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
    }
);

builder.Services.AddDbContext<QoqoContext>();
builder.Services.AddTransient<UserProvider>();
builder.Services.AddTransient<ClickProvider>();
builder.Services.AddTransient<OfferProvider>();
builder.Services.AddTransient<OrderProvider>();
builder.Services.AddTransient<HubService>();
builder.Services.AddTransient<IAuthenticationService, AuthorizationService>();

builder.Services.AddSignalR();
builder.Services.AddResponseCaching();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseHsts();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<QoqoContext>();
    context.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseResponseCaching();

app.MapControllerRoute(
    "default",
    "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.MapHub<OfferHub>("/offerHub");

app.Run();