using Microsoft.EntityFrameworkCore;
using Persistence.Common.Interfaces;

namespace Persistence.Common;

public class TrackingContext : DbContext
{
    public TrackingContext(DbContextOptions<TrackingContext> options) : base(options) { }

    protected TrackingContext(DbContextOptions options) : base(options) {}

    public override int SaveChanges()
    {
        SetProperties();
        return base.SaveChanges();
    }
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        SetProperties();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        SetProperties();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        SetProperties();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void SetProperties()
    {
        foreach (var entity in ChangeTracker.Entries().Where(p => p.State == EntityState.Added))
        {
            if (entity.Entity is IDateCreated created)
            {
                created.DateCreated = DateTime.Now;
            }
        }

        foreach (var entity in ChangeTracker.Entries().Where(p => p.State == EntityState.Modified))
        {
            if (entity.Entity is IDateUpdated updated)
            {
                updated.DateUpdated = DateTime.Now;
            }
        }
    }
}