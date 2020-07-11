using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AirMonitor.Services
{
    public class LocationService
    {
        public async Task<Location> GetLocation()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            return await Geolocation.GetLocationAsync(request);
        }
    }
}