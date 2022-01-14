namespace Challenge;

public class Portfolio
{
    /// <summary>
    /// Instance variable <c>_stocks</c> represents the stocks of the portfolio associated with the
    /// amount acquired.
    /// </summary>
    private readonly Dictionary<Stock, decimal> _stocks;

    /// <summary>
    /// Initialize a new Portfolio without stocks.
    /// </summary>
    public Portfolio()
    {
        _stocks = new Dictionary<Stock, decimal>();
    }

    /// <summary>
    /// Adds a certain <paramref name="amount"/> of the given <paramref name="stock"/> to the portfolio.
    /// </summary>
    /// <param name="stock">Stock to add.</param>
    /// <param name="amount">Amount to add of the given stock.</param>
    /// <returns>The current total amount of the stock in the portfolio.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If the <paramref name="amount"/> is 0 or negative.</exception>
    public decimal AddStock(Stock stock, decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "The given amount must be above 0");
        }
        if (_stocks.ContainsKey(stock))
        {
            _stocks[stock] += amount;
        }
        else
        {
            _stocks.Add(stock, amount);
        }

        return _stocks[stock];
    }

    /// <summary>
    /// By default calculate the annualized return of the portfolio, if <paramref name="isAnnualized"/> is set to
    /// <c>false</c>, calculate the cumulative return instead.
    /// </summary>
    /// <param name="start">Initial date of the period.</param>
    /// <param name="end">Final date of the period.</param>
    /// <param name="isAnnualized">If <c>true</c> calculate the annualized return; otherwise, calculate the cumulative return.</param>
    /// <returns>The profit of the portfolio (annualized or cumulative).</returns>
    /// <exception cref="ArgumentException">If <paramref name="start"/> is a date after <paramref name="end"/>.</exception>
    public decimal Profit(DateOnly start, DateOnly end, bool isAnnualized = true)
    {
        if (end < start)
        {
            throw new ArgumentException($"The parameter {nameof(start)} is a date after {nameof(end)}");
        }
        
        return isAnnualized ?
            AnnualizedReturn(start, end) :
            CumulativeReturn(start, end);
    }

    /// <summary>
    /// Calculate the cumulative return between the dates given.<br/>
    /// See: <see href="https://www.investopedia.com/ask/answer/07/portfolio_calculations.asp#toc-calculating-the-percentage-return-of-your-portfolio">
    /// 'Calculating the Percentage Return of Your Portfolio' on www.investopedia.com</see><br/>
    /// See also: <seealso href="https://www.investopedia.com/terms/c/cumulativereturn.asp">
    /// 'Cumulative Return' on www.investopedia.com</seealso>
    /// </summary>
    /// <param name="start">Initial date of the period.</param>
    /// <param name="end">Final date of the period.</param>
    /// <returns>The cumulative return of the portfolio.</returns>
    private decimal CumulativeReturn(DateOnly start, DateOnly end)
    {
        if (start >= end || _stocks.Count <= 0)
        {
            return 0;
        }
        
        decimal startTotal = 0;
        decimal endTotal = 0;
        foreach (var stock in _stocks.Keys)
        {
            decimal stockAmount = _stocks[stock];
            startTotal += stockAmount * stock.Price(start);
            endTotal += stockAmount * stock.Price(end);
        }

        return (endTotal - startTotal) / startTotal;

    }

    /// <summary>
    /// Calculate the annualized return between the dates given.<br/>
    /// See: <see href="https://www.investopedia.com/terms/a/annualized-total-return.asp">'Annualized Total Return' on www.investopedia.com</see>
    /// </summary>
    /// <param name="start">Initial date of the period.</param>
    /// <param name="end">Final date of the period.</param>
    /// <returns>The annualized return of the portfolio.</returns>
    /// <exception cref="OverflowException">If the result of the calculation is over <c>decimal.MaxValue</c></exception>
    private decimal AnnualizedReturn(DateOnly start, DateOnly end)
    {
        decimal cumulativeReturn = CumulativeReturn(start, end);

        switch (cumulativeReturn)
        {
            // The difference of start and end dates is in years
            // Use the formula for yearly periods
            case > 0 when start.Month == end.AddDays(1).Month && start.Day == end.AddDays(1).Day:
            {
                decimal yearsHeld = end.AddDays(1).Year - start.Year;
                
                double basePow = (double) (1m + cumulativeReturn);
                double exponentPow = (double) (1m / yearsHeld);
                return (decimal)Math.Pow(basePow, exponentPow) - 1m;
            }
            // The difference of start and end dates isn't exact years
            // Use the formula with the difference of days instead
            case > 0:
            {
                DateTime dtStart = start.ToDateTime(TimeOnly.MinValue);
                DateTime dtEnd = end.ToDateTime(TimeOnly.MinValue);
                double daysHeld = (dtEnd - dtStart).Days;
                
                double basePow = (double) (1m + cumulativeReturn);
                double exponentPow = 365 / daysHeld;
                return (decimal)Math.Pow(basePow, exponentPow) - 1m;
            }
            default:
                return 0;
        }
    }

}