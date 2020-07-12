using System.Collections.Generic;
using System.Linq;

namespace AirMonitor.Extension
{
    public static class UrlExtensions
    {
        public static string AddParameters(this string url, IDictionary<string, object> parameters) => 
            $"{url}?{parameters.ToQueryString()}";
        
        private static string ToQueryString(this IDictionary<string, object> parameters) => 
            string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"));
    }
}