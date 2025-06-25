using System;
using FinanceTrackerCore.Helpers;

namespace FinanceTracker.Core.Models;

public class Money
{
    public decimal Amount { get; set; } = 0;
    private string _currencyISO = "EUR";
    public string CurrencyISO
    {
        get { return _currencyISO; }
        set
        {
            if (value == _currencyISO)
            {
                return;
            }

            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            if (!CurrencyHelper.IsValidISOCode(value))
            {
                throw new ArgumentException($"{value} is not a valid ISO currency code");
            }

            _currencyISO = value;
        }
    }

    public Money()
    {
    }

    /// <summary>
    /// Returns money with the same currancy and a negated amount
    /// </summary>
    /// <returns>Money with the same currancy and a negated amount</returns>
    public Money GetNegated()
    {
        return new Money()
        {
            Amount = -this.Amount,
            CurrencyISO = _currencyISO
        };
    }

    public override string ToString()
    {
        return $"{Amount} {CurrencyHelper.ISOToSymbol(CurrencyISO)}";
    }
}
