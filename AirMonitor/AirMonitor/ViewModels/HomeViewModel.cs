using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AirMonitor.Global;
using AirMonitor.Models;
using AirMonitor.Services;
using AirMonitor.Views;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AirMonitor.ViewModels
{
    class HomeViewModel : BaseViewModel
    {
        private LocationService _locationService;
        private MeasurementsRepository _measurementsRepository;
        private INavigation _navigation;

        private ObservableCollection<Measurement> _measurements;
        private ObservableCollection<MapLocation> _locations;

        public ObservableCollection<Measurement> Measurements
        {
            get => _measurements;
            set => SetProperty(ref _measurements, value);
        }

        public ObservableCollection<MapLocation> Locations
        {
            get => _locations;
            set => SetProperty(ref _locations, value);
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private bool _isRefreshing;

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        private ICommand _refreshCommand;

        public ICommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new Command(async () => await Refresh()));


        private ICommand _navigateToDetailsCommand;

        public ICommand NavigateToDetailsCommand =>
            _navigateToDetailsCommand ?? (_navigateToDetailsCommand = new Command<Measurement>(NavigateToDetails));


        public HomeViewModel(INavigation navigation)
        {
            _locationService = new LocationService();
            _measurementsRepository =
                new MeasurementsRepository(new ApiService(Links.Api, App.ApiKey), App.DatabaseManager);
            _navigation = navigation;

            Initialize();
        }

        public Measurement GetItem(string address) => _measurements.FirstOrDefault(i => i.Installation.Address.DisplayAddress1.Equals(address));

        private ICommand _infoWindowClickedCommand;
        public ICommand InfoWindowClickedCommand => 
                  _infoWindowClickedCommand ?? (_infoWindowClickedCommand = new Command<Measurement>(NavigateToDetails)); 
        
                
            

        private async Task Initialize()
        {
            IsBusy = true;
            await LoadMeasurements();
            IsBusy = false;
        }

        private async Task LoadMeasurements(bool forceRefresh = false)
        {
            var location = await _locationService.GetLocation();
            var measurements = await _measurementsRepository.GetMeasurements(location, forceRefresh);
            Measurements = new ObservableCollection<Measurement>(measurements);
            Locations = new ObservableCollection<MapLocation>(Measurements.Select(i => new MapLocation { Address = i.Installation.Address.DisplayAddress1, Description = "CAQI: " + i.CurrentDisplayValue, Position = new Position(i.Installation.Location.Latitude, i.Installation.Location.Longitude) }));
        }

        private async Task Refresh()
        {
            IsRefreshing = true;
            await LoadMeasurements(true);
            IsRefreshing = false;
        }

        private void NavigateToDetails(Measurement measurement) =>
            _navigation.PushAsync(new DetailsPage(measurement));
    }
}