using System;
using System.Collections.Generic;
using Challenge;
using Xunit;

namespace ChallengeTests;

public class StockTests
{
    [Fact]
    public void TryAddPrice_ShouldAddAPrice()
    {
        // Arrange
        Stock stock = new Stock("Fintual");
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        decimal price = 1;
        
        // Act
        var isAdded = stock.TryAddPrice(date, price);
        
        // Assert
        Assert.True(isAdded);
    }
    
    [Fact]
    public void TryAddPrice_ShouldAddPriceForDifferentDates()
    {
        // Arrange
        Stock stock = new Stock("Fintual");
        
        DateOnly date1 = DateOnly.FromDateTime(DateTime.Now);
        decimal price1 = 1.1m;

        DateOnly date2 = date1.AddDays(1);
        decimal price2 = 2.2m;
        
        // Act
        var isAdded1 = stock.TryAddPrice(date1, price1);
        var isAdded2 = stock.TryAddPrice(date2, price2);
        
        // Assert
        Assert.True(isAdded1);
        Assert.True(isAdded2);
    }
    
    [Fact]
    public void TryAddPrice_ShouldNotAddPricesOnTheSameDate()
    {
        // Arrange
        Stock stock = new Stock("Fintual");
        
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        
        decimal price1 = 1.1m;
        decimal price2 = 2.2m;
        
        // Act
        var isAdded1 = stock.TryAddPrice(date, price1);
        var isAdded2 = stock.TryAddPrice(date, price2);
        
        // Assert
        Assert.True(isAdded1);
        Assert.False(isAdded2);
    }

    [Fact]
    public void Price_ShouldThrowException_IfArentPricesAdded()
    {
        // Arrange
        Stock stock = new Stock("Fintual");
        
        DateOnly date1 = DateOnly.FromDateTime(DateTime.Now);
        
        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => stock.Price(date1));
    }
    
    
    [Fact]
    public void Price_ShouldGetTheCorrectPriceForAGivenDate()
    {
        // Arrange
        Stock stock = new Stock("Fintual");
        
        DateOnly date1 = DateOnly.FromDateTime(DateTime.Now);
        decimal price1 = 1.1m;

        DateOnly date2 = date1.AddYears(1);
        decimal price2 = 2.2m;
        
        stock.TryAddPrice(date1, price1);
        stock.TryAddPrice(date2, price2);
        
        // Act
        var result1 = stock.Price(date1);
        var result2 = stock.Price(date2);
        
        // Assert
        Assert.Equal(price1, result1);
        Assert.Equal(price2, result2);
    }
}