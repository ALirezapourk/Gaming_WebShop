using MerchStore.Domain.Entities;
using MerchStore.Domain.ValueObjects;

namespace MerchStore.Domain.UnitTests.Entities;

public class ProductTests
{
    // Helper method to create a valid product for testing
    private Product CreateValidProduct()
    {
        return new Product(
            "Test Product",
            "Test Description",
            new Uri("https://example.com/image.jpg"),
            new Money(19.99m, "USD"),
            10);
    }

    [Fact]
    public void Constructor_WithValidParameters_CreatesProduct()
    {
        // Arrange
        string name = "Test Product";
        string description = "Test Description";
        var imageUrl = new Uri("https://example.com/image.jpg");
        var price = new Money(19.99m, "USD");
        int stockQuantity = 10;

        // Act
        var product = new Product(name, description, imageUrl, price, stockQuantity);

        // Assert
        Assert.Equal(name, product.Name);
        Assert.Equal(description, product.Description);
        Assert.Equal(imageUrl, product.ImageUrl);
        Assert.Equal(price, product.Price);
        Assert.Equal(stockQuantity, product.StockQuantity);
        Assert.NotEqual(Guid.Empty, product.Id);
    }

    [Theory]
    [InlineData("", "Test Description", "name")]
    [InlineData(null, "Test Description", "name")]
    [InlineData("Test Product", "", "description")]
    [InlineData("Test Product", null, "description")]
    public void Constructor_WithInvalidNameOrDescription_ThrowsArgumentException(string? name, string? description, string paramName)
    {
        // Arrange
        var imageUrl = new Uri("https://example.com/image.jpg");
        var price = new Money(19.99m, "USD");
        int stockQuantity = 10;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Product(name!, description!, imageUrl, price, stockQuantity));

