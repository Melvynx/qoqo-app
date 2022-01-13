using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using qoqo.Hubs;
using qoqo.Model;
using qoqo.Providers;
using qoqo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.ContractResolver = new DefaultContractResolver
            {NamingStrategy = new CamelCaseNamingStrategy()};
        options.SerializerSettings.Formatting = Formatting.Indented;
        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
    }
);

if (builder.Environment.EnvironmentName == "Test")
{
    builder.Services.AddDbContext<QoqoContext>(options => options.UseInMemoryDatabase("qoqo"));    
}
else if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<QoqoContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("App")));
}
else
{
    builder.Services.AddDbContext<QoqoContext>();
}


builder.Services.AddTransient<UserProvider>();
builder.Services.AddTransient<ClickProvider>();
builder.Services.AddTransient<OfferProvider>();
builder.Services.AddTransient<OrderProvider>();

builder.Services.AddTransient<HubService>();

builder.Services.AddTransient<ITokenService, TokenService>();

// libraries
builder.Services.AddSignalR();
builder.Services.AddResponseCaching();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseHsts();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    if (builder.Environment.EnvironmentName != "Test")
    {
        var context = services.GetRequiredService<QoqoContext>();
        context.Database.EnsureCreated();
    }
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

app.MapSwagger();
app.UseSwaggerUI(options =>
{
    options.InjectJavascript("/swagger-ui/swagger.js");
});

app.Run();

// for tests purposes
public partial class Program { }