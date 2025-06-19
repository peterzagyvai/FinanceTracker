using System;
using FinanceTracker.Core.Interfaces;

namespace FinanceTracker.Core.Models;

public class Purchase : TransactionSource
{
    private DateTime _dateOfPurchase;
    public override DateTime DateOfTransaction
    {
        get { return _dateOfPurchase; }
        set
        {
            DateOfPurchase = value;
        }
    }

    public DateTime DateOfPurchase
    {
        get { return _dateOfPurchase; }
        set
        {
            if (value.Equals(_dateOfPurchase))
            {
                return;
            }

            _dateOfPurchase = value;
        }
    }

    private readonly List<PurchasedItem> _purchasedItems;
    public List<PurchasedItem> PurchasedItems { get { return _purchasedItems; } }

    public Purchase(DateTime dateOfPurchase, ITransactionParticipant from, ITransactionParticipant to)
        : base(from, to)
    {
        _purchasedItems = new();
    }

    public void AddItem(Item item, decimal amount, Money pricePerUnit)
    {
        PurchasedItems.Add(
            new PurchasedItem(amount, pricePerUnit, item)
        );

        From.TransactionAmount = TotalPriceInCurrency(From.TransactionAmount.CurrencyISO).Negate();
        To.TransactionAmount = TotalPriceInCurrency(To.TransactionAmount.CurrencyISO);
    }

    public void RemoveItem(PurchasedItem item)
    {
        PurchasedItems.Remove(item);
    }

    public Money TotalPriceInCurrency(string isoCode)
    {
        decimal sumPrice = 0;
        foreach (var item in PurchasedItems)
        {
            sumPrice += item.Price.ConvertAmountToCurrency(isoCode);
        }

        return new Money()
        {
            Amount = sumPrice,
            CurrencyISO = isoCode
        };
    }
}
