using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Families.FamilyMP.Models
{
    public abstract class FamilyMPModel
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public BattleLocation Location { get; set; }
        public int WinnerFamily { get; set; }
        public string WinnerFamilyName { get; set; }
        public bool IsFinished { get; set; }
        public string Description { get; set; }
        public string Rewards { get; set; }
        public List<KillerModel> KillLog { get; set; }
        public bool IsPlaying { get; set; }
        public FamilyMPType Type { get; set; }
        public string _timer { get; set; }
        public string NameMP 
        { 
            get
            {
                return Configs.ConfigMPList.GetValueOrDefault(Type)?.NameMP ?? "";
            }
        }
        public KillerModel AddKill(int uuid, string name, int familyId)
        {
            var killer = KillLog.FirstOrDefault(item => item.UUID == uuid);
            if (killer != null)
                killer.AddKill();
            else
            {
                killer = new KillerModel(name, uuid, 1, familyId);
                KillLog.Add(killer);
            }
            return killer;
        }

        public void Save()
        {
            MySQL.Query("UPDATE `familymp` SET " +
                "`winner` = @prop1, " +
                "`finished` = @prop2, " +
                "`kills` = @prop3 " +
                "WHERE `id` = @prop0",
                ID,
                WinnerFamily,
                IsFinished,
                JsonConvert.SerializeObject(KillLog));
        }

        public FamilyMPModel(DataRow row)
        {
            ID = Convert.ToInt32(row["id"]);
            Date = Convert.ToDateTime(row["date"]);
            Location = (BattleLocation)Convert.ToInt32(row["location"]);
            WinnerFamily = Convert.ToInt32(row["winner"]);
            WinnerFamilyName = FamilyManager.GetFamilyName(WinnerFamily);
            IsFinished = Convert.ToBoolean(row["finished"]);
            Type = (FamilyMPType)Convert.ToInt32(row["type"]);
            KillLog = JsonConvert.DeserializeObject<List<KillerModel>>(row["kills"].ToString());
            
            Description = Configs.ConfigMPList[Type].Description;
            Rewards = Configs.ConfigMPList[Type].Reward;
            IsPlaying = false;
            _timer = null;

            if (Date < DateTime.Now)
            {
                IsFinished = true;
            }
        }
        public FamilyMPModel(DateTime date, BattleLocation location, FamilyMPType type)
        {
            Date = date;
            Location = location;
            WinnerFamily = 0;
            WinnerFamilyName = FamilyManager.GetFamilyName(WinnerFamily);
            IsFinished = false;
            Type = type;
            KillLog = new List<KillerModel>();
            Description = Configs.ConfigMPList[Type].Description;
            Rewards = Configs.ConfigMPList[Type].Reward;
            IsPlaying = false;
            _timer = null;
        }
        public FamilyMPModel()
        {

        }
        public abstract bool IsMember(ExtPlayer player);
        public abstract bool TryStartMP();
        public abstract void TryEndMP();
        public abstract bool PlayerDeath(ExtPlayer player, ExtPlayer killer, uint weapon);

    }
}
