public class kWhPricesBackgroundService : IHostedService, IDisposable
{
    private string _baseurlSpotPrices = "https://api.energidataservice.dk/dataset/Elspotprices";
    private string _baseurlNetTariffs = "https://api.energidataservice.dk/dataset/DatahubPriceList";
    private Timer _timer;
    private readonly IServiceProvider _serviceProvider;

    public SpotPriceBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(GetSpotPrices, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));

        return Task.CompletedTask;
    }

    private async Task<JsonDocument> GetSpotPrices(object state)
    {
        // Fetch spot prices from api.energidataservice.dk/dataset/spotprices
        using var _httpClient = new HttpClient();
        var response = await _httpClient.GetAsync(_baseurlSpotPrices + "?limit=72");

        if (!response.IsSuccessStatusCode) { return null; }

        using var contentStream = await response.Content.ReadAsStreamAsync();
        return await JsonDocument.ParseAsync(contentStream);
    }

    private async Task<JsonDocument> GetNetworkTariffs(object state)
    {
        // Fetch net tariffs from api.energidataservice.dk/dataset/datahubpricelist
        using var _httpClient = new HttpClient();
        var response = await _httpClient.GetAsync(_baseurlNetTariffs);

        if (!response.IsSuccessStatusCode) { return null; }

        using var contentStream = await response.Content.ReadAsStreamAsync();
        return await JsonDocument.ParseAsync(contentStream);
    }

    private void ProcessAndSaveData(JsonDocument data, EnergyPricesDbContext dbContext)
    {
        // To get the total kWh price it is necessary to get data from to api calls.
        // 1. SpotPrices is the raw kWh prices
        // 2. Net tariffs is the price pr kWh covering transportation, network etc.
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<EnergyPricesDbContext>();

        }


        dbContext.EnergyPrices.Add(energyPrice);
        dbContext.SaveChanges();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}