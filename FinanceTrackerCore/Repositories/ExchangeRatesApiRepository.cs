using System.Data.Common;
using System.Runtime.InteropServices.Marshalling;
using System.Text.Json;
using FinanceTrackerCore.Interfaces;
using FinanceTrackerCore.Models;

namespace FinanceTrackerCore.Repositories;

public class ExchangeRatesApiRepository : IExchangeRepository
{
    private readonly string _saveDir = @"./saves/exchange_rates/";
    private readonly bool _saveDirCahnged = false;
    private static readonly string saveFilePostfix = "_currency.json";
    private readonly string _apiKey;

    /// <summary>
    /// Sets the api key for the calls. Can change the caching directory but only for testing. DO NOT set for prod
    /// </summary>
    /// <param name="apiKey"></param>
    /// <param name="saveDir"></param>
    public ExchangeRatesApiRepository(string apiKey, string? saveDir = null)
    {
        if (saveDir is not null) // Can cahnge the save dir for testing
        {
            _saveDir = saveDir;
            _saveDirCahnged = true;
        }

        CreateSaveDir();
        _apiKey = apiKey;
    }

    /// <summary>
    /// Returns a CurrencyRateInfo object. If the object did not exist the method will call an api and cache the result.
    /// </summary>
    /// <param name="date">The date of of the currency rates</param>
    /// <returns>CurrencyRateInfo object containing information about currency exchange</returns>
    public async Task<CurrencyRateInfo?> GetCurrencyRates(DateTime date)
    {
        if (CurrencyRatesIsCached(date))
        {
            return await ReadCurrencyDataFromJsonAsync(date);
        }

        CurrencyRateInfo? response = await GetCurrencyDataFromApiAsync(date);

        if (response != null)
        {
            await CacheCurrencyDataToJsonAsync(response, date);
        }

        return response;
    }

    /// <summary>
    /// Cehcks if the CurrencyRateInfo is already cached. If the infor is saved in a file it means it's cached.
    /// </summary>
    /// <param name="date">The date of the currency rates</param>
    /// <returns></returns>
    public bool CurrencyRatesIsCached(DateTime date)
    {
        return File.Exists(GetFileName(date));
    }

    /// <summary>
    /// Empties the save directory. Only usable if caching directory was cahgned (used for testing)
    /// </summary>
    public void ResetSaveDirectory()
    {
        if (!_saveDirCahnged) // Only if runs in a testing environment
        {
            return;
        }

        string[] filesInDir = Directory.GetFiles(_saveDir);
        foreach (string filePath in filesInDir)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    /// <summary>
    /// Creates save file name with the date specified in the parameters. 
    /// The file name is formated as "<save_directory>/<date_timestamp>_currency.json 
    /// where the 'date_timestamp' is the hash code of the date's Date property.
    /// </summary>
    /// <param name="date"></param>
    /// <returns>A string representing a save file name</returns>
    private string GetFileName(DateTime date)
    {
        return $"{_saveDir}{date.Date.GetHashCode()}{saveFilePostfix}";
    }

    /// <summary>
    /// Saves a currency info object in a json file
    /// </summary>
    /// <param name="info"></param>
    /// <param name="date"></param>
    /// <returns></returns>
    private async Task CacheCurrencyDataToJsonAsync(CurrencyRateInfo info, DateTime date)
    {
        string filePath = GetFileName(date);
        if (!File.Exists(filePath))
        {
            await using FileStream fs = File.Open(filePath, FileMode.OpenOrCreate);
            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
            await JsonSerializer.SerializeAsync<CurrencyRateInfo>(fs, info, options);
        }
    }

    /// <summary>
    /// Calls the api to get the currency rate info of a specific date then returns the info.
    /// </summary>
    /// <param name="date"></param>
    /// <returns>A CurrencyRateInfo if the method was succeds. null if not</returns>
    private async Task<CurrencyRateInfo?> GetCurrencyDataFromApiAsync(DateTime date)
    {
        if (date.Date > DateTime.Today)
        {
            return null;
        }

        string formatedDateString =
            (date.Date == DateTime.Today ?
            "latest" :
            date.ToShortDateString());

        using HttpClient client = new();

        var content = await client.GetStringAsync($"https://api.exchangeratesapi.io/v1/{formatedDateString}?access_key={_apiKey}");

        var doc = JsonDocument.Parse(content);
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        return doc.Deserialize<CurrencyRateInfo>(options);
    }

    /// <summary>
    /// Reads the currency rate info from a cached file.
    /// </summary>
    /// <param name="date"></param>
    /// <returns>A CurrencyRateInfo if the method was succeds. null if not</returns>
    private async Task<CurrencyRateInfo?> ReadCurrencyDataFromJsonAsync(DateTime date)
    {
        string filePath = GetFileName(date);

        if (!File.Exists(filePath))
        {
            return null;
        }

        await using FileStream fs = File.OpenRead(filePath);

        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
        return await JsonSerializer.DeserializeAsync<CurrencyRateInfo?>(fs, options);
    }

    private void CreateSaveDir()
    {
        if (Directory.Exists(_saveDir))
        {
            return;
        }

        Directory.CreateDirectory(_saveDir);
    }
}
