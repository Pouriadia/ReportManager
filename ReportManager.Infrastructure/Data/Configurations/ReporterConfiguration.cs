using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportManager.Domain.Entities;

namespace ReportManager.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Fluent API configuration for the Reporter entity.
    /// </summary>
    public class ReporterConfiguration : IEntityTypeConfiguration<Reporter>
    {
        public void Configure(EntityTypeBuilder<Reporter> builder)
        {
            builder.ToTable("Reporters");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id)
                .ValueGeneratedNever();

            builder.Property(r => r.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(r => r.LastName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(r => r.Email)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(r => r.Phone)
                .HasMaxLength(50);
            builder.Property(r => r.HireDate)
                .IsRequired();
            builder.Property(r => r.Bio);
        }
    }
}