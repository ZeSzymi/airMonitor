using System.Runtime.CompilerServices;

namespace AirMonitor.Models
{
    public class Installation
    {
        public string Id { get; set; }
        public Coordinates Location { get; set; }
        public Address Address { get; set; }
        public double Elevation { get; set; }
        public bool IsAirlyInstallation { get; set; }
        public Measurement Measurement { get; set; }
    }
}