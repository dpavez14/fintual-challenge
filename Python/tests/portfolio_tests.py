# -*- coding: utf-8 -*-
import unittest
from decimal import Decimal
from datetime import date
from dateutil.relativedelta import relativedelta

from portfolio.portfolio import Portfolio
from portfolio.stock import Stock


class PortfolioAddStock(unittest.TestCase):
    def test_negative_amount(self):
        # Arrange
        amount = -1
        portfolio = Portfolio()
        stock = Stock('Fintual')

        # Act & Assert
        self.assertRaises(ValueError, portfolio.add_stock, stock, amount)

    def test_zero_amount(self):
        # Arrange
        amount = 0
        portfolio = Portfolio()
        stock = Stock('Fintual')

        # Act & Assert
        self.assertRaises(ValueError, portfolio.add_stock, stock, amount)

    def test_single_positive_amount(self):
        # Arrange
        amount = Decimal(1)
        portfolio = Portfolio()
        stock = Stock('Fintual')

        # Act
        actual = portfolio.add_stock(stock, amount)

        # Assert
        self.assertEqual(amount, actual)

    def test_multiple_positive_amounts(self):
        amount1 = Decimal(1)
        amount2 = Decimal(9)
        expected1 = amount1
        expected2 = amount1 + amount2

        portfolio = Portfolio()
        stock = Stock('Fintual')

        # Act
        actual1 = portfolio.add_stock(stock, amount1)
        actual2 = portfolio.add_stock(stock, amount2)

        # Assert
        self.assertEqual(expected1, actual1)
        self.assertEqual(expected2, actual2)


class PortfolioProfitCommon:
    @staticmethod
    def without_stocks(parent: unittest.TestCase, is_annualized: bool) -> None:
        # Arrange
        expected = 0
        portfolio = Portfolio()

        start = date.today()
        end = start + relativedelta(years=1)

        # Act
        actual = portfolio.profit(start, end, is_annualized)

        # Assert
        parent.assertEqual(expected, actual)

    @staticmethod
    def start_and_end_equals(parent: unittest.TestCase, is_annualized: bool) -> None:
        # Arrange
        expected = 0
        portfolio = Portfolio()

        day = date.today()

        stock = Stock('Fintual')
        stock.add_price(day, Decimal(50))
        stock.add_price(day, Decimal(20))

        portfolio.add_stock(stock, Decimal(100))

        # Act
        actual = portfolio.profit(day, day, is_annualized)

        # Assert
        parent.assertEqual(expected, actual)

    @staticmethod
    def start_after_end(parent: unittest.TestCase, is_annualized: bool) -> None:
        # Arrange
        portfolio = Portfolio()

        start = date.today()
        end = start - relativedelta(years=1)

        stock = Stock('Fintual')
        stock.add_price(start, Decimal(10))
        stock.add_price(end, Decimal(20))

        portfolio.add_stock(stock, Decimal(100))

        # Act & Assert
        parent.assertRaises(ValueError, portfolio.profit, start, end, is_annualized)


class PortfolioProfitCumulative(unittest.TestCase):
    def test_without_stocks(self):
        PortfolioProfitCommon.without_stocks(self, False)

    def test_start_and_end_equals(self):
        PortfolioProfitCommon.start_and_end_equals(self, False)

    def test_start_after_end(self):
        PortfolioProfitCommon.start_after_end(self, False)

    def test_calculate_one_stock(self):
        # Arrange
        expected = Decimal(4) / Decimal(100)
        portfolio = Portfolio()

        start = date.today()
        end = start + relativedelta(years=1)

        stock = Stock('Fintual')
        stock.add_price(start, Decimal(50))
        stock.add_price(end, Decimal(52))

        portfolio.add_stock(stock, Decimal(100))

        # Act
        actual = portfolio.profit(start, end, False)

        # Assert
        self.assertEqual(expected, actual)

    def test_calculate_multiple_stocks(self):
        # Arrange
        expected = Decimal(4) / Decimal(100)
        portfolio = Portfolio()

        start = date.today()
        end = start + relativedelta(years=1)

        stock1 = Stock('Fintual 1')
        stock1.add_price(start, Decimal(7))
        stock1.add_price(end, Decimal(8))

        stock2 = Stock('Fintual 2')
        stock2.add_price(start, Decimal(11))
        stock2.add_price(end, Decimal(10))

        portfolio.add_stock(stock1, Decimal(4))
        portfolio.add_stock(stock2, Decimal(2))

        # Act
        actual = portfolio.profit(start, end, False)

        # Assert
        self.assertEqual(expected, actual)

    def test_calculate_multiple_stocks2(self):
        # Arrange
        expected = Decimal(5) / Decimal(100)
        portfolio = Portfolio()

        start = date.today()
        end = start + relativedelta(years=1)

        stock1 = Stock('Fintual 1')
        stock1.add_price(start, Decimal(87))
        stock1.add_price(end, Decimal(29))

        stock2 = Stock('Fintual 2')
        stock2.add_price(start, Decimal(17))
        stock2.add_price(end, Decimal(20))

        portfolio.add_stock(stock1, Decimal(25))
        portfolio.add_stock(stock2, Decimal(725))

        # Act
        actual = portfolio.profit(start, end, False)

        # Assert
        self.assertEqual(expected, actual)


class PortfolioProfitAnnualized(unittest.TestCase):
    def test_without_stocks(self):
        PortfolioProfitCommon.without_stocks(self, True)

    def test_start_and_end_equals(self):
        PortfolioProfitCommon.start_and_end_equals(self, True)

    def test_start_after_end(self):
        PortfolioProfitCommon.start_after_end(self, True)

    def test_calculate_yearly(self):
        # Arrange
        expected = (Decimal(130903) / Decimal(100000)) ** (Decimal(1) / Decimal(5)) - 1
        portfolio = Portfolio()

        start = date.today()
        end = start + relativedelta(years=5) - relativedelta(days=1)

        stock = Stock('Fintual')
        stock.add_price(start, Decimal(100))
        stock.add_price(end, Decimal(130903) / Decimal(1000))

        portfolio.add_stock(stock, Decimal(1))

        # Act
        actual = portfolio.profit(start, end)

        # Assert
        self.assertEqual(expected, actual)

    def test_calculate_non_yearly(self):
        # Arrange
        expected = (Decimal(12374) / Decimal(10000)) ** (Decimal(365) / Decimal(575)) - 1
        portfolio = Portfolio()

        start = date.today()
        end = start + relativedelta(days=575)

        stock = Stock('Fintual')
        stock.add_price(start, Decimal(100))
        stock.add_price(end, Decimal(12374) / Decimal(100))

        portfolio.add_stock(stock,  Decimal(100))

        # Act
        actual = portfolio.profit(start, end)

        # Assert
        self.assertEqual(expected, actual)


if __name__ == '__main__':
    unittest.main()  # pragma: no cover
