using System;
using FinanceTracker.Core.Models;

namespace FinanceTrackerCore.Interfaces;

public interface ICurrencyHelper
{
    /// <summary>
    /// Checks if the iso code is a valid currency iso code
    /// </summary>
    /// <param name="isoCode">The iso code that will be checked</param>
    /// <returns>True if isoCode is an existing currency iso code. False otherwise.</returns>
    public bool IsValidISOCode(string isoCode);

    /// <summary>
    /// Returns the symbol of the currency's iso code
    /// </summary>
    /// <returns>The symbol of the currency's iso code</returns>
    public string ISOToSymbol(string isoCode);

    /// <summary>
    /// Returns a new money object converted into a ew currency while holding its value
    /// </summary>
    /// <param name="isoCode"></param>
    /// <returns>Converted money object with the new currency while holding it's value</returns>
    public Money ConvertToCurrency(Money money, string isoCode, DateTime date);
}
