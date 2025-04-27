using MerchStore.Domain.ValueObjects;

namespace MerchStore.Domain.UnitTests.ValueObjects;

public class MoneyTests
{
    [Theory]
    [InlineData(10.99, "USD")]
    [InlineData(0, "EUR")]
    [InlineData(999999.99, "SEK")]
    public void Constructor_WithValidParameters_CreatesMoneyObject(decimal amount, string currency)
    {
        // Act
        var money = new Money(amount, currency);

        // Assert
        Assert.Equal(amount, money.Amount);
        Assert.Equal(currency.ToUpper(), money.Currency);
    }

    [Theory]
    [InlineData(-1, "USD", "amount")]
    [InlineData(-0.01, "EUR", "amount")]
    public void Constructor_WithNegativeAmount_ThrowsArgumentException(decimal amount, string currency, string paramName)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Money(amount, currency));
        Assert.Equal(paramName, exception.ParamName);
    }

    [Theory]
    [InlineData(10.0, "", "currency")]
    [InlineData(10.0, null, "currency")]
    [InlineData(10.0, "US", "currency")]
    [InlineData(10.0, "USDD", "currency")]
    public void Constructor_WithInvalidCurrency_ThrowsArgumentException(decimal amount, string? currency, string paramName)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Money(amount, currency!));
        Assert.Equal(paramName, exception.ParamName);
    }

    [Fact]
    public void FromSEK_WithValidAmount_CreatesSEKMoney()
    {
        // Arrange
        decimal amount = 15.75m;

        // Act
        var money = Money.FromSEK(amount);

        // Assert
        Assert.Equal(amount, money.Amount);
        Assert.Equal("SEK", money.Currency);
    }

    [Fact]
    public void AddOperator_WithSameCurrency_AddsMoney()
    {
        // Arrange
        var money1 = new Money(10.5m, "USD");
        var money2 = new Money(5.25m, "USD");
        var expectedSum = 15.75m;

        // Act
        var result = money1 + money2;

        // Assert
        Assert.Equal(expectedSum, result.Amount);
        Assert.Equal("USD", result.Currency);
    }

    [Fact]
    public void AddOperator_WithDifferentCurrencies_ThrowsInvalidOperationException()
    {
        // Arrange
        var money1 = new Money(10.5m, "USD");
        var money2 = new Money(5.25m, "EUR");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => money1 + money2);
    }

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        // Arrange
        var money = new Money(10.5m, "USD");
        var expected = "10.50 USD";

        // Act
        var result = money.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void RecordEquality_WithEqualValues_ReturnsTrue()
    {
        // Arrange
        var money1 = new Money(10.5m, "USD");
        var money2 = new Money(10.5m, "USD");

        // Act & Assert
        Assert.Equal(money1, money2);
    }

    [Fact]
    public void RecordEquality_WithDifferentValues_ReturnsFalse()
    {
        // Arrange
        var money1 = new Money(10.5m, "USD");
        var money2 = new Money(10.5m, "EUR");

        // Act & Assert
        Assert.NotEqual(money1, money2);
    }
}