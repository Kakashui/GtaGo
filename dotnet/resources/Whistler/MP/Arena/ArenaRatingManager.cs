using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.MP.Arena.UI.DTO;
using Whistler.SDK;

namespace Whistler.MP
{
    internal class ArenaRatingManager : Script
    {
        public const int MinimalPointsToRating = 30;

        public const int RatingMembersCount = 20;
        
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(ArenaRatingManager));
        
        public static List<RatingModel> Ratings = new List<RatingModel>();
        
        public ArenaRatingManager()
        {
            Update();
            Main.Payday += Update;
        }

        // [Command("arenarating")]
        [RemoteEvent("arenarating")]
        public static void ShowRatingForPlayer(ExtPlayer player)
        {
            try
            {
                var mapper = MapperManager.Get();
                
                lock(Ratings)
                {
                    SafeTrigger.ClientEvent(player,"arena:sr",
                        JsonConvert.SerializeObject(mapper.Map<IEnumerable<ArenaRatingItemDTO>>(Ratings)));
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Error in {nameof(ArenaRatingManager)}.{nameof(ShowRatingForPlayer)}: {ex}");
            }
        }
       
        public static async void Update()
        {
            try
            {
                var response = await MySQL.QueryReadAsync(
                    "SELECT firstname, lastname, arena_points FROM characters WHERE arena_points >= @prop0 LIMIT @prop1",
                     MinimalPointsToRating, RatingMembersCount);
                
                var list = response.Rows
                    .Cast<DataRow>()
                    .Select(row => 
                        new RatingModel(row["firstname"].ToString(), 
                            row["lastname"].ToString(), 
                            Convert.ToInt32(row["arena_points"])))
                    .ToList();

                lock (Ratings)
                {
                    Ratings = list.OrderByDescending(m => m.Points).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Error in {nameof(ArenaRatingManager)}.{nameof(Update)}: {ex}");
            }
        }
    }

    internal class RatingModel
    {
        public string FullName { get; }
        public int Points { get; }
        
        public RatingModel(string firstname, string lastname, int points)
        {
            Points = points;
            FullName = $"{firstname}_{lastname}";
        }
    }
}