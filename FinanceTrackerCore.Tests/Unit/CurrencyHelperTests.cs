using FinanceTracker.Core.Models;
using FinanceTrackerCore.Helpers;
using FinanceTrackerCore.Tests.Dummies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Company.TestProject1;

[TestClass]
public class CurrencyHelperTests
{
    private CurrencyHelper? _currencyHelper = null;
    private readonly HashSet<string> _convertableCurrencyISOs = new()
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
    private readonly DateTime _testDate = new DateTime(2024, 01, 01);
    private const decimal eurToHuf = 382.522621M;

    [TestInitialize]
    public void Setup()
    {
        _currencyHelper = new(new DummyExchangeRepository());
    }

    [TestMethod]
    public void ValidISOCodeTest()
    {
        Assert.IsFalse(CurrencyHelper.IsValidISOCode("Invalid"));
        Assert.IsFalse(CurrencyHelper.IsValidISOCode("AAA"));

        foreach (var isoCode in _convertableCurrencyISOs)
        {
            Assert.IsTrue(CurrencyHelper.IsValidISOCode(isoCode));
        }

        Assert.IsTrue(CurrencyHelper.IsValidISOCode("aed"));
    }

    [TestMethod]
    public void ExchangeToNewCurrencyTest()
    {
        Money originalMoneyEUR = new Money()
        {
            Amount = 2.5M,
            CurrencyISO = "EUR"
        };

        Money exchangedMoneyHUF = new Money()
        {
            Amount = eurToHuf * 2.5M,
            CurrencyISO = "HUF"
        };

        Money? exchanged = _currencyHelper.ExchangeToNewCurrency(originalMoneyEUR, "huf", _testDate);
        Assert.IsNotNull(exchanged);

        Assert.AreEqual(exchanged.Amount, exchangedMoneyHUF.Amount);
        Assert.AreEqual(exchanged.CurrencyISO, exchangedMoneyHUF.CurrencyISO);

        Assert.ThrowsException<ArgumentException>(() => _currencyHelper.ExchangeToNewCurrency(
            new Money(),
            "Invalid ISO",
            _testDate
        ));
    }

    [TestMethod]
    public void AddMoneyTest()
    {
        Money m1 = new()
        {
            Amount = 15.63M,
            CurrencyISO = "EUR"
        };

        Money m2 = new()
        {
            Amount = 2,
            CurrencyISO = "EUR"
        };

        Money m3 = new()
        {
            Amount = 500,
            CurrencyISO = "HUF"
        };

        Money result12Expected = new Money()
        {
            Amount = 17.63M,
            CurrencyISO = "EUR"
        };

        Money result32Expected = new Money()
        {
            Amount = eurToHuf * 2 + 500,
            CurrencyISO = "HUF"
        };

        Money result12 = _currencyHelper.Add(m1, m2, _testDate);
        Money result32 = _currencyHelper.Add(m3, m2, _testDate);

        Assert.AreEqual(result12.Amount, result12Expected.Amount);
        Assert.AreEqual(result12.CurrencyISO, result12Expected.CurrencyISO);

        Assert.AreEqual(result32.Amount, result32Expected.Amount);
        Assert.AreEqual(result32.CurrencyISO, result32Expected.CurrencyISO);
    }

    [TestMethod]
    public void SubMoneyTest()
    {
        Money m1 = new()
        {
            Amount = 15.63M,
            CurrencyISO = "EUR"
        };

        Money m2 = new()
        {
            Amount = 2.1M,
            CurrencyISO = "EUR"
        };

        Money m3 = new()
        {
            Amount = 500,
            CurrencyISO = "HUF"
        };

        Money diff12Expected = new Money()
        {
            Amount = 13.53M,
            CurrencyISO = "EUR"
        };

        Money diff32Expected = new Money()
        {
            Amount = 500 - eurToHuf * 2.1M,
            CurrencyISO = "HUF"
        };

        Money diff12 = _currencyHelper.Sub(m1, m2, _testDate);
        Money diff32 = _currencyHelper.Sub(m3, m2, _testDate);

        Assert.AreEqual(diff12.Amount, diff12Expected.Amount);
        Assert.AreEqual(diff12.CurrencyISO, diff12Expected.CurrencyISO);

        Assert.AreEqual(diff32.Amount, diff32Expected.Amount);
        Assert.AreEqual(diff32.CurrencyISO, diff32Expected.CurrencyISO);
    }
}
