namespace Play.MerchantPortal.Api
{
    public class WeatherForecast
    {
        #region Instance Values

        public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public string? Summary { get; set; }

        #endregion
    }
}