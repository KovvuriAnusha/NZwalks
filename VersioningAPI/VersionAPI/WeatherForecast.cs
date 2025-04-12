namespace VersionAPI
{
    public class WeatherForecastV1
    {
        public DateOnly Date { get; set; }
        public int TemperatureC { get; set; }
        public string? Summary { get; set; }
    }

    public class WeatherForecastV2
    {
        public DateOnly Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF { get; set; }
        public string? WeatherSummary { get; set; }
    }
}
