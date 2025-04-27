namespace MerchStore.Domain.ValueObjects;

public record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        // Validate the amount
        if (amount < 0)
            throw new ArgumentException("Money Amount cannot be negative", nameof(amount));

        // Validate the currency
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Money Currency cannot be empty", nameof(currency));

        if (currency.Length != 3)
            throw new ArgumentException("Money Currency code must be 3 characters (ISO 4217 format)", nameof(currency));

        Amount = amount;
        Currency = currency.ToUpper(); // Standardize to uppercase
    }

    // Create SEK currency shorthand using the Static Factory Method design pattern
    public static Money FromSEK(decimal amount) => new Money(amount, "SEK");

    // Add two money values (only if same currency)
    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot add money values with different currencies");

        return new Money(left.Amount + right.Amount, left.Currency);
    }

    // Multiply money by a scalar value (e.g., quantity)
    public static Money operator *(Money money, int multiplier)
    {
        return new Money(money.Amount * multiplier, money.Currency);
    }

    // Multiply money by a decimal value (for percentages, etc.)
    public static Money operator *(Money money, decimal multiplier)
    {
        if (multiplier < 0)
            throw new ArgumentException("Cannot multiply money by a negative value", nameof(multiplier));

        return new Money(money.Amount * multiplier, money.Currency);
    }

    // Support for commutative property (int * Money) (var total = 3 * price; // Same as price * 3, resulting in 150 SEK)
    public static Money operator *(int multiplier, Money money)
    {
        return money * multiplier;
    }

    // Support for commutative property (decimal * Money)
    public static Money operator *(decimal multiplier, Money money)
    {
        return money * multiplier;
    }

    // Format as string with currency
    // public override string ToString() => $"{Amount:F2} {Currency}";
    // Format as string with invariant culture to avoid localization issues (e.g., decimal separator)
    public override string ToString() => $"{Amount.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)} {Currency}";
}