using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Whistler.Common;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Houses;
using Whistler.MoneySystem.DTO.MenuDTO;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models.VehiclesData;

namespace Whistler.MoneySystem.DTO.MenuDTO
{
    class BankCreditDTO
    {
        public bool fastMoney { get; set; }
        public List<CreditDTO> creditsList { get; set; }
        public List<PropertyDTO> propertyList { get; set; }
        public BankCreditDTO(ExtPlayer player)
        {
            var character = player.Character;
            fastMoney = character.LVL >= 10;
            creditsList = CreditManager.GetCreditDTOs(player);
            propertyList = VehicleManager.GetAllHolderVehicles(character.UUID, OwnerType.Personal).Select(item => new PropertyDTO(item as PersonalBaseVehicle)).ToList();
            var house = HouseManager.GetHouse(character.UUID, OwnerType.Personal, true);
            if (house != null)
                propertyList.Add(new PropertyDTO(house));
            var houseFam = HouseManager.GetHouse(character.FamilyID, OwnerType.Family, true);
            if (houseFam != null)
                propertyList.Add(new PropertyDTO(houseFam));
            var family = player.GetFamily();
            if (family != null)
                propertyList.AddRange(VehicleManager.GetAllHolderVehicles(family.Id, OwnerType.Family).Select(item => new PropertyDTO(item as PersonalBaseVehicle)));
            var business = player.GetBusiness();
            if (business != null)
                propertyList.Add(new PropertyDTO(business));
        }
    }
}
