using System;
using FinanceTrackerApi.Utilities;

namespace FinanceTrackerApi.Models;

public class PurchasedItem
{
    private decimal _amount;
    public decimal Amount
    {
        get { return _amount; }
        set
        {
            if (value == _amount)
            {
                return;
            }

            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"Amount can't be less or equal to 0. Current value: {value}");
            }

            _amount = value;
        }
    }

    private Money _price;
    public Money Price
    {
        get { return _price; }
        set
        {
            if (value == _price)
            {
                return;
            }

            if (value.Amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"Price of an item can't be less than 0. Current price is {value}");
            }

            _price = value;
        }
    }

    public Item Item { get; set; }

    public PurchasedItem(decimal amount, Money price, Item item)
    {
        Amount = amount;
        Price = price;
        Item = item;
    }
}
