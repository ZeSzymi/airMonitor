using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AirMonitor.Global;
using AirMonitor.Extensions;
using AirMonitor.Views;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AirMonitor
{
    public partial class App : Application
    {
        public static string ApiKey { get; private set; }
        
        public static DatabaseManager DatabaseManager { get; set; }
        public App()
        {
            InitializeComponent();
            LoadConfig();
            InitializeDatabase();
            MainPage = new RootTabbedPage();
        }
        
        private void InitializeDatabase()
        {
            if (DatabaseManager == null)
            {
                DatabaseManager = new DatabaseManager();
                DatabaseManager.Initialize();
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            DatabaseManager?.Dispose();
            DatabaseManager = null;
        }

        protected override void OnResume()
        {
        }
        
        private static async Task LoadConfig()
        {
            var assembly = Assembly.GetAssembly(typeof(App));
            var resourceNames = assembly.GetManifestResourceNames();
            var configName = resourceNames.FirstOrDefault(s => s.Contains("config.json"));
            
            using (var stream = assembly.GetManifestResourceStream(configName))
            {
                using (var reader = new StreamReader(stream))
                {
                    var json = await reader.ReadToEndAsync();
                    var dynamicJson = JObject.Parse(json);

                    ApiKey = dynamicJson["ApiKey"].Value<string>();
                    Links.Api = dynamicJson["ApiUrl"].Value<string>();
                    Links.Installations = dynamicJson["InstalationUrl"].Value<string>();
                    Links.Measurement = dynamicJson["MeasurmentUrl"].Value<string>();
                }
            }
        }
    }
}
