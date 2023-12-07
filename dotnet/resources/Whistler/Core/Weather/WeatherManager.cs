using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Whistler.Entities;
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

            if (Main.IsEnviromentWinter) NAPI.World.SetWeather(GTANetworkAPI.Weather.XMAS);
            else NAPI.World.SetWeather(Env_lastWeather);
        }

        private static WeatherModel GetCurrentWeather()
        {
            lock (_weathers)
            {
                WeatherModel currWeather = _weathers.Where(item => item.Date.AddHours(2) > DateTime.Now).FirstOrDefault();
                if (currWeather == null) return _weathers.LastOrDefault();
                return currWeather;
            }
        }

        public static void ChangeWeather(byte id)
        {
            try
            {
                if (WeatherConfigs.WeatherNames.ContainsKey(id)) Env_lastWeather = WeatherConfigs.WeatherNames[id];
                switch (id)
                {
                    case 13:
                        NAPI.World.SetWeather(GTANetworkAPI.Weather.XMAS);
                        break;
                    default:
                        //NAPI.World.SetWeather(GTANetworkAPI.Weather.CLEAR);
                        break;
                }
                NextWeatherChange = DateTime.Now.AddMinutes(30);
                SafeTrigger.ClientEventForAll("Enviroment_Weather", Env_lastWeather);

            }
            catch (Exception e) { _logger.WriteError($"ChangeWeather: {e.ToString()}"); }
        }

        private static void EnviromentChangeTrigger()
        {
            try
            {
                DateTime now = DateTime.Now;
                SafeTrigger.ClientEventForAll("Enviroment_Time", new List<int> { now.Hour, now.Minute });

                if (now.Day != Env_lastDate)
                {
                    Env_lastDate = now.Day;
                    SafeTrigger.ClientEventForAll("Enviroment_Date", new List<int>() { now.Day, now.Month, now.Year });
                }

                string newWeather = Env_lastWeather;
                if (now >= NextWeatherChange)
                {
                    WeatherModel weather = GetCurrentWeather();
                    newWeather = weather.Weather;
                    NextWeatherChange = now.AddMinutes(30);
                }
                
                if (newWeather != Env_lastWeather)
                {
                    Env_lastWeather = newWeather;
                    SafeTrigger.ClientEventForAll("Enviroment_Weather", newWeather);
                }
            }
            catch (Exception e) { _logger.WriteError($"enviromentChangeTrigger: {e.ToString()}"); }
        }

        public static void PlayerConnected(ExtPlayer player)
        {
            DateTime now = DateTime.Now;
            SafeTrigger.ClientEvent(player,"Enviroment_Start", new List<int>() { now.Hour, now.Minute }, new List<int>() { now.Day, now.Month, now.Year }, Env_lastWeather);
            player.TriggerCefEvent("smartphone/weatherPage/setFuture", JsonConvert.SerializeObject(_weathers));
        }
    }
}
