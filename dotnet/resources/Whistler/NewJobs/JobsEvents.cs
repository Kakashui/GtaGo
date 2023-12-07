using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.SDK;

namespace Whistler.NewJobs
{
    class JobsEvents:Script
    {
       
        [ServerEvent(Event.ResourceStart)]
        public void OnStart()
        {          
            var query = $"CREATE TABLE IF NOT EXISTS `workers`(" +
                 $"`id` int(11) NOT NULL AUTO_INCREMENT," +
                 $"`uuid` int(11) NOT NULL," +
                 $"`exp` TEXT NOT NULL," +
                 $"PRIMARY KEY(`id`)" +
                 $")ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";
            MySQL.Query(query);
            Main.DatabaseSave += JobService.UpdateWorkers;
        }
    }
}
