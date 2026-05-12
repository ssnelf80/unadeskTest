using MassTransit;
using Microsoft.EntityFrameworkCore;
using UnadeskTest.Domain.Entities;

namespace UnadeskTest.DAL.PdfContext;

public sealed class PdfDbContext(DbContextOptions<PdfDbContext> options) : DbContext(options)
{
    public DbSet<UploadFileTask> UploadFileTasks { get; set; }
    public DbSet<Domain.Entities.PdfDocument> PdfDocuments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql()
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
      
        builder.AddInboxStateEntity();
        builder.AddOutboxMessageEntity();
        builder.AddOutboxStateEntity();

        builder.Entity<UploadFileTask>().HasKey(k => k.TaskId);
        builder.Entity<Domain.Entities.PdfDocument>().HasKey(k => k.FileId);
    }
}