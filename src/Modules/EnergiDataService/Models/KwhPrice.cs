namespace sgeltid.Modules.Energinet.Domain;

public class KwhPrice()
{
    public int Id { get; set; }
    public string PriceArea { get; set; }
    public DateTime Timestamp { get; set; }
    public string Currency { get; set; }
    public double SpotPrice { get; set; }
    public double NetworkTariff { get; set; }
    public double SystemTariff { get; set; }
    public double Taxes { get; set; }
    public double Vat { get; set; }
    public double TotalPrice { get; set; }
}