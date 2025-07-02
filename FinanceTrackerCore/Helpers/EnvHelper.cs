using System;

namespace FinanceTrackerCore.Helpers;

public static class EnvHelper
{
    private static string? envPath = null;

    public static bool SetPath(string path)
    {
        if (!File.Exists(path))
        {
            return false;
        }

        envPath = path;
        return true;
    }

    public static async Task<string?> GetExchangeRatesKey()
    {
        if (envPath is null)
        {
            return null;
        }

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
