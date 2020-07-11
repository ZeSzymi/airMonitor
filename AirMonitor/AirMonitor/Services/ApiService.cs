using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using AirMonitor.Const;
using AirMonitor.Models;
using AirMonitor.Utils;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms.Internals;

namespace AirMonitor.Services
{
    public class ApiService
    {
        private String _url;
        private String _apiKey;

        public ApiService(string url, string apiKey)
        {
            _url = url;
            _apiKey = apiKey;
        }

        public async Task<List<Installation>> GetInstallationsFor(Location location)
        {
            var parameters = new Dictionary<string, object>
            {
                {"lat", location.Latitude},
                {"lng", location.Longitude},
                {"maxDistanceKM", -1},
                {"maxResults", 10},
            };

            var url = Links.Api + Links.Installations;
            var final = url.AddParameters(parameters);

           var installations = await HttpGet<List<Installation>>(final);
           App.DatabaseManager.SaveInstallations(installations);

           return installations;
        }

        public async Task<List<Measurement>> GetMeasurementFor(IEnumerable<Installation> installations)
        {
            var measurements = new List<Measurement>();

            foreach (var installation in installations)
            {
                var parameters = new Dictionary<string, object>
                {
                    {"installationId", installation?.Id}
                };

                var url = Links.Api + Links.Measurement;
                var final = url.AddParameters(parameters);
                var response = await HttpGet<Measurement>(final);

                if (response != null)
                {
                    response.Installation = installation;
                    response.CurrentDisplayValue =
                        (int) Math.Round(response.Current?.Indexes?.FirstOrDefault()?.Value ?? 0);
                    measurements.Add(response);
                }
            }
            
            App.DatabaseManager.SaveMeasurements(measurements);

            return measurements;
        }

        private async Task<T> HttpGet<T>(string url)
        {
            try
            {
                var client = _httpClient();
                var response = await client.GetAsync(url);

                switch ((int) response.StatusCode)
                {
                    case 200:
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                        return result;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return default;
        }

        private HttpClient _httpClient()
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(_url)
            };

            client.DefaultRequestHeaders.Add("apikey", _apiKey);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
            return client;
        }
    }
}