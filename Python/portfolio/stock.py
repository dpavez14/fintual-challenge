# -*- coding: utf-8 -*-
from datetime import date
from decimal import Decimal


class Stock:
    """Represents a stock with a unique name and registers the variations of it's price in a dictionary.

    Attributes:
        _name: Name of the stock.
        _prices: Variation of the price of the stock on time. It's a dictionary that correlates a `date` with the
        price of the stock on that date (date:price).

    Notes:
        - The name of the stock must be unique.
        - The prices are set by days, so it doesn't consider the variations in shorter periods of time like hours,
        minutes, seconds, milliseconds, etc., which could be the case in a real-world exchange, which is out of scope.
    """

    def __init__(self, name: str):
        """Initialize a new Stock with a given name and prices not set yet.
        Args:
            name: Name of the stock. It's assumed that the name of the stock is unique.
        """
        self._name = name
        self._prices = {}

    def get_name(self) -> str:
        """Gets the stock's name.
        Returns:
            The name of the stock.
        """
        return self._name

    def add_price(self, day: date, price: Decimal) -> bool:
        """Sets the price of the stock in a certain date.
        Args:
            day: Date to set the price.
            price: Price to set.
        Returns:
            True if the price was added successfully (wasn't set previously for the date given); otherwise, False.
        """
        if self._prices.__contains__(day):
            return False
        else:
            self._prices[day] = price
            return True

    def price(self, day: date) -> Decimal:
        """Gets the price of the stock on a certain date.
        Args:
            day: Date to get the price.
        Returns:
            The price of the stock in the given date.
        Raises:
            KeyError: If doesn't exist a price set on the given date.
        """
        return self._prices[day]

    def __hash__(self):
        """Return the hash based on the `Stock` name (as it must be unique)"""
        return self._name.__hash__()
