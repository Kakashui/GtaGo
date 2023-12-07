using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Core.Weather
{
    class WeatherModel
    {
        public DateTime Date { get; set; }
        public int WeatherType { get; set; }
        public string Weather { get; set; }
        public int Degrees { get; set; }
        public WeatherModel(DateTime date, int weatherType, string weather, int degrees)
        {
            Date = date;
            WeatherType = weatherType;
            Weather = weather;
            Degrees = degrees;
        }
    }
}
