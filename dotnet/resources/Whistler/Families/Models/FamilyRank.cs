using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Families.Models.DTO;
using Whistler.Houses;

namespace Whistler.Families.Models
{
    public class FamilyRank
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Dictionary<int, FamilyVehicleAccess> AccessVehicles { get; set; }
        public FamilyHouseAccess AccessHouse { get; set; }
        public FamilyFurnitureAccess AccessFurniture { get; set; }
        public bool AccessClothes { get; set; }
        public bool AccessBizWar { get; set; }
        public bool AccessMemberManagement { get; set; }
        public FamilyRank(int id)
        {
            ID = id;
            Name = "NewFag";
            AccessVehicles = new Dictionary<int, FamilyVehicleAccess>();
            AccessHouse = FamilyHouseAccess.EnterHouse;
            AccessFurniture = FamilyFurnitureAccess.None;
            AccessClothes = false;
            AccessBizWar = false;
            AccessMemberManagement = false;
        }

        internal string GetJsonDTO()
        {
            return JsonConvert.SerializeObject(new FamilyRankDto(this));
        }

        public void SetVehicleAccess(int key, int access)
        {
            if (!Enum.IsDefined(typeof(FamilyVehicleAccess), access))
                return;
            if (AccessVehicles.ContainsKey(key))
                AccessVehicles[key] = (FamilyVehicleAccess)access;
            else
                AccessVehicles.Add(key, (FamilyVehicleAccess)access);
        }

        public void DeleteVehicleAccess(int key)
        {
            if (AccessVehicles.ContainsKey(key))
                AccessVehicles.Remove(key);
        }
    }
}