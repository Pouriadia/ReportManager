using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReportManager.Infrastructure.Data;
using ReportManager.Domain.Entities;

namespace ReportManager.Infrastructure.Migrations
{
    [DbContext(typeof(NewsDbContext))]
    partial class NewsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("ReportManager.Domain.Entities.Reporter", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedNever();
                b.Property<string>("Bio").HasColumnType("nvarchar(max)");
                b.Property<string>("Email").IsRequired().HasMaxLength(200);
                b.Property<DateTime>("HireDate");
                b.Property<string>("Phone").HasMaxLength(50);
                b.Property<string>("FirstName").IsRequired().HasMaxLength(100);
                b.Property<string>("LastName").IsRequired().HasMaxLength(100);
                b.HasKey("Id");
                b.ToTable("Reporters");
            });

            modelBuilder.Entity("ReportManager.Domain.Entities.Article", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedNever();
                b.Property<string>("Content").IsRequired();
                b.Property<string>("Country").IsRequired().HasMaxLength(100);
                b.Property<string>("Summary").IsRequired();
                b.Property<DateTime>("PublishDate");
                b.Property<int>("Importance");
                b.Property<string>("Title").IsRequired().HasMaxLength(500);
                b.Property<Guid>("ReporterId");
                b.HasKey("Id");
                b.HasIndex("PublishDate");
                b.HasIndex("Country", "PublishDate");
                b.ToTable("Articles");
            });

            modelBuilder.Entity("ReportManager.Domain.Entities.Article", b =>
            {
                b.HasOne("ReportManager.Domain.Entities.Reporter")
                    .WithMany()
                    .HasForeignKey("ReporterId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });
        }
    }
}