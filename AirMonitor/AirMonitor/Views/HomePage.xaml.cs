using AirMonitor.Models;
using AirMonitor.ViewModels;
using System;
using Xamarin.Forms;

namespace AirMonitor.Views
{
    public partial class HomePage : ContentPage
    {
        private HomeViewModel _viewModel => BindingContext as HomeViewModel;
        public HomePage()
        {
            InitializeComponent();
            BindingContext = new HomeViewModel(Navigation);
        }

        void ListView_ItemTapped(Object sender, ItemTappedEventArgs e)
        {
            _viewModel.NavigateToDetailsCommand.Execute(e.Item as Measurement);
        }
    }
}