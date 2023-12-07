using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Core.Weather
{
    class WeatherConfigs
    {

        public static Dictionary<int, string> WeatherNames = new Dictionary<int, string>
        {
            { 0,  "EXTRASUNNY" },//Солнечно+
            { 1,  "CLEAR" },//Ясно+
            { 2,  "CLOUDS" },//Облачно+
            { 3,  "SMOG" },//Смог+
            { 4,  "FOGGY" },//Туман+
            { 5,  "OVERCAST" },//Пасмурно
            { 6,  "RAIN" },//Дождь+
            { 7,  "THUNDER" },//Гроза+
            { 8,  "CLEARING" },//Легкий дождь+
            { 9,  "SMOG" },//+
            { 10, "SNOW" },//снег+
            { 11, "SNOWLIGHT" },//легкий снег+
            { 12, "BLIZZARD" },//метель+
            { 13, "XMAS" },//с туманом+
            { 14, "HALLOWEEN" },//с туманом+
        };

        public static int GetWeatherByID(int id)
        {
            if (id >= 200 && id <= 232)
                return 7;
            if (id >= 300 && id <= 331)
                return 8;
            if (id >= 500 && id <= 531)
                return 6;
            if (id >= 700 && id < 721)
                return 3;
            if (id >= 722 && id < 800)
                return 4;
            switch (id)
            {
                case 600:
                    return 11;
                case 601:
                case 602:
                    return 10;
                case 611:
                case 612:
                case 613:
                case 615:
                case 616:
                    return 12;
                case 620:
                case 621:
                case 622:
                    return 13;
                case 800:
                    return 0;
                case 801:
                    return 1;
                case 802:
                case 803:
                    return 2;
                case 804:
                    return 5;
            }
            return 0;
        }
        public static List<WeatherModel> DefaultWeather = new List<WeatherModel>
        {
            new WeatherModel(DateTime.Now, 0, "EXTRASUNNY", 25 ),
            new WeatherModel(DateTime.Now.AddHours(3), 0, "EXTRASUNNY", 25 ),
            new WeatherModel(DateTime.Now.AddHours(6), 0, "EXTRASUNNY", 25 ),
            new WeatherModel(DateTime.Now.AddHours(9), 0, "EXTRASUNNY", 25 ),
            new WeatherModel(DateTime.Now.AddHours(12), 0, "EXTRASUNNY", 25 ),
            new WeatherModel(DateTime.Now.AddHours(15), 0, "EXTRASUNNY", 25 ),
            new WeatherModel(DateTime.Now.AddHours(18), 0, "EXTRASUNNY", 25 ),
            new WeatherModel(DateTime.Now.AddHours(21), 0, "EXTRASUNNY", 25 ),
            new WeatherModel(DateTime.Now.AddHours(24), 0, "EXTRASUNNY", 25 ),
            new WeatherModel(DateTime.Now.AddHours(27), 0, "EXTRASUNNY", 25 ),
            new WeatherModel(DateTime.Now.AddHours(30), 0, "EXTRASUNNY", 25 ),
            new WeatherModel(DateTime.Now.AddHours(33), 0, "EXTRASUNNY", 25 ),
            new WeatherModel(DateTime.Now.AddHours(36), 0, "EXTRASUNNY", 25 ),
            new WeatherModel(DateTime.Now.AddHours(39), 0, "EXTRASUNNY", 25 ),
            new WeatherModel(DateTime.Now.AddHours(42), 0, "EXTRASUNNY", 25 ),
            new WeatherModel(DateTime.Now.AddHours(45), 0, "EXTRASUNNY", 25 ),
            new WeatherModel(DateTime.Now.AddHours(48), 0, "EXTRASUNNY", 25 ),
            new WeatherModel(DateTime.Now.AddHours(51), 0, "EXTRASUNNY", 25 ),
        };
    }
}