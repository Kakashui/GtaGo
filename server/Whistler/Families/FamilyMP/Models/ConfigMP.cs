using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Families.FamilyMP.Models
{
    class ConfigMP
    {
        public ConfigMP(string description, string reward, string nameMP)
        {
            Description = description;
            Reward = reward;
            NameMP = nameMP;
        }

        public string Description { get; set; }
        public string Reward { get; set; }
        public string NameMP { get; set; }

    }
}
