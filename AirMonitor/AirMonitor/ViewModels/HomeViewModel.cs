using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using AirMonitor.Const;
using AirMonitor.Models;
using AirMonitor.Services;
using AirMonitor.Views;
using Xamarin.Forms;

namespace AirMonitor.ViewModels
{
    class HomeViewModel : BaseViewModel
    {
        private LocationService _locationService;
        private MeasurementsRepository _measurementsRepository;
        private INavigation _navigation;

        private ObservableCollection<Measurement> _measurements;

        public ObservableCollection<Measurement> Measurements
        {
            get => _measurements;
            set => SetField(ref _measurements, value);
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetField(ref _isBusy, value);
        }

        private bool _isRefreshing;

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetField(ref _isRefreshing, value);
        }

        private ICommand _refreshCommand;

        public ICommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new Command( async () => await Refresh()));
        

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