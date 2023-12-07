using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Whistler.GUI.Documents.Enums;

namespace Whistler.GUI.Documents.Models
{
    public class License
    {
        public LicenseName Name { get; set; }
        public DateTime DateReceive { get; set; }
        public DateTime DateEnd { get; set; }
        [JsonIgnore]
        public bool IsActive 
        { 
            get
            {
                return DateTime.Now < DateEnd;
            }
        }
        public License(LicenseName name)
        {
            Name = name;
            DateReceive = DateTime.Now;
            DateEnd = DateTime.Now.AddDays(DocumentConfigs.GetLicenseDuration(name));
        }
        public License()
        {

        }
        public string GetSerialize()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings { DateFormatString = "yyyy-MM-dd" });
        }

        public void ToExtend(int days = 0)
        {
            DateEnd = DateTime.Now.AddDays(days == 0 ? DocumentConfigs.GetLicenseDuration(Name) : days);
        }
    }
}