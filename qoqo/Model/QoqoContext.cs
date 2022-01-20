#nullable disable
// ReSharper disable UnusedAutoPropertyAccessor.Global

using Microsoft.EntityFrameworkCore;

namespace qoqo.Model;

public class QoqoContext : DbContext
{
    public QoqoContext()
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        DbPath = Path.Join(path, "qoqo.db");
    }

    public QoqoContext(DbContextOptions<QoqoContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<Click> Clicks { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Token> Tokens { get; set; }

    private string DbPath { get; } = "";

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (DbPath.Length == 0) return;
        options.UseSqlite($"Data Source={DbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // add user on hasData params
        var adminUser = new User
        {
            UserId = 1,
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
        var defaultOffer = new Offer
        {
            OfferId = 1,
            Title = "TOBLERONE",
            Description = "TOBLERONE BLANC DANS SON EMBALLAGE BLANC ASSOCIE DU CHOCOLAT BLANC SUISSE DE HAUTE QUALITÉ À DU NOUGAT AU MIEL ET AUX AMANDES.",
            StartAt = now.AddDays(-2),
            EndAt = now.AddDays(2),
            BarredPrice = 2.5,
            Price = 0,
            ClickObjective = 100,
            SpecificationText = "Chocolat blanc suisse avec nougat (10%) au miel et aux amandes, Produit en Suisse",
            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/c/cc/Toblerone_3362.jpg",
            IsOver = false,
            IsDraft = false
        };
        modelBuilder.Entity<User>().HasData(adminUser);
        modelBuilder.Entity<Offer>().HasData(defaultOffer);
        base.OnModelCreating(modelBuilder);
    }
}