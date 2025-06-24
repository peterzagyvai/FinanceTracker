using System;
using FinanceTrackerCore.Helpers;
using FinanceTrackerCore.Interfaces;

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

            if (!_currencyHelper.IsValidISOCode(value))
            {
                throw new ArgumentException($"{value} is not a valid ISO currency code");
            }

            _currencyISO = value;
        }
    }

    public Money(ICurrencyHelper currencyHelper)
    {
        _currencyHelper = currencyHelper;
    }

    /// <summary>
    /// Returns the symbol of the currency's iso code
    /// </summary>
    /// <returns>The symbol of the currency's iso code</returns>
    private static string ISOToSymbol(string isoCode)
    {
        return _currencyHelper.ISOToSymbol(isoCode);
    }

    //TODO: implement later
    /// <summary>
    /// Returns the amount property of the money in the currency given in the parameters
    /// </summary>
    /// <param name="isoCode"></param>
    /// <returns>The amount of money in the currency given in the parameters</returns>
    public decimal GetAmountInCurrency(string isoCode)
    {
        return Amount;
    }

    //TODO: implement later
    /// <summary>
    /// Converts the money to a different currency.
    /// </summary>
    /// <param name="isoCode"></param>
    public void ConvertAmountToCurrency(string isoCode)
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
        return $"{Amount} {ISOToSymbol(CurrencyISO)}";
    }

    /// <summary>
    /// Returns the sum of two money object represented with the first one's currency
    /// </summary>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <returns>The sum of two money object represented with the first one's currency</returns>
    public static Money operator +(Money m1, Money m2)
    {
        return new Money()
        {
            Amount = m1.Amount + m2.GetAmountInCurrency(m1.CurrencyISO),
            CurrencyISO = m1.CurrencyISO
        };
    }

    /// <summary>
    /// Returns the difference of two money object represented with the first one's currency
    /// </summary>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <returns>The difference of two money object represented with the first one's currency</returns>
    public static Money operator -(Money m1, Money m2)
    {
        return new Money()
        {
            Amount = m1.Amount - m2.GetAmountInCurrency(m1.CurrencyISO),
            CurrencyISO = m1.CurrencyISO
        };
    }

    /// <summary>
    /// Checks if the value of the first money is more than the second one's value
    /// </summary>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <returns>True if the value of the first money object is more than the second. False otherwise</returns>
    public static bool operator >(Money m1, Money m2)
    {
        decimal m1Amount = m1.Amount;
        decimal m2Amount = m2.GetAmountInCurrency(m1.CurrencyISO);

        return m1Amount > m2Amount;
    }

    /// <summary>
    /// Checks if the value of the first money is less than the second one's value
    /// </summary>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <returns>True if the value of the first money object is more less the second. False otherwise</returns>
    public static bool operator <(Money m1, Money m2)
    {
        decimal m1Amount = m1.Amount;
        decimal m2Amount = m2.GetAmountInCurrency(m1.CurrencyISO);

        return m1Amount < m2Amount;
    }

    /// <summary>
    /// Checks if the values of the two money object (regardless of currency) are equal
    /// </summary>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <returns>True if the values of the two money object (regardless of currency) are equal. False otherwise</returns>
    public static bool operator ==(Money m1, Money m2)
    {
        decimal m1Amount = m1.Amount;
        decimal m2Amount = m2.GetAmountInCurrency(m1.CurrencyISO);

        return m1Amount == m2Amount;
    }

    /// <summary>
    /// Checks if the values of the two money object (regardless of currency) are NOT equal
    /// </summary>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <returns>True if the values of the two money object (regardless of currency) are NOT equal. False otherwise</returns>
    public static bool operator !=(Money m1, Money m2)
    {
        return !(m1 == m2);
    }
}