        Assert.Equal(paramName, exception.ParamName);
    }

    [Theory]
    [InlineData("A very long product name that exceeds the maximum allowed length of 100 characters which is meant to test validation logic", "Test Description", "name")]
    [InlineData("Test Product", "A very long product description that exceeds the maximum allowed length. It goes on and on with unnecessary details and filler content just to make sure we hit the 500 character limit that we've set for our validation logic. It keeps going with more and more text that doesn't really add any value but just takes up space to ensure we exceed the limit. We're adding even more text here to make absolutely certain that this description is too long for our product entity. This should definitely trigger the validation logic that checks for description length and throw an appropriate exception with the correct parameter name to help developers identify and fix the issue quickly.", "description")]
    public void Constructor_WithTooLongNameOrDescription_ThrowsArgumentException(string name, string description, string paramName)
    {
        // Arrange
        var imageUrl = new Uri("https://example.com/image.jpg");
        var price = new Money(19.99m, "USD");
        int stockQuantity = 10;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Product(name, description, imageUrl, price, stockQuantity));

        Assert.Equal(paramName, exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullPrice_ThrowsArgumentNullException()
    {
        // Arrange
        string name = "Test Product";
        string description = "Test Description";
        var imageUrl = new Uri("https://example.com/image.jpg");
        Money price = null!; // Simulating null price
        int stockQuantity = 10;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new Product(name, description, imageUrl, price, stockQuantity));
    }

    [Fact]
    public void Constructor_WithNegativeStockQuantity_ThrowsArgumentException()
    {
        // Arrange
        string name = "Test Product";
        string description = "Test Description";
        var imageUrl = new Uri("https://example.com/image.jpg");
        var price = new Money(19.99m, "USD");
        int stockQuantity = -1;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Product(name, description, imageUrl, price, stockQuantity));

        Assert.Equal("stockQuantity", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithInvalidImageUrl_ThrowsArgumentException()
    {
        // Arrange
        string name = "Test Product";
        string description = "Test Description";
        var imageUrl = new Uri("file:///C:/invalid/path.txt");
        var price = new Money(19.99m, "USD");
        int stockQuantity = 10;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Product(name, description, imageUrl, price, stockQuantity));

        Assert.Contains("URL must use HTTP or HTTPS", exception.Message);
    }

    [Fact]
    public void UpdateDetails_WithValidParameters_UpdatesProduct()
    {
        // Arrange
        var product = CreateValidProduct();
        string newName = "Updated Product";
        string newDescription = "Updated Description";
        var newImageUrl = new Uri("https://example.com/new-image.jpg");

        // Act
        product.UpdateDetails(newName, newDescription, newImageUrl);

        // Assert
        Assert.Equal(newName, product.Name);
        Assert.Equal(newDescription, product.Description);
        Assert.Equal(newImageUrl, product.ImageUrl);
    }

    [Theory]
    [InlineData("", "Updated Description", "name")]
    [InlineData(null, "Updated Description", "name")]
    [InlineData("Updated Product", "", "description")]
    [InlineData("Updated Product", null, "description")]
    public void UpdateDetails_WithInvalidParameters_ThrowsArgumentException(string? newName, string? newDescription, string paramName)
    {
        // Arrange
        var product = CreateValidProduct();
        var newImageUrl = new Uri("https://example.com/new-image.jpg");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            product.UpdateDetails(newName!, newDescription!, newImageUrl));

        Assert.Equal(paramName, exception.ParamName);
    }

    [Fact]
    public void UpdatePrice_WithValidPrice_UpdatesPrice()
    {
        // Arrange
        var product = CreateValidProduct();
        var newPrice = new Money(29.99m, "USD");

        // Act
        product.UpdatePrice(newPrice);

        // Assert
        Assert.Equal(newPrice, product.Price);
    }

    [Fact]
    public void UpdatePrice_WithNullPrice_ThrowsArgumentNullException()
    {
        // Arrange
        var product = CreateValidProduct();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => product.UpdatePrice(null!));
    }

    [Fact]
    public void UpdateStock_WithValidQuantity_UpdatesStockQuantity()
    {
        // Arrange
        var product = CreateValidProduct();
        int newQuantity = 20;

        // Act
        product.UpdateStock(newQuantity);

        // Assert
        Assert.Equal(newQuantity, product.StockQuantity);
    }

    [Fact]
    public void UpdateStock_WithNegativeQuantity_ThrowsArgumentException()
    {
        // Arrange
        var product = CreateValidProduct();
        int newQuantity = -1;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            product.UpdateStock(newQuantity));

        Assert.Equal("quantity", exception.ParamName);
    }

    [Fact]
    public void DecrementStock_WithValidQuantity_DecreasesStock()
    {
        // Arrange
        var product = CreateValidProduct();
        int initialStock = product.StockQuantity;
        int decrementAmount = 3;

        // Act
        bool result = product.DecrementStock(decrementAmount);

        // Assert
        Assert.True(result);
        Assert.Equal(initialStock - decrementAmount, product.StockQuantity);
    }

    [Fact]
    public void DecrementStock_WithDefaultQuantity_DecreasesByOne()
    {
        // Arrange
        var product = CreateValidProduct();
        int initialStock = product.StockQuantity;

        // Act
        bool result = product.DecrementStock();

        // Assert
        Assert.True(result);
        Assert.Equal(initialStock - 1, product.StockQuantity);
    }

    [Fact]
    public void DecrementStock_WithInsufficientStock_ReturnsFalse()
    {
        // Arrange
        var product = CreateValidProduct();
        int initialStock = product.StockQuantity;
        int decrementAmount = initialStock + 1;

        // Act
        bool result = product.DecrementStock(decrementAmount);

        // Assert
        Assert.False(result);
        Assert.Equal(initialStock, product.StockQuantity); // Stock should remain unchanged
    }

    [Fact]
    public void DecrementStock_WithNegativeQuantity_ThrowsArgumentException()
    {
        // Arrange
        var product = CreateValidProduct();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => product.DecrementStock(-1));
    }

    [Fact]
    public void IncrementStock_WithValidQuantity_IncreasesStock()
    {
        // Arrange
        var product = CreateValidProduct();
        int initialStock = product.StockQuantity;
        int incrementAmount = 5;

        // Act
        product.IncrementStock(incrementAmount);

        // Assert
        Assert.Equal(initialStock + incrementAmount, product.StockQuantity);
    }

    [Fact]
    public void IncrementStock_WithZeroQuantity_ThrowsArgumentException()
    {
        // Arrange
        var product = CreateValidProduct();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => product.IncrementStock(0));
    }

    [Fact]
    public void IncrementStock_WithNegativeQuantity_ThrowsArgumentException()
    {
        // Arrange
        var product = CreateValidProduct();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => product.IncrementStock(-1));
    }
}