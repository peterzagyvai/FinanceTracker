using System;
using System.Dynamic;
using System.Numerics;
using System.Resources;
using System.Threading.Tasks;
using FinanceTracker.Core.Models;
using FinanceTrackerCore.Models;
using FinanceTrackerCore.Repositories;

namespace FinanceTrackerCore.Helpers;

public class CurrencyHelper
{
    private static readonly Dictionary<string, string> ISOCodeSymbolDictionary = new()
    {
        { "ALL" , "Lek" },
        { "AFN" , "؋" },
        { "ARS" , "$" },
        { "AWG" , "ƒ" },
        { "AUD" , "$" },
        { "AZN" , "₼" },
        { "BSD" , "$" },
        { "BBD" , "$" },
        { "BYN" , "Br" },
        { "BZD" , "BZ$" },
        { "BMD" , "$" },
        { "BOB" , "$b" },
        { "BAM" , "KM" },
        { "BWP" , "P" },
        { "BGN" , "лв" },
        { "BRL" , "R$" },
        { "BND" , "$" },
        { "KHR" , "៛" },
        { "CAD" , "$" },
        { "KYD" , "$" },
        { "CLP" , "$" },
        { "CNY" , "¥" },
        { "COP" , "$" },
        { "CRC" , "₡" },
        { "HRK" , "kn" },
        { "CUP" , "₱" },
        { "CZK" , "Kč" },
        { "DKK" , "kr" },
        { "DOP" , "RD$" },
        { "XCD" , "$" },
        { "EGP" , "£" },
        { "SVC" , "$" },
        { "EUR" , "€" },
        { "FKP" , "£" },
        { "FJD" , "$" },
        { "GHS" , "¢" },
        { "GIP" , "£" },
        { "GTQ" , "Q" },
        { "GGP" , "£" },
        { "GYD" , "$" },
        { "HNL" , "L" },
        { "HKD" , "$" },
        { "HUF" , "Ft" },
        { "ISK" , "kr" },
        { "INR" , "" },
        { "IDR" , "Rp" },
        { "IRR" , "﷼" },
        { "IMP" , "£" },
        { "ILS" , "₪" },
        { "JMD" , "J$" },
        { "JPY" , "¥" },
        { "JEP" , "£" },
        { "KZT" , "лв" },
        { "KPW" , "₩" },
        { "KRW" , "₩" },
        { "KGS" , "лв" },
        { "LAK" , "₭" },
        { "LBP" , "£" },
        { "LRD" , "$" },
        { "MKD" , "ден" },
        { "MYR" , "RM" },
        { "MUR" , "₨" },
        { "MXN" , "$" },
        { "MNT" , "₮" },
        { "MZN" , "MT" },
        { "NAD" , "$" },
        { "NPR" , "₨" },
        { "ANG" , "ƒ" },
        { "NZD" , "$" },
        { "NIO" , "C$" },
        { "NGN" , "₦" },
        { "NOK" , "kr" },
        { "OMR" , "﷼" },
        { "PKR" , "₨" },
        { "PAB" , "B/." },
        { "PYG" , "Gs" },
        { "PEN" , "S/." },
        { "PHP" , "₱" },
        { "PLN" , "zł" },
        { "QAR" , "﷼" },
        { "RON" , "lei" },
        { "RUB" , "₽" },
        { "SHP" , "£" },
        { "SAR" , "﷼" },
        { "RSD" , "Дин." },
        { "SCR" , "₨" },
        { "SGD" , "$" },
        { "SBD" , "$" },
        { "SOS" , "S" },
        { "ZAR" , "R" },
        { "LKR" , "₨" },
        { "SEK" , "kr" },
        { "CHF" , "CHF" },
        { "SRD" , "$" },
        { "SYP" , "£" },
        { "TWD" , "NT$" },
        { "THB" , "฿" },
        { "TTD" , "TT$" },
        { "TRY" , "" },
        { "TVD" , "$" },
        { "UAH" , "₴" },
        { "GBP" , "£" },
        { "USD" , "$" },
        { "UYU" , "$U" },
        { "UZS" , "лв" },
        { "VEF" , "Bs" },
        { "VND" , "₫" },
        { "YER" , "﷼" },
        { "ZWD" , "Z$" }
    };
    private readonly ExchangeRatesApiRepository _repository;

    public CurrencyHelper(ExchangeRatesApiRepository repository)
    {
        _repository = repository;
    }

    public static CurrencyHelper GetDefaultHelper()
    {
        string? key = EnvHelper.GetExchangeRatesKey().Result;

        if (key is null)
        {
            throw new Exception("Could not find Exchange rates api key");
        }

        return new CurrencyHelper(new ExchangeRatesApiRepository(key));
    }

    public static bool IsValidISOCode(string isoCode)
    {
        return ISOCodeSymbolDictionary.ContainsKey(isoCode.ToUpper());
    }

    public static string ISOToSymbol(string isoCode)
    {
        return ISOCodeSymbolDictionary[isoCode.ToUpper()];
    }

    public Money ExchangeToNewCurrency(Money money, string to, DateTime date)
    {
        if (!IsValidISOCode(to))
        {
            throw new Exception($"{to} is not a valid ISO currency");
        }

        CurrencyRateInfo? info = _repository.GetCurrencyRates(date).Result;
        if (info is null)
        {
            throw new Exception("Getting currency info failed");
        }

        decimal moneyAmountInBase = money.Amount / info.Rates[info.Base];
        decimal moneyAmountInNewCurrency = moneyAmountInBase * info.Rates[to.ToUpper()];

        return new Money()
        {
            Amount = moneyAmountInNewCurrency,
            CurrencyISO = to
        };
    }

    public Money Add(Money m1, Money m2)
    {
        if (m1.CurrencyISO.ToUpper().Equals(m2.CurrencyISO.ToUpper()))
        {
            return new Money()
            {
                Amount = m1.Amount + m2.Amount,
                CurrencyISO = m1.CurrencyISO
            };
        }

        decimal sumAmount = m1.Amount + ExchangeToNewCurrency(m2, m1.CurrencyISO, DateTime.Today).Amount;
        return new Money()
        {
            Amount = sumAmount,
            CurrencyISO = m1.CurrencyISO
        };
    } 

    public Money Sub(Money m1, Money m2)
    {
        if (m1.CurrencyISO.ToUpper().Equals(m2.CurrencyISO.ToUpper()))
        {
            return new Money()
            {
                Amount = m1.Amount - m2.Amount,
                CurrencyISO = m1.CurrencyISO
            };
        }

        decimal difAmount = m1.Amount - ExchangeToNewCurrency(m2, m1.CurrencyISO, DateTime.Today).Amount;
        return new Money()
        {
            Amount = difAmount,
            CurrencyISO = m1.CurrencyISO
        };
    } 
}
