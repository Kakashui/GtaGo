using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.NewDonateShop.Models;

namespace Whistler.NewDonateShop.Configs
{
    public class DonateConfig
    {
        [JsonProperty(PropertyName = "coinToVirtual")]
        public int CoinToVirtual { get; set; }
        [JsonProperty(PropertyName = "rubToCoin")]
        public int RubToCoin { get; set; }
        public string PaymentProvider { get; set; }
        public string PayUrl { get; set; }
        public string Database { get; set; }
        public string Secret { get; set; }
        public string Currency { get; set; }
        public string Language { get; set; }
        public int ProjectId { get; set; }
        public List<CoinKitConfigModel> CoinKits { get; set; }
    }
}
