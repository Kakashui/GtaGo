using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Models
{
    class CostumeModel
    {
        private static Dictionary<bool, Dictionary<CostumeClothesSlots, CostumeElement>> BaseClothes = new Dictionary<bool, Dictionary<CostumeClothesSlots, CostumeElement>>
        {
            {
                true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Torsos, new CostumeElement(15, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(61, 0) },
                    { CostumeClothesSlots.Bags, new CostumeElement(0, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(34, 0) },
                    { CostumeClothesSlots.Accessories, new CostumeElement(0, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(15, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(0, 0) },
                    { CostumeClothesSlots.Decals, new CostumeElement(0, 0) },
                    { CostumeClothesSlots.Top, new CostumeElement(15, 0) },
                }
            },
            {
                false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Torsos, new CostumeElement(15, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(56, 0) },
                    { CostumeClothesSlots.Bags, new CostumeElement(0, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(35, 0) },
                    { CostumeClothesSlots.Accessories, new CostumeElement(0, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(6, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(0, 0) },
                    { CostumeClothesSlots.Decals, new CostumeElement(0, 0) },
                    { CostumeClothesSlots.Top, new CostumeElement(101, 0) },
                }
            }
        };
        public bool Gender { get; set; }
        /// <summary>
        /// eqiped with costume
        /// </summary>
        public int TypeArmor { get; set; } = -1;

        [JsonIgnore]
        public Dictionary<CostumeClothesSlots, CostumeElement> Clothes { get; set; }
        [JsonIgnore]
        public Dictionary<CostumePropsSlots, CostumeElement> Props { get; set; }
        public Dictionary<int, CostumeElement> ClothesDto
        {
            get
            {
                Dictionary<int, CostumeElement> result = new Dictionary<int, CostumeElement>();
                foreach (var item in Clothes)
                {
                    result.Add((int)item.Key, item.Value);

                }
                return result;
            }
        }
        public Dictionary<int, CostumeElement> PropsDto
        {
            get
            {
                Dictionary<int, CostumeElement> result = new Dictionary<int, CostumeElement>();
                foreach (var item in Props)
                {
                    result.Add((int)item.Key, item.Value);

                }
                return result;
            }
        }
        public CostumeModel(bool gender, Dictionary<CostumeClothesSlots, CostumeElement> clothes, Dictionary<CostumePropsSlots, CostumeElement> props, int typeArmor = -1)
        {
            Gender = gender;
            Clothes = clothes;
            Props = props;
            TypeArmor = typeArmor;
        }
        public CostumeElement GetSlotClothes(CostumeClothesSlots slot)
        {
            if (Clothes.ContainsKey(slot))
                return Clothes[slot];
            else
                return BaseClothes[Gender][slot];
        }
        public CostumeElement GetSlotProp(CostumePropsSlots slot)
        {
            if (Props.ContainsKey(slot))
                return Props[slot];
            else
                return new CostumeElement(-1, 0);
        }
    }
}
