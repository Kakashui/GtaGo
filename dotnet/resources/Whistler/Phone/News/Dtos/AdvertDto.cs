using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Domain.Phone.News;
using Whistler.Helpers;

namespace Whistler.Phone.News.Dtos
{
    class AdvertDto
    {
        public int Id { get; set; }
        public int SenderUUID { get; set; }
        public string Sender { get; set; }
        public int EditorUUID { get; set; }
        public string Editor { get; set; }
        public int Simnum { get; set; }
        public string MessengerLogin { get; set; }
        public string Title { get; set; }
        public string Picture { get; set; }
        public int DateCreate { get; set; }
        public int DateCompleate { get; set; }
        public bool PrimeAdvert { get; set; }
        public string Status { get; set; }

        public AdvertDto(Advert advert)
        {
            Id = advert.Id;
            SenderUUID = advert.SenderUUID;
            Sender = Main.PlayerNames.GetValueOrDefault(SenderUUID, "Unknown");
            EditorUUID = advert.EditorUUID;
            Editor = Main.PlayerNames.GetValueOrDefault(EditorUUID, "Unknown");
            Simnum = advert.PhoneNumber;
            MessengerLogin = advert.MessengerLogin;
            Title = advert.Text;
            Picture = advert.ImageUrl;
            DateCreate = advert.DateCreate.GetTotalSeconds();
            DateCompleate = advert.DateCompleate.GetTotalSeconds();
            PrimeAdvert = advert.PrimeAdvert;
            Status = advert.Status.ToString();
        }
    }
}
