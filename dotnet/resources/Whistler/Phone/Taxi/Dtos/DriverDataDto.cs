using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core.Character;
using Whistler.Phone.Taxi.Service;

namespace Whistler.Phone.Taxi.Dtos
{
    internal class DriverDataDto
    {
        public string Name { get; set; }

        public string Car { get; set; }

        public int Phone { get; set; }
        public DriverDataDto(Character character, DriverData driverData)
        {
            Name = character.FullName;
            Car = driverData.CarModel + " " + driverData.CarNumber;
            Phone = character.PhoneTemporary.Phone.SimCard.Number;
        }
    }
}
