using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Whistler.NewDonateShop.Configs;
using Whistler.NewDonateShop.Models;

namespace Whistler.ServerConfiguration
{
    public class ServerConfig
    {
        public MainConfig Main { get; set; }
        public MySQLConfig MySQL { get; set; }
        public TimersConfig Timers { get; set; }
        public MailServiceConfig MailService { get; set; }
        public JobsConfig Jobs { get; set; }
        public BonusConfig Bonus { get; set; }
        public QueryConfig Query { get; set; }
        public DonateConfig DonateConfig { get; set; }
        public static ServerConfig Load()
        {          
            using (var r = new StreamReader("settings.json"))
            {
                return JsonConvert.DeserializeObject<ServerConfig>(r.ReadToEnd());
            }      
        }
    }
}
