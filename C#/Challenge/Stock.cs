namespace Challenge;

public class Stock
{
    /// <summary>
    /// Property <c>Name</c> represent the name of the Stock.
    /// </summary>
    /// <remarks>The <c>Name</c> of the stock must be unique</remarks>
    public string Name { get; }
    
    /// <summary>
    /// Instance variable <c>_prices</c> store the prices of the stock for different dates.
    /// </summary>
    /// <remarks>The prices are set by days, so it doesn't consider the variations in shorter
    /// periods of time like hours, minutes, seconds, milliseconds, etc., which could be the case
    /// in a real-world exchange.</remarks>
    private readonly Dictionary<DateOnly, decimal> _prices;

    /// <summary>
    /// Initialize a new Stock with a given name and prices not set yet.
    /// </summary>
    /// <param name="name">Name of the stock.</param>
    /// <remarks>It's assumed that the name of the stock is unique.</remarks>
    public Stock(string name)
    {
        Name = name;
        _prices = new Dictionary<DateOnly, decimal>();
    }

    /// <summary>
    /// Sets the price of the stock in a certain date.
    /// </summary>
    /// <param name="date">Date to set the price.</param>
    /// <param name="price">Price to set.</param>
    /// <returns>Returns <c>true</c> if the price was added successfully (wasn't set previously for the date given);
    /// otherwise, <c>false</c>.</returns>
    public bool TryAddPrice(DateOnly date, decimal price)
    {
        return _prices.TryAdd(date, price);
    }

    /// <summary>
    /// Gets the price of the stock on a certain date.
    /// </summary>
    /// <param name="date">Date to get the price.</param>
    /// <returns>The price of the stock in the given date.</returns>
    /// <exception cref="KeyNotFoundException">If doesn't exist a price set on the given date.</exception>
    public decimal Price(DateOnly date)
    {
        return _prices[date];
    }

    /// <summary>
    /// Returns the hash code for this <c>Stock</c>.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    /// <remarks>The <c>Name</c> of the Stock must be unique.</remarks>
    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}