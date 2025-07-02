using System;
using System.Dynamic;
using System.Numerics;
using System.Resources;
using System.Threading.Tasks;
using FinanceTracker.Core.Models;
using FinanceTrackerCore.Interfaces;
using FinanceTrackerCore.Models;
using FinanceTrackerCore.Repositories;

namespace FinanceTrackerCore.Helpers;

public class CurrencyHelper
{
    private static readonly Dictionary<string, string> ISOCodeSymbolDictionary = new()
    {
        { "AFN" , "؋" },
        { "ARS" , "$" },
        { "ALL" , "Lek" },
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
    private static readonly HashSet<string> ConvertableISOCodes = new()
    {
        "AED",
        "AFN",
        "ALL",
        "AMD",
        "ANG",
        "AOA",
        "ARS",
        "AUD",
        "AWG",
        "AZN",
        "BAM",
        "BBD",
        "BDT",
        "BGN",
        "BHD",
        "BIF",
        "BMD",
        "BND",
        "BOB",
        "BRL",
        "BSD",
        "BTC",
        "BTN",
        "BWP",
        "BYN",
        "BYR",
        "BZD",
        "CAD",
        "CDF",
        "CHF",
        "CLF",
        "CLP",
        "CNY",
        "COP",
        "CRC",
        "CUC",
        "CUP",
        "CVE",
        "CZK",
        "DJF",
        "DKK",
        "DOP",
        "DZD",
        "EGP",
        "ERN",
        "ETB",
        "EUR",
        "FJD",
        "FKP",
        "GBP",
        "GEL",
        "GGP",
        "GHS",
        "GIP",
        "GMD",
        "GNF",
        "GTQ",
        "GYD",
        "HKD",
        "HNL",
        "HRK",
        "HTG",
        "HUF",
        "IDR",
        "ILS",
        "IMP",
        "INR",
        "IQD",
        "IRR",
        "ISK",
        "JEP",
        "JMD",
        "JOD",
        "JPY",
        "KES",
        "KGS",
        "KHR",
        "KMF",
        "KPW",
        "KRW",
        "KWD",
        "KYD",
        "KZT",
        "LAK",
        "LBP",
        "LKR",
        "LRD",
        "LSL",
        "LTL",
        "LVL",
        "LYD",
        "MAD",
        "MDL",
        "MGA",
        "MKD",
        "MMK",
        "MNT",
        "MOP",
        "MRU",
        "MUR",
        "MVR",
        "MWK",
        "MXN",
        "MYR",
        "MZN",
        "NAD",
        "NGN",
        "NIO",
        "NOK",
        "NPR",
        "NZD",
        "OMR",
        "PAB",
        "PEN",
        "PGK",
        "PHP",
        "PKR",
        "PLN",
        "PYG",
        "QAR",
        "RON",
        "RSD",
        "RUB",
        "RWF",
        "SAR",
        "SBD",
        "SCR",
        "SDG",
        "SEK",
        "SGD",
        "SHP",
        "SLE",
        "SLL",
        "SOS",
        "SRD",
        "STD",
        "SYP",
        "SZL",
        "THB",
        "TJS",
        "TMT",
        "TND",
        "TOP",
        "TRY",
        "TTD",
        "TWD",
        "TZS",
        "UAH",
        "UGX",
        "USD",
        "UYU",
        "UZS",
        "VEF",
        "VES",
        "VND",
        "VUV",
        "WST",
        "XAF",
        "XAG",
        "XAU",
        "XCD",
        "XDR",
        "XOF",
        "XPF",
        "YER",
        "ZAR",
        "ZMK",
        "ZMW",
        "ZWL"
    };

    private readonly IExchangeRepository _repository;

    public CurrencyHelper(IExchangeRepository repository)
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
        return ConvertableISOCodes.Contains(isoCode.ToUpper());
    }

    public static string? ISOToSymbol(string isoCode)
    {
        if (!IsValidISOCode(isoCode))
        {
            return null;
        }

        if (ISOCodeSymbolDictionary.ContainsKey(isoCode))
        {
            return ISOCodeSymbolDictionary[isoCode.ToUpper()];
        }

        return isoCode.ToUpper();
    }

    public static bool AreSameCurrencies(string isoCode1, string isoCode2)
    {
        return isoCode1.ToUpper().Equals(isoCode2.ToUpper());
    }

    public Money ExchangeToNewCurrency(Money money, string to, DateTime date)
    {
        if (!IsValidISOCode(to))
        {
            throw new ArgumentException($"{to} is not a valid ISO currency");
        }

        if (AreSameCurrencies(money.CurrencyISO, to))
        {
            return money;
        }

        CurrencyRateInfo? info = _repository.GetCurrencyRates(date).Result;
        if (info is null)
        {
            throw new InvalidOperationException("Getting currency info failed");
        }

        decimal moneyAmountInBase = money.Amount / info.Rates[info.Base];
        decimal moneyAmountInNewCurrency = moneyAmountInBase * info.Rates[to.ToUpper()];

        return new Money()
        {
            Amount = moneyAmountInNewCurrency,
            CurrencyISO = to
        };
    }

    public Money Add(Money m1, Money m2, DateTime date)
    {
        if (m1.CurrencyISO.ToUpper().Equals(m2.CurrencyISO.ToUpper()))
        {
            return new Money()
            {
                Amount = m1.Amount + m2.Amount,
                CurrencyISO = m1.CurrencyISO
            };
        }

        decimal sumAmount = m1.Amount + ExchangeToNewCurrency(m2, m1.CurrencyISO, date).Amount;
        return new Money()
        {
            Amount = sumAmount,
            CurrencyISO = m1.CurrencyISO
        };
    }

    public Money Sub(Money m1, Money m2, DateTime date)
    {
        if (m1.CurrencyISO.ToUpper().Equals(m2.CurrencyISO.ToUpper()))
        {
            return new Money()
            {
                Amount = m1.Amount - m2.Amount,
                CurrencyISO = m1.CurrencyISO
            };
        }

        decimal difAmount = m1.Amount - ExchangeToNewCurrency(m2, m1.CurrencyISO, date).Amount;
        return new Money()
        {
            Amount = difAmount,
            CurrencyISO = m1.CurrencyISO
        };
    }
}
