using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework12
{
    public class WeatherResponse
    {
        public Location location { get; set; }
        public CurrentWeather current { get; set; }
    }

    public class Location
    {
        public string name { get; set; }
        public string country { get; set; }
    }

    public class CurrentWeather
    {
        public float tempc { get; set; }
        public float windkph { get; set; }

    }
}
