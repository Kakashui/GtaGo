using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.Fractions.PDA;
using Whistler.GUI.Documents.Models;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Fractions.Models
{
    class ArrestedModel
    {
        public int Id { get; set; }
        public int Uuid { get; set; }
        public int DetainedUUID { get; set; }
        public string Licenses { get; set; }
        public bool Gender { get; set; }
        public int NumberPhone { get; set; }
        public int WantedLevel { get; set; }
        public DateTime ArrestDate { get; set; } = DateTime.Now;
        public DateTime ReleaseDate { get; set; } = DateTime.Now;
        public List<License> StrippedLicenses { get; set; } = new List<License>();
        public bool CanBeIssue { get; set; }
        public int BailAmount { get; set; }
        public int BailPlayerUUID { get; set; }
        public string Reason { get; set; }
        public ArrestedModel(DataRow row)
        {
            Id = Convert.ToInt32(row["id"]);
            Uuid = Convert.ToInt32(row["uuid"]);
            DetainedUUID = Convert.ToInt32(row["detaineduuid"]);
            try
            {
                Licenses = WantedSystem.GetLicenses(JsonConvert.DeserializeObject<List<License>>(row["licenses"].ToString()));
            }
            catch (Exception)
            {
                Licenses = WantedSystem.GetLicenses(new List<License>());
            }
            Gender = Convert.ToBoolean(row["gender"]);
            NumberPhone = Convert.ToInt32(row["phone"]);
            WantedLevel = Convert.ToInt32(row["wantedlevel"]);
            ArrestDate = Convert.ToDateTime(row["arrestdate"]);
            ReleaseDate = Convert.ToDateTime(row["releasedate"]);
            try
            {
                StrippedLicenses = JsonConvert.DeserializeObject<List<License>>(row["strippedlicenses"].ToString());
            }
            catch (Exception)
            {
                StrippedLicenses = new List<License>();
            }
            CanBeIssue = Convert.ToBoolean(row["canbeissue"]);
            BailAmount = Convert.ToInt32(row["bailamount"]);
            BailPlayerUUID = Convert.ToInt32(row["bailplayeruuid"]);
            Reason = row["reason"].ToString();
        }
        public ArrestedModel(ExtPlayer player, ExtPlayer officer, string reason)
        {
            Uuid = player.Character.UUID;
            DetainedUUID = officer.Character.UUID;
            Licenses = WantedSystem.GetLicenses(player.Character.Licenses);
            Gender = player.GetGender();
            NumberPhone = player.GetPhone()?.Phone?.SimCard.Number ?? -1;
            WantedLevel = player.Character.WantedLVL.Level;
            ArrestDate = DateTime.Now;
            ReleaseDate = ArrestDate.AddDays(-1);
            StrippedLicenses = new List<License>();
            CanBeIssue = true;
            BailAmount = 0;
            BailPlayerUUID = -1;
            Reason = reason;
            var dataQuery = MySQL.QueryRead("INSERT INTO `arrests`(`uuid`, `detaineduuid`, `arrestdate`, `releasedate`, `strippedlicenses`, `canbeissue`, `bailamount`, `bailplayeruuid`, `reason`, `wantedlevel`, `phone`) " +
                "VALUES (@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6, @prop7, @prop8, @prop9, @prop10); SELECT @@identity;", 
                Uuid, DetainedUUID, MySQL.ConvertTime(ArrestDate), MySQL.ConvertTime(ReleaseDate), JsonConvert.SerializeObject(StrippedLicenses), CanBeIssue, BailAmount, BailPlayerUUID, Reason, WantedLevel, NumberPhone
                );
            Id = Convert.ToInt32(dataQuery.Rows[0][0]);
        }
        public DTO.ArrestedModelDTO GetArrestedModelDTO()
        {
            return new DTO.ArrestedModelDTO(this);
        }

        public void Release(int amount, int bailPlayerUuid)
        {
            BailAmount = amount;
            BailPlayerUUID = bailPlayerUuid;
            ReleaseDate = DateTime.Now;
            CanBeIssue = false;
            MySQL.Query("UPDATE `arrests` SET `releasedate` = @prop1, `bailamount` = @prop2, `bailplayeruuid` = @prop3, `canbeissue` = @prop4 WHERE `id` = @prop0", Id, MySQL.ConvertTime(ReleaseDate), BailAmount, BailPlayerUUID, CanBeIssue);
            PersonalDigitalAssistant.UpdateArrestData(this);
        }
        public bool BlockCanBeIssue(int officerUuid)
        {
            if (officerUuid != DetainedUUID)
                return false;
            CanBeIssue = false;
            MySQL.Query("UPDATE `arrests` SET `canbeissue` = @prop1 WHERE `id` = @prop0", Id, CanBeIssue);
            PersonalDigitalAssistant.UpdateArrestData(this);
            return true;
        }
    }
}
