namespace sgeltid.Modules.Energinet;

public record GetEnerginetPricesCommand();

public class GetEnerginetPrices
{
    public static async void Handle(GetEnerginetPricesCommand command)
    {
        var result = await GetEnergiDataSpotPrices();

        if (result == null)
        {
            // Handle the case where data retrieval failed
            return BadRequest("Failed to retrieve SpotPrices");
        }

        var current_time = DateTime.Now;
        var startTime = current_time.RoundDownToNearestHour();
        var endTime = startTime.AddHours(double.Parse(timeframe));

        var data = List<KwhPrice>();

        foreach (var record in result["records"])
        {
            var priceArea = (string)record["PriceArea"];
            var timestampStr = (string)record["HourUTC"];
            var timestamp = DateTime.Parse(timestampStr, CultureInfo.InvariantCulture) + TimeSpan.FromHours(1);
            var spotPriceDkk = (double)record["SpotPriceDKK"];
            var spotPriceKwh = spotPriceDkk / 1000.0;

            data.Add(new KwhPrice
            {
                PriceArea = priceArea,
                timestamp = timestamp,
                SpotPrice = spotPriceKwh
            };)
        }
    }

    private async Task<JsonDocument> GetEnergiDataSpotPrices()
    {
        try
        {
            var response = await _httpClient.GetAsync("https://api.energidataservice.dk/dataset/Elspotprices?limit=72");
            if (!response.IsSuccessStatusCode) { return null; }

            using var contentStream = await response.Content.ReadAsStreamAsync();
            return await JsonDocument.ParseAsync(contentStream);
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    private async Task<JsonDocument> GetEnergiDataNetTariffs()
    {
        try
        {
            var response = await _httpClient.GetAsync("https://api.energidataservice.dk/dataset/DatahubPricelist?filter=%7B%22chargetype%22:%22D03%22,%22gln_number%22:[%225790000610976%22],%22chargetypecode%22:%22TNT1009%22%7D&sort=ValidFrom%20desc");
            if (!response.IsSuccessStatusCode) { return null; }

            using var contentStream = await response.Content.ReadAsStreamAsync();
            return await JsonDocument.ParseAsync(contentStream);
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

}
