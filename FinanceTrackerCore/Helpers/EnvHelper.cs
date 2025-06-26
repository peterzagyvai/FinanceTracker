using System;

namespace FinanceTrackerCore.Helpers;

public static class EnvHelper
{
    private static string envPath = @"D:/Projects/FinanceTracker/FinanceTrackerCore/.env"; //FIXME: Delete this priavte static field

    public static async Task<string?> GetExchangeRatesKey()
    {
        using FileStream fs = File.OpenRead(envPath);
        using StreamReader sr = new(fs);
        string? line = null;
        while ((line = await sr.ReadLineAsync()) is not null)
        {
            if (line.Contains("exhange_rates"))
            {
                return line.Split(":")[1].Trim();
            }
        }

        return null;
    }
}
