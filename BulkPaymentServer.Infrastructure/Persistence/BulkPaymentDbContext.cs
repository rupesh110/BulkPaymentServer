using Microsoft.EntityFrameworkCore;

using BulkPaymentServer.Domain.Entities;

namespace BulkPaymentServer.Infrastructure.Persistence;

public class BulkPaymentDbContext : DbContext
{
    public BulkPaymentDbContext(DbContextOptions<BulkPaymentDbContext> options) :base(options)
    {

    }

    public DbSet<Upload> Uploads { get; set; } = null;
    public DbSet<Payment> Payments { get; set; } = null;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        //upload config
        modelBuilder.Entity<Upload>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.UserId)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.FileName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(u => u.BlobUrl)
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(u => u.UploadedAt)
                .IsRequired();

            //upload -> creates many payments -> one to many relationship
            entity.HasMany(u => u.Payments)
                .WithOne(p => p.Upload)
                .HasForeignKey(p => p.UploadId);
        });


        //payment config
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.InvoiceNumber)
                .IsRequired();

            entity.Property(p => p.RecipientName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(p => p.RecipientBsb)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(p => p.RecipientAccount)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(p => p.Currency)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(p => p.Amount)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

            entity.Property(p => p.CreatedAt)
                  .IsRequired();

            entity.Property(p => p.Status)
                  .IsRequired()
                  .HasMaxLength(50);
        });
    }
}