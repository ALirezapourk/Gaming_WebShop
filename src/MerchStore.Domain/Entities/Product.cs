using MerchStore.Domain.Common;
using MerchStore.Domain.ValueObjects;

namespace MerchStore.Domain.Entities;

public class Product : Entity<Guid>
{
    // Properties with private setters for encapsulation
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Money Price { get; private set; } = Money.FromSEK(0);
    public int StockQuantity { get; private set; } = 0;
    public Uri? ImageUrl { get; private set; } = null;

    // Private parameterless constructor for EF Core
    private Product()
    {
        // Required for EF Core, but we don't want it to be used directly
    }

    // Public constructor with required parameters
    public Product(string name, string description, Uri? imageUrl, Money price, int stockQuantity) : base(Guid.NewGuid())
    {
        // Validate parameters
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("Product name cannot exceed 100 characters", nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Product description cannot be empty", nameof(description));

        if (description.Length > 500)
            throw new ArgumentException("Product description cannot exceed 500 characters", nameof(description));

        // Image URI validation
        if (imageUrl != null)
        {
            // Validate URI scheme (only allow http and https)
            if (imageUrl.Scheme != "http" && imageUrl.Scheme != "https")
                throw new ArgumentException("Image URL must use HTTP or HTTPS protocol", nameof(imageUrl));

            // Validate URI length - using AbsoluteUri to get the full string representation
            if (imageUrl.AbsoluteUri.Length > 2000)
                throw new ArgumentException("Image URL exceeds maximum length of 2000 characters", nameof(imageUrl));

            // Optional: Validate file extension for images
            string extension = Path.GetExtension(imageUrl.AbsoluteUri).ToLowerInvariant();
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

            if (!validExtensions.Contains(extension))
                throw new ArgumentException("Image URL must point to a valid image file (jpg, jpeg, png, gif, webp)", nameof(imageUrl));
        }

        if (price is null)
            throw new ArgumentNullException(nameof(price));

        if (stockQuantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative", nameof(stockQuantity));

        // Set properties
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        Price = price;
        StockQuantity = stockQuantity;
    }

    // Domain methods that encapsulate business logic
    public void UpdateDetails(string name, string description, Uri? imageUrl)
    {
        // Validate name with clear domain rules
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("Name cannot exceed 100 characters", nameof(name));

        // Validate description with clear domain rules
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));

        if (description.Length > 500)
            throw new ArgumentException("Description cannot exceed 500 characters", nameof(description));

        // Image URI validation
        if (imageUrl != null)
        {
            // Validate URI scheme (only allow http and https)
            if (imageUrl.Scheme != "http" && imageUrl.Scheme != "https")
                throw new ArgumentException("Image URL must use HTTP or HTTPS protocol", nameof(imageUrl));

            // Validate URI length - using AbsoluteUri to get the full string representation
            if (imageUrl.AbsoluteUri.Length > 2000)
                throw new ArgumentException("Image URL exceeds maximum length of 2000 characters", nameof(imageUrl));

            // Optional: Validate file extension for images
            string extension = Path.GetExtension(imageUrl.AbsoluteUri).ToLowerInvariant();
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

            if (!validExtensions.Contains(extension))
                throw new ArgumentException("Image URL must point to a valid image file (jpg, jpeg, png, gif, webp)", nameof(imageUrl));
        }

        // Update properties after all validation passes
        Name = name;
        Description = description;
        ImageUrl = imageUrl;  // Assuming the property name has been updated to imageUrl
    }

    public void UpdatePrice(Money newPrice)
    {
        ArgumentNullException.ThrowIfNull(newPrice);

        Price = newPrice;
    }

    public void UpdateStock(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative", nameof(quantity));

        StockQuantity = quantity;
    }


    public bool DecrementStock(int quantity = 1)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        if (StockQuantity < quantity)
            return false; // Not enough stock

        StockQuantity -= quantity;
        return true;
    }

    public void IncrementStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        StockQuantity += quantity;
    }
}