using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Whistler.Families.Models.DTO
{
    class FamilyRankDto
    {
        public int rankId { get; }
        public string rankName { get; }
        public int accessHouse { get; }
        public int accessFurniture { get; }
        public bool accessClothes { get; }
        public bool accessWar { get; }
        public bool accessMembers { get; }
        public List<VehicleAccessDTO> accessCars { get; }
        public FamilyRankDto(FamilyRank rank)
        {
            rankId = rank.ID;
            rankName = rank.Name;
            accessHouse = (int)rank.AccessHouse;
            accessFurniture = (int)rank.AccessFurniture;
            accessClothes = rank.AccessClothes;
            accessWar = rank.AccessBizWar;
            accessMembers = rank.AccessMemberManagement;
            accessCars = rank.AccessVehicles.Select(item => new VehicleAccessDTO(item.Key, (int)item.Value)).ToList();
        }
    }
}
