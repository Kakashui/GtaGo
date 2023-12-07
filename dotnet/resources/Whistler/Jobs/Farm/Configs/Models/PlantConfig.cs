using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Whistler.Inventory.Enums;
using Whistler.Jobs.Farm.Models;

namespace Whistler.Jobs.Farm.Configs.Models
{
    class PlantConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stages">Этапы созревания (время и модель)</param>
        /// <param name="secondBeforeWatering">Время на поливку и удобрение с момента посадки, бонусы не влияют</param>
        /// <param name="ripeningTime">Время созревания с момента посадки, без учета бонусов</param>
        /// <param name="witheringTime">Время увядания с момента созревания, бонусы не влияют</param>
        /// <param name="type">Тип плода</param>
        /// <param name="exp">Опыт за выращивание</param>
        /// <param name="fetus">Получаемый плод</param>
        /// <param name="countFetus">Количество получаемых плодов</param>
        /// <param name="name">Название семян</param>
        public PlantConfig(List<PlantStages> stages, int secondBeforeWatering, int ripeningTime, int witheringTime, PlantType type, int exp, ItemNames fetus, int countFetus, ItemNames name, int price)
        {
            Stages = stages;
            SecondBeforeWatering = secondBeforeWatering;
            RipeningTime = ripeningTime;
            WitheringTime = witheringTime;
            Type = type;
            Exp = exp;
            Fetus = fetus;
            CountFetus = countFetus;
            Name = name;
            Price = price;
        }

        /// <summary>
        /// Этапы созревания (время и модель)
        /// </summary>
        public List<PlantStages> Stages { get; set; }

        /// <summary>
        /// Время на поливку и удобрение с момента посадки, бонусы не влияют
        /// </summary>
        public int SecondBeforeWatering { get; set; }
        /// <summary>
        /// Время созревания с момента посадки, без учета бонусов
        /// </summary>
        public int RipeningTime { get; set; }
        /// <summary>
        /// Время увядания с момента созревания, бонусы не влияют
        /// </summary>
        public int WitheringTime  { get; set; }
        /// <summary>
        /// Тип плода
        /// </summary>
        public PlantType Type { get; set; }
        /// <summary>
        /// Опыт за выращивание
        /// </summary>
        public int Exp { get; set; }
        /// <summary>
        /// Получаемый плод
        /// </summary>
        public ItemNames Fetus { get; set; }
        /// <summary>
        /// Количество получаемого продукта
        /// </summary>
        public int CountFetus { get; set; }
        /// <summary>
        /// Название семян
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public ItemNames Name { get; set; }
        [JsonIgnore]
        public int Price { get; set; }
    }
}
