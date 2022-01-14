# -*- coding: utf-8 -*-
import unittest
from datetime import date
from decimal import Decimal

from dateutil.relativedelta import relativedelta

from portfolio.stock import Stock


class StockAddPrice(unittest.TestCase):
    def test_add_price(self):
        # Arrange
        stock = Stock('Fintual')
        day = date.today()

        # Act
        is_added = stock.add_price(day, Decimal(1))

        # Assert
        self.assertTrue(is_added)

    def test_add_different_dates(self):
        # Arrange
        stock = Stock('Fintual')

        day1 = date.today()
        price1 = Decimal(11) / Decimal(10)

        day2 = day1 + relativedelta(days=1)
        price2 = Decimal(22) / Decimal(10)

        # Act
        is_added1 = stock.add_price(day1, price1)
        is_added2 = stock.add_price(day2, price2)

        # Assert
        self.assertTrue(is_added1)
        self.assertTrue(is_added2)

    def test_prices_same_date(self):
        # Arrange
        stock = Stock('Fintual')

        day = date.today()
        price1 = Decimal(11) / Decimal(10)
        price2 = Decimal(22) / Decimal(10)

        # Act
        is_added1 = stock.add_price(day, price1)
        is_added2 = stock.add_price(day, price2)

        # Assert
        self.assertTrue(is_added1)
        self.assertFalse(is_added2)


class StockPrice(unittest.TestCase):
    def test_price_not_added(self):
        # Arrange
        stock = Stock('Fintual')

        day = date.today()

        # Act & Assert
        self.assertRaises(KeyError, stock.price, day)

    def test_get_prices(self):
        # Arrange
        stock = Stock('Fintual')

        day1 = date.today()
        day2 = date.today() + relativedelta(days=1)
        price1 = Decimal(11) / Decimal(10)
        price2 = Decimal(22) / Decimal(10)

        stock.add_price(day1, price1)
        stock.add_price(day2, price2)

        # Act
        result1 = stock.price(day1)
        result2 = stock.price(day2)

        # Assert
        self.assertEqual(price1, result1)
        self.assertEqual(price2, result2)


class StockGetName(unittest.TestCase):
    def test_get_name(self):
        # Arrange
        name = 'Fintual'
        stock = Stock(name)

        # Act
        name_got = stock.get_name()

        # Assert
        self.assertEqual(name, name_got)


class StockHash(unittest.TestCase):
    def test_hash_depends_on_name(self):
        # Arrange
        expected = 'Fintual'.__hash__()

        day1 = date.today()
        day2 = day1 + relativedelta(days=1)
        day3 = day1 + relativedelta(days=2)

        stock1 = Stock('Fintual')
        stock1.add_price(day1, Decimal(1))

        stock2 = Stock('Fintual')
        stock1.add_price(day2, Decimal(2))

        stock3 = Stock("Fintual")
        stock1.add_price(day3, Decimal(3))

        stock4 = Stock('Fintual 2')
        stock4.add_price(day1, Decimal(1))

        # Act
        actual1 = stock1.__hash__()
        actual2 = stock2.__hash__()
        actual3 = stock3.__hash__()
        actual4 = stock4.__hash__()

        # Assert
        self.assertEqual(expected, actual1)
        self.assertEqual(expected, actual2)
        self.assertEqual(expected, actual3)

        self.assertNotEqual(expected, actual4)

        self.assertNotEqual(actual1, actual4)
        self.assertNotEqual(actual2, actual4)
        self.assertNotEqual(actual3, actual4)


if __name__ == '__main__':
    unittest.main()  # pragma: no cover
