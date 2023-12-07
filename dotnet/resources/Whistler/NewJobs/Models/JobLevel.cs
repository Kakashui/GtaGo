using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Whistler.NewJobs.Models
{
    public class JobLevel
    {
        public JobLevel(int id, int neededExpiriance, string displayName = null)
        {
            Id = id;
            NeededExpiriance = neededExpiriance;
            DisplayName = displayName;
        }
        public int Id { get; set; }
        public int NeededExpiriance { get; set; }
        public string DisplayName { get; set; }
        
    }
}
