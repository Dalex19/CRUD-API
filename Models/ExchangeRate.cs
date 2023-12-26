public class ExchangeRate
{
    public int Id { get; set; }
    public string BaseCoin { get; set; }
    public string TargetCurrency { get; set; }
    public double Rate { get; set; }
    public DateTime Date { get; set; } 
}