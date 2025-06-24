using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinanceTrackerCore.Helpers;

public class ExchangeRatesApiSerializer
{
    private static readonly string saveDir = @"./saves/exchange_rates/";
    private static readonly string lastFetchDateFile = "last_fetch_date";
    private DateTime? _lastFetchDate = null;
    private CurrencyApiResponse _cachedApiResponse;
    private readonly string _apiKey;

    public ExchangeRatesApiSerializer(string apiKey)
    {
        _apiKey = apiKey;
    }

    private async Task LoadLastFetchDateAsync()
    {
        using StreamReader sr = new($"{saveDir}{lastFetchDateFile}");
        string dateString = await sr.ReadLineAsync();
        if (DateTime.TryParse(dateString, out var date))
        {
            _lastFetchDate = date;
        }
    }

    public async Task<CurrencyApiResponse> GetApiResponseAsync()
    {
        if (_lastFetchDate is null)
        {
            await LoadLastFetchDateAsync();
        }

        if (_lastFetchDate is null) // Loading _lastFetchDate Failed
        {
            throw new Exception("Loading last fetch date failed");
        }

        if (_lastFetchDate != DateTime.Today)
        {
            await UpdateLastFetchDateAsync();
        }

        if (_cachedApiResponse is null)
        {
            await UpdateCacheAsync();
        }

        if (_cachedApiResponse is null)
        {
            throw new Exception("Failed to update currency rates");
        }

        return _cachedApiResponse;
    }

    private async Task UpdateCacheAsync()
    {
        // Get result from API
        using HttpClient client = new();
        var httpResponse = await client.GetAsync($"https://api.exchangeratesapi.io/v1/latest?access_key={_apiKey}");
        var content = httpResponse.Content;

        // Parse it
        var apiResponse = await content.ReadFromJsonAsync<CurrencyApiResponse>();

        // Cahce it
        _cachedApiResponse = apiResponse;

        // Write it

    }

    private Task UpdateLastFetchDateAsync()
    {
        return Task.Run(async () =>
        {
            DateTime newDate = DateTime.Today;

            // Write it
            using StreamWriter sw = new($"{saveDir}{lastFetchDateFile}");
            await sw.WriteLineAsync(newDate.ToString());

            // Cache it
            _lastFetchDate = newDate;
        });
    }
    
    private class CurrencyApiResponse
    {
        public bool Success { get; set; }
        public int Timestamp { get; set; }
        public string Base { get; set; }
        public DateOnly Date { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
