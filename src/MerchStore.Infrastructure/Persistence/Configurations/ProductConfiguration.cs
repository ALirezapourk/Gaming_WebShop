using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MerchStore.Domain.Entities;

namespace MerchStore.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuration class for the Product entity.
/// This defines how a Product is mapped to the database.
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    /// <summary>
    /// Configures the entity mapping using EF Core's Fluent API.
    /// </summary>
    /// <param name="builder">Entity type builder used to configure the entity</param>
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Define the table name explicitly
        builder.ToTable("Products");

        // Configure the primary key
        builder.HasKey(p => p.Id);

        // Configure Name property
        builder.Property(p => p.Name)
            .IsRequired() // NOT NULL constraint
            .HasMaxLength(100); // VARCHAR(100)

        // Configure Description property
        builder.Property(p => p.Description)
            .IsRequired() // NOT NULL constraint
            .HasMaxLength(500); // VARCHAR(500)

        // Configure StockQuantity property
        builder.Property(p => p.StockQuantity)
            .IsRequired(); // NOT NULL constraint

        // Configure ImageUrl property - it's nullable
        builder.Property(p => p.ImageUrl)
            .IsRequired(false); // NULL allowed

        // Configure the owned entity Money as a complex type
        // This maps the Money value object to columns in the Products table
        builder.OwnsOne(p => p.Price, priceBuilder =>
        {
            // Map Amount property to a column named Price
            priceBuilder.Property(m => m.Amount)
                .HasColumnName("Price")
                .IsRequired();

            // Map Currency property to a column named Currency
            priceBuilder.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        // Add an index on the Name for faster lookups
        builder.HasIndex(p => p.Name);
    }
}