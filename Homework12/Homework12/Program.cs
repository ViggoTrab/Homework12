using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace Homework12
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            await GetPublicApiDataAsync();
        }

        public static async Task GetPublicApiDataAsync()
        {
            try
            {
                HttpClient client = new HttpClient();

                const string apiKey = "6c9325335c464251b44121921242512";
                const string city = "New York";
                string url = $"http://api.weatherapi.com/v1/current.json?key={apiKey}&q={city}&aqi=no";

                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                
                Console.WriteLine("Http response message:");
                Console.WriteLine(responseBody);

                DeserializeJsonDemo(responseBody);
                
                client.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        public static void DeserializeJsonDemo(string responseBody)
        {
            var weatherData = JsonSerializer.Deserialize<WeatherResponse>(responseBody);
            
            if (weatherData != null && weatherData.current != null)
            {
                Console.WriteLine();
                Console.WriteLine($"Weather Data for {weatherData.location.name}.");
                Console.WriteLine($"Temperature: {weatherData.current.tempc}°C.");
                Console.WriteLine($"Wind Speed: {weatherData.current.windkph} km/h.");
            }
            else
            {
                Console.WriteLine("Error when deserializing.");
            }
        }
    }
}





