using System.Net.Http.Json;

namespace Client.Apis
{
    public class ApiClient
    {
        const string Address = "https://localhost:8082";
       
        private readonly HttpClient _client;

        public ApiClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecastListAsync()
        {
            var response = await _client.GetAsync(Address + "/WeatherForecast");
            return await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>()
                ?? throw new NullReferenceException();
        }
    }

    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}
