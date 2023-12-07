using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Families.Models;

namespace Whistler.GUI
{
    public class MaritalStatus
    {
        public MaritalStatus(bool gender, int partner)
        {
            this.sex = gender ? 1 : 0;
            if (partner < 0)
                this.secondHalf = null;
            else
            {
                if (Main.PlayerNames.ContainsKey(partner)) 
                    this.secondHalf = Main.PlayerNames[partner];
                else 
                    this.secondHalf = null;
            }
        }

        public int sex { get; set; }
        public string secondHalf { get; set; }
    }
    class StatsDTO
    {
        public int level { get; set; }
        public string username { get; set; }
        public int exp { get; set; }
        public string phoneNumber { get; set; }
        public string passportNumber { get; set; }
        public string licenses { get; set; }
        public string bankCount { get; set; }
        public string organization { get; set; }
        public int rank { get; set; }

        public string rankName { get; set; }
        public string work { get; set; }
        public MaritalStatus maritalStatus { get; set; }
        public int bans { get; set; }
        public int warns { get; set; }
        public DateTime registrationDate { get; set; }

        public string family { get; set; }
    }
}
