using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.MP.OrgBattle.Models
{
    class KillerModel
    {
        public string Name { get; set; }
        public int UUID { get; set; }
        public int Kills { get; set; }
        public int FamilyId { get; set; }
        public KillerModel(string name, int uuid, int kills, int familyId)
        {
            Name = name;
            UUID = uuid;
            Kills = kills;
            FamilyId = familyId;
        }
        public void AddKill()
        {
            Kills++;
        }
    }
}
