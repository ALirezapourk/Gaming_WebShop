namespace MerchStore.WebUI.Models.Api.Basic;

/// <summary>
/// Simple Data Transfer Object for product information in the Basic API.
/// </summary>
/// <remarks>
/// This DTO contains only the essential product information needed for the Basic API response.
/// It's intentionally kept separate from DTOs used in other API implementations to allow
/// independent evolution.
/// </remarks>
public class BasicProductDto
{
    /// <summary>
    /// The unique identifier of the product
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the product
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// A description of the product
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The price amount of the product
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// The currency code for the price (e.g., "SEK", "USD")
    /// </summary>
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// The URL to the product image, if available
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// The current stock quantity of the product
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Indicates whether the product is currently in stock
    /// </summary>
    public bool InStock => StockQuantity > 0;
}