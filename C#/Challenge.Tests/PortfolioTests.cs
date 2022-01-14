using System;
using Xunit;
using Challenge;

namespace ChallengeTests;

public class PortfolioTests
{
    [Fact]
    public void AddStock_ShouldThrowArgumentOutOfRangeException_IfAddsANegativeAmount()
    {
        // Arrange
        decimal amount = -1;
        Portfolio portfolio = new Portfolio();
        Stock stock = new Stock("Fintual");

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => portfolio.AddStock(stock, amount));
    }
    
    [Fact]
    public void AddStock_ShouldThrowArgumentOutOfRangeException_IfAddsZeroAmount()
    {
        // Arrange
        decimal amount = 0;
        Portfolio portfolio = new Portfolio();
        Stock stock = new Stock("Fintual");

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => portfolio.AddStock(stock, amount));
    }
    
    [Fact]
    public void AddStock_ShouldAddAPositiveAmountOfAStockCorrectly()
    {
        // Arrange
        decimal amount = 1;
        Portfolio portfolio = new Portfolio();
        Stock stock = new Stock("Fintual");

        // Act
        decimal actual = portfolio.AddStock(stock, amount);
        
        // Assert
        Assert.Equal(amount, actual);
    }
    
    [Fact]
    public void AddStock_ShouldAddAPositiveAmountOfAStockMultipleTimesCorrectly()
    {
        // Arrange
        decimal amount1 = 1;
        decimal amount2 = 9;
        decimal expected = amount1 + amount2;
        
        Portfolio portfolio = new Portfolio();
        Stock stock = new Stock("Fintual");

        // Act
        portfolio.AddStock(stock, amount1);
        decimal actual = portfolio.AddStock(stock, amount2);
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void Profit_CumulativeReturn_ShouldReturnZero_IfDontHaveStocks()
    {
        ShouldReturnZero_IfDontHaveStocks(false);
    }
    
    [Fact]
    public void Profit_AnnualizedReturn_ShouldReturnZero_IfDontHaveStocks()
    {
        ShouldReturnZero_IfDontHaveStocks(true);
    }
    
    private void ShouldReturnZero_IfDontHaveStocks(bool isAnnualized)
    {
        // Arrange
        const decimal expected = 0;
        Portfolio portfolio = new Portfolio();

        DateOnly start = DateOnly.FromDateTime(DateTime.Now);
        DateOnly end = start.AddYears(1);

        // Act
        decimal actual = portfolio.Profit(start, end, isAnnualized);

        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void Profit_CumulativeReturn_ShouldReturnZero_IfStartAndEndAreEquals()
    {
        ShouldReturnZero_IfStartAndEndAreEquals(false);
    }
    
    [Fact]
    public void Profit_AnnualizedReturn_ShouldReturnZero_IfStartAndEndAreEquals()
    {
        ShouldReturnZero_IfStartAndEndAreEquals(true);
    }
    
    private void ShouldReturnZero_IfStartAndEndAreEquals(bool isAnnualized)
    {
        // Arrange
        const decimal expected = 0;
        Portfolio portfolio = new Portfolio();

        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        
        Stock stock = new Stock("Fintual");
        stock.TryAddPrice(date, 50);
        
        portfolio.AddStock(stock, 100);

        // Act
        decimal actual = portfolio.Profit(date, date, isAnnualized);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Profit_CumulativeReturn_ShouldThrowArgumentException_IfStartIsAfterEnd()
    {
        ShouldThrowArgumentException_IfStartIsAfterEnd(false);
    }
    
    [Fact]
    public void Profit_AnnualizedReturn_ShouldThrowArgumentException_IfStartIsAfterEnd()
    {
        ShouldThrowArgumentException_IfStartIsAfterEnd(true);
    }
    
    private void ShouldThrowArgumentException_IfStartIsAfterEnd(bool isAnnualized)
    {
        // Arrange
        Portfolio portfolio = new Portfolio();

        DateOnly start = DateOnly.FromDateTime(DateTime.Now);
        DateOnly end = start.AddYears(-1); // Before
        
        Stock stock = new Stock("Fintual");
        stock.TryAddPrice(start, 10);
        stock.TryAddPrice(end, 20);
        
        portfolio.AddStock(stock, 100);

        // Act & assert
        Assert.Throws<ArgumentException>(() => portfolio.Profit(start, end, isAnnualized));
    }
    
    [Fact]
    public void Profit_CumulativeReturn_ShouldCalculateCorrectlyForOneStock()
    {
        // Arrange
        const decimal expected = 0.04m;
        Portfolio portfolio = new Portfolio();

        DateOnly start = DateOnly.FromDateTime(DateTime.Now);
        DateOnly end = start.AddYears(1);
        
        Stock stock = new Stock("Fintual");
        stock.TryAddPrice(start, 50);
        stock.TryAddPrice(end, 52);
        
        portfolio.AddStock(stock, 100);

        // Act
        decimal actual = portfolio.Profit(start, end, false);

        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void Profit_CumulativeReturn_ShouldCalculateCorrectlyForMultipleStocks()
    {
        // Arrange
        const decimal expected = 0.04m;
        Portfolio portfolio = new Portfolio();

        DateOnly start = DateOnly.FromDateTime(DateTime.Now);
        DateOnly end = start.AddYears(1);
        
        Stock stock1 = new Stock("Fintual 1");
        stock1.TryAddPrice(start, 7);
        stock1.TryAddPrice(end, 8);
        
        Stock stock2 = new Stock("Fintual 2");
        stock2.TryAddPrice(start, 11);
        stock2.TryAddPrice(end, 10);
        
        portfolio.AddStock(stock1, 4);
        portfolio.AddStock(stock2, 2);

        // Act
        decimal actual = portfolio.Profit(start, end, false);

        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void Profit_CumulativeReturn_ShouldCalculateCorrectlyForMultipleStocks2()
    {
        // Arrange
        const decimal expected = 0.05m;
        Portfolio portfolio = new Portfolio();

        DateOnly start = DateOnly.FromDateTime(DateTime.Now);
        DateOnly end = start.AddYears(1);
        
        Stock stock1 = new Stock("Fintual 1");
        stock1.TryAddPrice(start, 87);
        stock1.TryAddPrice(end, 29);
        
        Stock stock2 = new Stock("Fintual 2");
        stock2.TryAddPrice(start, 17);
        stock2.TryAddPrice(end, 20);
        
        portfolio.AddStock(stock1, 25);
        portfolio.AddStock(stock2, 725);

        // Act
        decimal actual = portfolio.Profit(start, end, false);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    void Profit_AnnualizedReturn_ShouldCalculateCorrectlyForYearlyPeriod()
    {
        // Arrange
        decimal expected = (decimal)Math.Pow(1.30903, 1.0/5.0) - 1m;
        Portfolio portfolio = new Portfolio();
        
        Stock stock = new Stock("Fintual");

        DateOnly start = DateOnly.FromDateTime(DateTime.Now);

        DateOnly end = start.AddYears(5).AddDays(-1);

        stock.TryAddPrice(start, 100);
        
        stock.TryAddPrice(end, 130.903m);
        
        portfolio.AddStock(stock, 1);

        // Act
        decimal actual = portfolio.Profit(start, end);
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    void Profit_AnnualizedReturn_ShouldCalculateCorrectlyForNonYearlyPeriod()
    {
        // Arrange
        decimal expected = (decimal)Math.Pow(1.2374, 365.0 / 575.0) - 1m;
        Portfolio portfolio = new Portfolio();
        
        Stock stock = new Stock("Fintual");

        DateOnly start = DateOnly.FromDateTime(DateTime.Now);

        DateOnly end = start.AddDays(575);

        stock.TryAddPrice(start, 100);
        
        stock.TryAddPrice(end, 123.74m);
        
        portfolio.AddStock(stock, 1);

        // Act
        decimal actual = portfolio.Profit(start, end);
        
        // Assert
        Assert.Equal(expected, actual);
    }
}