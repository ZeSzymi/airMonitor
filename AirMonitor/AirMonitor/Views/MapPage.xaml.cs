using AirMonitor.Models;
using AirMonitor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace AirMonitor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        private HomeViewModel _viewModel => BindingContext as HomeViewModel;
        public MapPage()
        {
            InitializeComponent();
            BindingContext = new HomeViewModel(Navigation);
        }

        private void Pin_InfoWindowClicked(object sender, PinClickedEventArgs e)
        {
            var item = _viewModel.GetItem((sender as Pin).Address);
            _viewModel.NavigateToDetailsCommand.Execute(item);
        }
    }
}