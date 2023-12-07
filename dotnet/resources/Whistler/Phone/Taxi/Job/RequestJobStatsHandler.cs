using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.Phone.Taxi.Dtos;
using Whistler.SDK;

namespace Whistler.Phone.Taxi.Job
{
    internal class RequestJobStatsHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(RequestJobStatsHandler));

        [RemoteEvent("phone::taxijob::requestStats")]
        public async Task RequestStatsHandler(ExtPlayer player)
        {
            try
            {
                var character = player.Character;

                using (var context = DbManager.TemporaryContext)
                {
                    var driverOrdersQuery = context.TaxiOrdersHistory
                        .Where(o => o.DriverUuid == character.UUID);

                    var todayQuery = driverOrdersQuery
                        .Where(o => o.Date.Date == DateTime.Now.Date);
                    var totalSumForDay = await todayQuery.SumAsync(o => o.TotalPrice);
                    var totalTripsForDay = await todayQuery.CountAsync();

                    var monthQuery = driverOrdersQuery
                        .Where(o => o.Date.Month == DateTime.Now.Month);
                    var totalSumForMonth = await monthQuery.SumAsync(o => o.TotalPrice);
                    var totalTripsForMonth = await monthQuery.CountAsync();

                    var totalSum = await driverOrdersQuery.SumAsync(o => o.TotalPrice);
                    var totalTrips = await driverOrdersQuery.CountAsync();

                    var dto = new TaxiJobStatsDto
                    {
                        TotalSumForDay = totalSumForDay,
                        TotalTripsForDay = totalTripsForDay,
                        TotalSumForMonth = totalSumForMonth,
                        TotalTripsForMonth = totalTripsForMonth,
                        TotalSum = totalSum,
                        TotalTrips = totalTrips
                    };
                    player.TriggerCefEvent("smartphone/taxiPage/taxijob_setStats", JsonConvert.SerializeObject(dto));
                }
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() =>
                {
                    _logger.WriteError($"Unhandled exception catched on phone::taxijob::requestStats ({player?.Name}) - " + e.ToString());
                });
            }
        }
    }
}
