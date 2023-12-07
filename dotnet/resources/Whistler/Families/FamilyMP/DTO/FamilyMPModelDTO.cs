using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Families.FamilyMP.Models;
using Whistler.Helpers;

namespace Whistler.Families.FamilyMP.DTO
{
    class FamilyMPModelDTO
    {
        public int ID { get; set; }
        public int Date { get; set; }
        public int Location { get; set; }
        public string WinnerFamilyName { get; set; }
        public bool IsFinished { get; set; }
        public string Description { get; set; }
        public string Rewards { get; set; }
        public List<KillerModel> KillLog { get; set; }
        public FamilyMPModelDTO(FamilyMPModel familyMpModel)
        {
            ID = familyMpModel.ID;
            Date = familyMpModel.Date.ToUniversalTime().GetTotalSeconds(DateTimeKind.Utc);
            Location = (int)familyMpModel.Location;
            WinnerFamilyName = familyMpModel.WinnerFamilyName;
            IsFinished = familyMpModel.IsFinished;
            Description = familyMpModel.Description;
            Rewards = familyMpModel.Rewards;
            KillLog = familyMpModel.KillLog;
        }
    }
}
