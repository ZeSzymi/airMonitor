using AirMonitor.Database.Entities;
using AirMonitor.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace AirMonitor.Database
{
    public static class DatabaseUtils
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