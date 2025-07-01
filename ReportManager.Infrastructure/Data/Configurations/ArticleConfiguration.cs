using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportManager.Domain.Entities;

namespace ReportManager.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Fluent API configuration for the Article entity.
    /// </summary>
    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.ToTable("Articles");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .ValueGeneratedNever();

            builder.Property(a => a.Title)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(a => a.Content)
                .IsRequired();
            builder.Property(a => a.Summary)
                .IsRequired();
            builder.Property(a => a.PublishDate)
                .IsRequired();
            builder.Property(a => a.Importance)
                .IsRequired();
            builder.Property(a => a.Country)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(a => a.ReporterId)
                .IsRequired();

            builder.HasOne<Reporter>()
                .WithMany()
                .HasForeignKey(a => a.ReporterId)
                .OnDelete(DeleteBehavior.Cascade);

            // Basic indexes for performance
            builder.HasIndex(a => a.PublishDate);
            builder.HasIndex(a => new { a.Country, a.PublishDate });
        }
    }
}