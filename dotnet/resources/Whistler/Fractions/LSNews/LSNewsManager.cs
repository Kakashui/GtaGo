using System;
using System.Collections.Generic;
using GTANetworkAPI;
using Whistler.Helpers;
using Whistler.Fractions.LSNews;
using Whistler.Phone.News;
using Whistler.Fractions.LSNews.Models;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.SDK;

namespace Whistler.Fractions
{
    class LSNewsManager : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(LSNewsManager));

        private static Dictionary<int, AdvertStats> _statistics = null;


        public static List<Vector3> LSNewsCoords = new List<Vector3>
        {
            new Vector3(-1072.655, -246.5676, 53.506), // Колшэйп на крыше LSN
            new Vector3(-1078.168, -254.4076, 43.521), // Колшэйп изнутри интерьера для телепорта наверх
        };

        [RemoteEvent("lsnews::pressOpenMenu")]
        public void RemoteEvent_OpenLSNews(ExtPlayer player)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                if (player.Character.FractionID != 15)
                    return;
                if (!SubscribeToLSNews.IsSubscribe(player))
                {
                    AdvertHandler.LoadAdvertsForRedactor(player);
                    //LoadStatistics(player);
                    SubscribeToLSNews.Subscribe(player);
                }
                SafeTrigger.ClientEvent(player, "lsnews::open");
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() => _logger.WriteError($"RemoteEvent_OpenLSNews: {e.ToString()}"));
            }
        }

        [RemoteEvent("lsnews::compleateAdvert")]
        public void RemoteEvent_CompleateAdvert(ExtPlayer player, int id, string text, string image)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                if (player.Character.FractionID != 15)
                    return;
                AdvertHandler.UpdateAdvert(player, id, Domain.Phone.News.AdvertStatus.Compleate, text, image);
                //if (result)
                //    UpdateStats(player);
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() => _logger.WriteError($"RemoteEvent_CompleateAdvert: {e.ToString()}"));
            }
        }

        [RemoteEvent("lsnews::canceledAdvert")]
        public void RemoteEvent_CanceledAdvert(ExtPlayer player, int id)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                if (player.Character.FractionID != 15)
                    return;
                AdvertHandler.UpdateAdvert(player, id, Domain.Phone.News.AdvertStatus.Canceled, null, null);
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() => _logger.WriteError($"RemoteEvent_CanceledAdvert: {e.ToString()}"));
            }
        }

        public static void OnPlayerDisconnectedhandler(ExtPlayer player, DisconnectionType type, string reason)
        {
            SubscribeToLSNews.UnSubscribe(player);
        }

        public static void OnPlayerRemoveFromFraction(ExtPlayer player)
        {
            SubscribeToLSNews.UnSubscribe(player);
        }

        #region Statistics Coming Soon
        private static void UpdateStats(ExtPlayer player)
        {
            return;
            var stats = GetStatistics();
            int uuid = player.Character.UUID;
            if (stats.ContainsKey(uuid))
            {
                stats[uuid].CountForDay++;
                stats[uuid].CountForMount++;
            }
            else
                stats.Add(uuid, new AdvertStats
                {
                    UUID = uuid,
                    Name = player.Name,
                    CountForMount = 1,
                    CountForDay = 1
                });
            SubscribeToLSNews.TriggerCefEventSubscribers("", JsonConvert.SerializeObject(stats[uuid]));
        }

        private static void LoadStatistics(ExtPlayer player)
        {
            var stats = GetStatistics();
            SubscribeToLSNews.TriggerCefEventSubscribers("", JsonConvert.SerializeObject(stats));
        }

        private static Dictionary<int, AdvertStats> GetStatistics()
        {
            if (_statistics != null)
                return _statistics;
            _statistics = new Dictionary<int, AdvertStats>();
            var adverts = AdvertHandler.GetAdverts();
            foreach (var advert in adverts)
            {
                if (_statistics.ContainsKey(advert.EditorUUID))
                {
                    _statistics[advert.EditorUUID].CountForMount++;
                    _statistics[advert.EditorUUID].CountForDay += advert.DateCompleate.DayOfYear == DateTime.Now.DayOfYear ? 1 : 0;
                }
                else
                    _statistics.Add(advert.EditorUUID, new AdvertStats 
                    { 
                        UUID = advert.EditorUUID,
                        Name = Main.PlayerNames.GetValueOrDefault(advert.EditorUUID, "Unknown"),
                        CountForMount = 1,
                        CountForDay = advert.DateCompleate.DayOfYear == DateTime.Now.DayOfYear ? 1 : 0
                    });
            }
            return _statistics;
        }
        #endregion
    }
}
