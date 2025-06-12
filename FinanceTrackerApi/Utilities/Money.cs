using System;

namespace FinanceTrackerApi.Utilities;

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

            if (!IsValidISOCode(value))
            {
                throw new ArgumentException($"{value} is not a valid ISO currency code");
            }

            _currencyISO = value;
        }
    }

    //TODO: implement later 
    private static bool IsValidISOCode(string isoCode)
    {
        return true;
    }

    //TODO: implement later
    private static string ISOToDisplayeble(string isoCode)
    {
        return isoCode;
    }

    //TODO: implement later
    public decimal ConvertAmountToCurrency(string convertToIsoCode)
    {
        return Amount;
    }

    public Money Negate()
    {
        return new Money()
        {
            Amount = -this.Amount,
            CurrencyISO = _currencyISO
        };
    }

    public override string ToString()
    {
        return $"{Amount} {ISOToDisplayeble(CurrencyISO)}";
    }
}
