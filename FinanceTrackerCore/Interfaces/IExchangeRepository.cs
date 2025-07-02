using System;
using FinanceTrackerCore.Models;

namespace FinanceTrackerCore.Interfaces;

public interface IExchangeRepository
{
    public Task<CurrencyRateInfo?> GetCurrencyRates(DateTime date);
}
