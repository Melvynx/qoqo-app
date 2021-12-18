using Microsoft.EntityFrameworkCore;

namespace qoqo.Model;

public class QoqoContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<Click> Clicks { get; set; }
    public DbSet<Order> Orders { get; set; }

    private string DbPath { get; }

    public QoqoContext()
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        DbPath = Path.Join(path, "qoqo.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}