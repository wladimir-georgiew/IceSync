using IceSync.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IceSync.Domain;

public class IceSyncDbContext(DbContextOptions<IceSyncDbContext> options) : DbContext(options)
{
    public DbSet<WorkflowEntity> Workflows { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkflowEntity>()
            .HasIndex(w => w.WorkflowId)
            .IsUnique();
    }
}