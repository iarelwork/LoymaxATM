using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Persistence.Common;
using Persistence.Records;

namespace Persistence;

public class AppDbContext : TrackingContext
{
    public DbSet<AccountRecord> Accounts { get; set; }
    public DbSet<TransactionRecord> Transactions { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
