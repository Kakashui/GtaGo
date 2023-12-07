using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Whistler.Helpers;
using Whistler.SDK;
using Whistler.SDK.Models;

namespace Whistler.Core.Weather
{
    class WeatherManager : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(WeatherManager));

        public static Random rnd = new Random();
        private static int Env_lastDate = -1;
        private static DateTime NextWeatherChange = DateTime.Now;
        public static string Env_lastWeather = "CLEAR";

        private static string URL = "http://api.openweathermap.org/data/2.5/forecast";
        private static List<HttpQueryParam> Parametrs = new List<HttpQueryParam>
        {
            new HttpQueryParam("id", 5368361),
            new HttpQueryParam("units", "metric"),
            new HttpQueryParam("lang", "ru"),
            new HttpQueryParam("appid", "5e39dcad587339ea6650f4d19ab910a3"),
        };

        private static List<WeatherModel> _weathers = new List<WeatherModel>
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

        public static void WeatherInit()
        {
            Timers.StartTask("envTimer", 60000, EnviromentChangeTrigger);

            if (Main.IsEnviromentWinter)
                NAPI.World.SetWeather(GTANetworkAPI.Weather.XMAS);
            else
                NAPI.World.SetWeather(Env_lastWeather);
            LoadLosAngelesWeather();
        }

        private static WeatherModel GetCurrentWeather()
        {
            lock (_weathers)
            {
                var currWeather = _weathers.Where(item => item.Date.AddHours(2) > DateTime.Now).FirstOrDefault();
                if (currWeather == null)
                    return _weathers.LastOrDefault();
                return currWeather;
            }
        }


        public static void ChangeWeather(byte id)
        {
            try
            {
                if (WeatherConfigs.WeatherNames.ContainsKey(id))
                    Env_lastWeather = WeatherConfigs.WeatherNames[id];
                switch (id)
                {
                    case 15:
                        NAPI.World.SetWeather(GTANetworkAPI.Weather.XMAS);
                        return;
                    case 16:
                        NAPI.World.SetWeather(GTANetworkAPI.Weather.CLEAR);
                        return;
                }
                NextWeatherChange = DateTime.Now.AddMinutes(30);
                Main.ClientEventToAll("Enviroment_Weather", Env_lastWeather);

            }
            catch (Exception e) { _logger.WriteError($"ChangeWeather: {e.ToString()}"); }
        }
        private static void EnviromentChangeTrigger()
        {
            try
            {
                Main.ClientEventToAll("Enviroment_Time", new List<int> { DateTime.Now.Hour, DateTime.Now.Minute });

                if (DateTime.Now.Day != Env_lastDate)
                {
                    Env_lastDate = DateTime.Now.Day;
                    Main.ClientEventToAll("Enviroment_Date", new List<int>() { DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year });
                }

                var weather = GetCurrentWeather();

                string newWeather = Env_lastWeather;

                if (DateTime.Now >= NextWeatherChange)
                {
                    newWeather = weather.Weather;
                    NextWeatherChange = DateTime.Now.AddMinutes(5);
                }


                if (newWeather != Env_lastWeather)
                {
                    Env_lastWeather = newWeather;
                    Main.ClientEventToAll("Enviroment_Weather", newWeather);
                }
            }
            catch (Exception e) { _logger.WriteError($"enviromentChangeTrigger: {e.ToString()}"); }
        }

        private static async void LoadLosAngelesWeather()
        {
            try
            {
                string currentWeather = HttpQuery.GET(URL, Parametrs);
                var weatherObj = JObject.Parse(currentWeather);
                var list = weatherObj["list"].ToList();
                lock (_weathers)
                {
                    _weathers = new List<WeatherModel>();
                    foreach (var item in list)
                    {
                        var date = Convert.ToDateTime(item["dt_txt"].ToString());
                        var weatherID = Convert.ToInt32(item["weather"][0]["id"].ToString());
                        int weatherType = WeatherConfigs.GetWeatherByID(weatherID);
                        var weather = WeatherConfigs.WeatherNames.GetValueOrDefault(weatherType, "EXTRASUNNY");
                        var temp = Convert.ToInt32(item["main"]["temp"]);
                        _weathers.Add(new WeatherModel(date.AddHours(-8), weatherType, weather, temp));
                    }
                }
                WhistlerTask.Run(EnviromentChangeTrigger);
            }
            catch (Exception e)
            {
                _logger.WriteError($"LoadLosAngelesWeather: {e.ToString()}");
                _weathers = WeatherConfigs.DefaultWeather.ToList();
            }
        }

        public static void PlayerConnected(Player player)
        {
            player.TriggerEvent("Enviroment_Start", new List<int>() { DateTime.Now.Hour, DateTime.Now.Minute }, new List<int>() { DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year }, Env_lastWeather);
            player.TriggerCefEvent("smartphone/weatherPage/setFuture", JsonConvert.SerializeObject(_weathers));
        }
    }
}
