using System.Threading.Tasks;
using FinanceTrackerCore.Helpers;
using FinanceTrackerCore.Models;
using FinanceTrackerCore.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Company.TestProject1;

[TestClass]
public class ExchangeRatesApiTests
{
    private readonly string _testingDir = @"./testing/";
    private readonly DateTime _testingDate = new DateTime(2024, 01, 01);
    private ExchangeRatesApiRepository _repo;

    [TestInitialize]
    public async Task Setup()
    {
        string? key = await EnvHelper.GetExchangeRatesKey();
        if (key is null)
        {
            Assert.Inconclusive("Api key was not found");
        }

        _repo = new(key, _testingDir);
    }

    [TestMethod]
    public async Task InitClassInvalidKeyTest()
    {
        // Setup
        ExchangeRatesApiRepository invalidRepo = new("Invalid key", _testingDir);
        _repo.ResetSaveDirectory();

        // Action and assert
        await Assert.ThrowsExceptionAsync<HttpRequestException>(() => invalidRepo.GetCurrencyRates(_testingDate));
    }

    [TestMethod]
    public async Task CachingCurrencyRateTest()
    {
        // Setup
        _repo.ResetSaveDirectory();

        // Action
        await _repo.GetCurrencyRates(_testingDate);

        // Assert
        Assert.IsTrue(_repo.CurrencyRatesIsCached(_testingDate));
    }

    [TestMethod]
    public async Task GetCurrencyRatesApiCallTest()
    {
        // Setup
        _repo.ResetSaveDirectory();

        // Action
        CurrencyRateInfo? info = await _repo.GetCurrencyRates(_testingDate);

        // Assert
        Assert.IsNotNull(info);
        Assert.IsTrue(info.Success);
        Assert.AreEqual<DateTime>(info.Date, _testingDate);
    }

    [TestMethod]
    public async Task GetCurrencyRatesFromFileTest()
    {
        // Setup
        if (!_repo.CurrencyRatesIsCached(_testingDate))
        {
            await _repo.GetCurrencyRates(_testingDate);
        }

        // Action
        Assert.IsTrue(_repo.CurrencyRatesIsCached(_testingDate)); // Make sure the currency rate is already cached
        CurrencyRateInfo? info = await _repo.GetCurrencyRates(_testingDate);

        // Assert
        Assert.IsNotNull(info);
        Assert.IsTrue(info.Success);
        Assert.AreEqual<DateTime>(info.Date, _testingDate);
    }
}
