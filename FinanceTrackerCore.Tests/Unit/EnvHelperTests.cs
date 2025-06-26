using System.Threading.Tasks;
using FinanceTrackerCore.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Company.TestProject1;

[TestClass]
public class EnvHelperTests
{
    [TestMethod]
    public async Task GetExchangeRatesKeyTest()
    {
        string? key = await EnvHelper.GetExchangeRatesKey();
        Assert.IsNotNull(key);
    }
}
