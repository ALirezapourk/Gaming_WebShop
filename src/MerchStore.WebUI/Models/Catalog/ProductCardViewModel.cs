namespace MerchStore.WebUI.Models.Catalog;

public class ProductCardViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TruncatedDescription { get; set; } = string.Empty;
    public string FormattedPrice { get; set; } = string.Empty;
    public decimal PriceAmount { get; set; }
    public string? ImageUrl { get; set; }
    public bool HasImage => !string.IsNullOrEmpty(ImageUrl);
    public bool InStock => StockQuantity > 0;
    public int StockQuantity { get; set; }
}