using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AirMonitor.Views
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage() => InitializeComponent();

        private void Help_Clicked(object sender, EventArgs e) => 
            DisplayAlert("Co to jest CAQI?", "Lorem ipsum.", "Zamknij");
    }
}
