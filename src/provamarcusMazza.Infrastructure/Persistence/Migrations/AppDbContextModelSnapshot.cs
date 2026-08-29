using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using provamarcusMazza.Infrastructure.Persistence;

#nullable disable

namespace provamarcusMazza.Infrastructure.Persistence.Migrations;

[DbContext(typeof(AppDbContext))]
partial class AppDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "10.0.11");

        modelBuilder.Entity("provamarcusMazza.Domain.Entities.Customer", b =>
        {
            b.Property<Guid>("Id").HasColumnType("TEXT");
            b.Property<DateTime>("CreatedAt").HasColumnType("TEXT");
            b.Property<string>("Email").IsRequired().HasMaxLength(200).HasColumnType("TEXT");
            b.Property<string>("Name").IsRequired().HasMaxLength(150).HasColumnType("TEXT");
            b.HasKey("Id");
            b.HasIndex("Email").IsUnique();
            b.ToTable("Customers");
        });

        modelBuilder.Entity("provamarcusMazza.Domain.Entities.Order", b =>
        {
            b.Property<Guid>("Id").HasColumnType("TEXT");
            b.Property<DateTime>("CreatedAt").HasColumnType("TEXT");
            b.Property<Guid>("CustomerId").HasColumnType("TEXT");
            b.Property<int>("Status").HasColumnType("INTEGER");
            b.HasKey("Id");
            b.ToTable("Orders");
        });

        modelBuilder.Entity("provamarcusMazza.Domain.Entities.OrderItem", b =>
        {
            b.Property<Guid>("Id").HasColumnType("TEXT");
            b.Property<Guid>("OrderId").HasColumnType("TEXT");
            b.Property<string>("ProductName").IsRequired().HasMaxLength(200).HasColumnType("TEXT");
            b.Property<int>("Quantity").HasColumnType("INTEGER");
            b.Property<decimal>("UnitPrice").HasPrecision(18, 2).HasColumnType("TEXT");
            b.HasKey("Id");
            b.HasIndex("OrderId");
            b.ToTable("OrderItems");
        });

        modelBuilder.Entity("provamarcusMazza.Domain.Entities.User", b =>
        {
            b.Property<Guid>("Id").HasColumnType("TEXT");
            b.Property<DateTime>("CreatedAt").HasColumnType("TEXT");
            b.Property<string>("Email").IsRequired().HasMaxLength(200).HasColumnType("TEXT");
            b.Property<bool>("IsActive").HasColumnType("INTEGER");
            b.Property<string>("PasswordHash").IsRequired().HasMaxLength(500).HasColumnType("TEXT");
            b.HasKey("Id");
            b.HasIndex("Email").IsUnique();
            b.ToTable("Users");
        });

        modelBuilder.Entity("provamarcusMazza.Domain.Entities.OrderItem", b =>
        {
            b.HasOne("provamarcusMazza.Domain.Entities.Order", null)
                .WithMany("Items")
                .HasForeignKey("OrderId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity("provamarcusMazza.Domain.Entities.Order", b =>
        {
            b.Navigation("Items");
        });
    }
}
