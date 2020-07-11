using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirMonitor.Database;
using AirMonitor.Models;
using AirMonitor.Services;
using Xamarin.Essentials;

namespace AirMonitor
{
    public class MeasurementsRepository
    {
        private ApiService _apiService;
        private DatabaseManager _databaseManager;

        public MeasurementsRepository(ApiService apiService, DatabaseManager databaseManager)
        {
            _apiService = apiService;
            _databaseManager = databaseManager;
        }

        public async Task<List<Measurement>> GetMeasurements(Location location, bool forceRefresh = false)
        {
            var savedMeasurements = _databaseManager.GetMeasurements();

            if (forceRefresh || ShouldUpdate(savedMeasurements))
            {
                var installations = await _apiService.GetInstallationsFor(location);
                return await _apiService.GetMeasurementFor(installations);
            }

            return savedMeasurements;
        }

        private bool ShouldUpdate(List<Measurement> savedMeasurements)
        {
            if (savedMeasurements.Count == 0)
            {
                return true;
            }

            var isAnyMeasurementOld =
                savedMeasurements.Any(s => s.Current.TillDateTime.AddMinutes(60) < DateTime.UtcNow);
            return !savedMeasurements.Any() || isAnyMeasurementOld;
        }
    }
}