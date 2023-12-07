using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Whistler.Domain.Phone.Contacts;
using Whistler.Domain.Phone.Messenger;

namespace Whistler.Domain.Phone
{
    [Table("phones")]
    public class Phone
    {
        [Key]
        public int CharacterUuid { get; set; }

        /// <summary>
        /// ID установленных приложений.
        /// </summary>
        public List<AppId> InstalledAppsIds { get; set; }

        public int? SimCardId { get; set; }
        public SimCard SimCard { get; set; }

        public int? AccountId { get; set; }
        public Account Account { get; set; }
    }

    public enum AppId
    {
        Invalid = 0,
        Weather = 1,
        Radio = 2,
        Forbes = 3,
        Bank = 4,
        TaxiGo = 5,
        TaxiJob = 6,
        AppStore = 7,
        News = 8,
        Camera = 9,
    }
}
