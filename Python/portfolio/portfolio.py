# -*- coding: utf-8 -*-
from decimal import Decimal
from datetime import date, timedelta

from portfolio.stock import Stock


class Portfolio:
    """Portfolio which contains a dictionary with the stocks of a client.

    Attributes:
        _stocks (dict): Dictionary that correlates a stock with the amount that the client possesses (stock:amount).
    """

    def __init__(self):
        """Initialize a new Portfolio without stocks."""
        self._stocks = {}

    def add_stock(self, stock: Stock, amount: Decimal) -> Decimal:
        """Adds a certain ``amount`` of the given `stock`` to the portfolio.
        Args:
            stock: Stock to add.
            amount: Amount to add of the given stock.
        Returns:
            The current total amount of the stock in the portfolio.
        Raises:
            ValueError: If the ``amount`` is 0 or negative.
        """
        if amount <= 0:
            raise ValueError("The given amount must be above 0")
        if stock not in self._stocks:
            self._stocks[stock] = Decimal(0)
        self._stocks[stock] += amount
        return self._stocks[stock]

    def profit(self, start: date, end: date, is_annualized: bool = True) -> Decimal:
        """By default calculate the annualized return of the portfolio, if ``is_annualized``
        is set to False, calculate the cumulative return instead.
        Args:
            start: Initial date of the period.
            end: Final date of the period.
            is_annualized: If True calculate the annualized return; otherwise, calculate the cumulative return.
        Returns:
            The profit of the portfolio (annualized or cumulative).
        Raises:
            ValueError: If ``start`` is a date afeter ``end``.
        """
        if end < start:
            raise ValueError("The given end date is before start")
        if is_annualized:
            return self.annualized_return(start, end)
        return self.cumulative_return(start, end)

    def cumulative_return(self, start: date, end: date) -> Decimal:
        """Calculate the cumulative return between the dates given.

        See: `'Calculating the Percentage Return of Your Portfolio' on www.investopedia.com <https://www.investopedia.com/ask/answer/07/portfolio_calculations.asp#toc-calculating-the-percentage-return-of-your-portfolio>`_

        See also: `'Cumulative Return' on www.investopedia.com <https://www.investopedia.com/terms/c/cumulativereturn.asp>`_
        Args:
            start: Initial date of the period.
            end: Final date of the period.
        Returns:
            The cumulative return of the portfolio.
        """
        if start >= end or len(self._stocks) <= 0:
            return Decimal(0)
        else:
            start_total = Decimal(0)
            end_total = Decimal(0)
            for stock in self._stocks.keys():
                stock_amount = self._stocks[stock]
                start_total += (stock_amount * stock.price(start))
                end_total += (stock_amount * stock.price(end))
            return (end_total - start_total) / start_total

    def annualized_return(self, start: date, end: date) -> Decimal:
        """Calculate the annualized return between the dates given.

        See: `'Annualized Total Return' on www.investopedia.com <https://www.investopedia.com/terms/a/annualized-total-return.asp>`_
        Args:
            start: Initial date of the period.
            end: Final date of the period.
        Returns:
            The annualized return of the portfolio.
        """
        cumulative_return = self.cumulative_return(start, end)

        if cumulative_return <= 0:
            return Decimal(0)
        else:
            end_next_day = end + timedelta(days=1)
            # Period in years
            if start.month == end_next_day.month and start.day == end_next_day.day:
                years_held = Decimal(end_next_day.year - start.year)
                return (1 + cumulative_return) ** (Decimal(1) / years_held) - 1
            else:
                days_held = Decimal((end - start).days)
                return (1 + cumulative_return) ** (365 / days_held) - 1
