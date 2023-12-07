using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Fractions.Models;

namespace Whistler.Fractions.DTO
{
    class ArrestedModelDTO
    {
        public int id {get; set; }
        public string name {get; set; }
        public int passport {get; set; }
        public int sex {get; set; }
        public int number {get; set; }
        public int wantedLevel {get; set; }
        public string licenses {get; set; }
        public string arrestDate {get; set; }
        public string detained {get; set; }
        public string acticle {get; set; }
        public string strippedOfLicenses {get; set; }
        public bool isEdit {get; set; }
        public int bail {get; set; }
        public string bailOfficer {get; set; }
        public string releaseDate {get; set; }
        public ArrestedModelDTO(ArrestedModel arrestedModel)
        {
            id = arrestedModel.Id;
            name = Main.PlayerNames.GetValueOrDefault(arrestedModel.Uuid, "Unknown");
            passport = arrestedModel.Uuid;
            sex = arrestedModel.Gender ? 1 : 0;
            number = arrestedModel.NumberPhone;
            wantedLevel = arrestedModel.WantedLevel;
            licenses = arrestedModel.Licenses;
            arrestDate = arrestedModel.ArrestDate.ToString("F");
            detained = Main.PlayerNames.GetValueOrDefault(arrestedModel.DetainedUUID, "Unknown");
            acticle = arrestedModel.Reason;
            if (arrestedModel.StrippedLicenses.Count > 0)
                strippedOfLicenses = PDA.WantedSystem.GetLicenses(arrestedModel.StrippedLicenses);
            else
                strippedOfLicenses = null;
            isEdit = arrestedModel.CanBeIssue;
            bail = arrestedModel.BailAmount;
            bailOfficer = Main.PlayerNames.GetValueOrDefault(arrestedModel.BailPlayerUUID);
            releaseDate = arrestedModel.ArrestDate > arrestedModel.ReleaseDate ? null : arrestedModel.ReleaseDate.ToString("F");
        }
    }
}
