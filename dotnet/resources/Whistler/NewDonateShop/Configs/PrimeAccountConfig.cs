using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.NewDonateShop.Configs
{
    class PrimeAccountConfig
    {
        [JsonProperty(PropertyName = "days")]
        public int Days { get; } = 30;
        [JsonProperty(PropertyName = "bonusExp")]
        public int BonusExp { get; } = 1;
        [JsonProperty(PropertyName = "bonusPayDay")]
        public int BonusPayDay{ get; } = 0;
        [JsonProperty(PropertyName = "sellVehicleMulripler")]
        public float SellVehicleMultipler { get; } = .5f;
        [JsonProperty(PropertyName = "sellHouseMulripler")]
        public float SellHouseMulripler { get; } = .75f;
        [JsonProperty(PropertyName = "jobsPaymentsMultipler")]
        public float JobsPaymentsMultipler { get; } = 1.0f;
        [JsonProperty(PropertyName = "paymentForBusinessInDays")]
        public int PaymentForBusinessInDays { get; } = 30;
        [JsonProperty(PropertyName = "demorganAndArrestMultipler")]
        public float DemorganAndArrestMultipler { get; } = .5f;
    }
}
