using System;
using System.Collections.ObjectModel;
using FinanceTracker.Core.Interfaces;
using FinanceTrackerCore.Helpers;

namespace FinanceTracker.Core.Models;

public class Purchase : TransactionSource
{
    private readonly CurrencyHelper _currencyHelper;
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
    public ReadOnlyCollection<PurchasedItem> PurchasedItems { get { return _purchasedItems.AsReadOnly(); } }

    public Purchase(DateTime dateOfPurchase, ITransactionParticipant from, ITransactionParticipant to)
        : base(from, to)
    {
        _currencyHelper = CurrencyHelper.GetDefaultHelper();
        _purchasedItems = new();
    }

    /// <summary>
    /// Adds a new purchasablew item to the PurchasedItems collection
    /// </summary>
    /// <param name="item">The item that will be added to the collection</param>
    /// <param name="amount">The amount of the items added to the colelction</param>
    /// <param name="pricePerUnit">The current price of the item per unit</param>
    public void AddItem(Item item, decimal amount, Money pricePerUnit)
    {
        _purchasedItems.Add(
            new PurchasedItem(amount, pricePerUnit, item)
        );

        Transaction.TransactionAmount = TotalPriceInCurrency(Transaction.TransactionAmount.CurrencyISO);
    }

    /// <summary>
    /// Removes an item from the Purchased items collection
    /// </summary>
    /// <param name="item">The item that will be removed from the colelction</param>
    public void RemoveItem(PurchasedItem item)
    {
        _purchasedItems.Remove(item);
    }

    /// <summary>
    /// Calculates the sum price of the purchsed items in a pre defiend currency
    /// </summary>
    /// <param name="isoCode">The iso code of the currency</param>
    /// <returns>The sum price of the pruchaed items</returns>
    public Money TotalPriceInCurrency(string isoCode)
    {
        decimal sumPrice = 0;
        foreach (var item in PurchasedItems)
        {
            sumPrice += _currencyHelper.ExchangeToNewCurrency(item.Price, isoCode, DateOfPurchase).Amount;
        }

        return new Money()
        {
            Amount = sumPrice,
            CurrencyISO = isoCode
        };
    }
}
