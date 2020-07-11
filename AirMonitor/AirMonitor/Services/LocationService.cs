using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AirMonitor.Services
{
    public class LocationService
    {
        public async Task<Location> GetLocation() =>
            await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));
        
    }
}