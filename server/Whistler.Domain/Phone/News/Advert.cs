using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Whistler.Domain.Phone.News
{
    [Table("phones_news_advert")]
    public class Advert
    {
        [Key]
        public int Id { get; set; }
        public int SenderUUID { get; set; }
        public int EditorUUID { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateCompleate { get; set; }
        public int PhoneNumber { get; set; }
        public string MessengerLogin { get; set; }
        public string Text { get; set; }
        public bool PrimeAdvert { get; set; }
        public string ImageUrl { get; set; }
        public AdvertStatus Status { get; set; }

    }
}
