using AirMonitor.Extensions.Entities;
using AirMonitor.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace AirMonitor.Extensions
{
    public static class DatabaseExtensions
    {
        public static Installation ToInstallation(this InstallationEntity installationEntity)
        {
            return new Installation()
            {
                Id = installationEntity.Id,
                Location = JsonConvert.DeserializeObject<Coordinates>(installationEntity.LocationString),
                Address = JsonConvert.DeserializeObject<Address>(installationEntity.AddressString),
                Elevation = installationEntity.Elevation,
                IsAirlyInstallation = installationEntity.IsAirlyInstallation,
            };
        }
    }
}