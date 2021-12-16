using Microsoft.EntityFrameworkCore;

namespace qoqo.Model;

public class QoqoContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    public string DbPath { get; }

    public QoqoContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "qoqo.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}