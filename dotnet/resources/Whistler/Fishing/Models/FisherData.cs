using GTANetworkAPI;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Whistler.Entities;

namespace Whistler.Fishing.Models
{
    public class FisherData
    {
        public FisherData(ExtPlayer client)
        {
            try
            {
                var query = $"SELECT * FROM `fisher_data` WHERE `socialname`=@prop0;";
                var resp = MySQL.QueryRead(query, client.SocialClubName);
                if (resp.Rows.Count == 0)
                {
                    Lvl = 1;
                    Exp = 0;
                    License = false;
                    MapExpires = DateTime.Now;
                    try
                    {
                        query = "INSERT INTO `fisher_data` (`lvl`, `exp`, `map_expires`, `license`, `socialname`) VALUES(@prop0, @prop1, @prop2, 0, @prop3); SELECT LAST_INSERT_ID();";
                        var sub_resp = MySQL.QueryRead(query, Lvl, Exp, MySQL.ConvertTime(DateTime.Now), client.SocialClubName);
                        Id = Convert.ToInt32(sub_resp.Rows[0][0]);
                    }
                    catch (Exception ex)
                    {
                        NAPI.Util.ConsoleOutput($"error insert fisher_data: {ex.ToString()}");
                    }
                }
                else
                {
                    foreach (DataRow row in resp.Rows)
                    {
                        Id = (int)row["id"];
                        Lvl = (int)row["lvl"];
                        Exp = (int)row["exp"];
                        License = (int)row["license"] > 0;
                        MapExpires = (DateTime)row["map_expires"];
                    }
                }
                LastSeaSpotsUpdate = DateTime.Now;
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"error select fisher_data: {ex.ToString()}");
            }
            MiniGame = new MiniGame(client);
        }

        public int Id { get; set; }
        public int Lvl { get; set; }
        public int Exp { get; set; }
        public bool License { get; set; }
        public MiniGame MiniGame { get; set; }
        public DateTime MapExpires { get; set; }
        public DateTime LastSeaSpotsUpdate { get; set; }

        internal void AddMap(int hours)
        {
            try
            {
                MapExpires = DateTime.Now.AddHours(hours);
                var query = $"UPDATE `fisher_data` SET `map_expires`='{MySQL.ConvertTime(MapExpires)}' WHERE `id`={Id};";
                MySQL.Query(query);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"error AddMap fisher_data: {ex.ToString()}");
            }
        }
        internal bool IsMapExpired()
        {
            return MapExpires < DateTime.Now;
        }
        internal bool AddExp()
        {
            try
            {
                if (Lvl >= Const.MAX_FISHER_LVL) return false;
                Exp++;
                var ret = false;
                var query = $"UPDATE `fisher_data` SET `exp`={Exp} WHERE `id`={Id}";
                if (Exp >= Const.EXP_ON_LVL[Lvl])
                {
                    Exp = 0;
                    Lvl++;
                    query = $"UPDATE `fisher_data` SET `exp`={Exp}, `lvl`={Lvl} WHERE `id`={Id}";
                    ret = true;
                }
                MySQL.Query(query);
                return ret;
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"error AddExp fisher_data: {ex.ToString()}");
                return false;
            }
        }
    }
}
