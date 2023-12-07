using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Inventory.Configs.Models
{
    public class WeaponCharacteristics
    {
        public double FireRate { get; set; }
        public double Damage { get; set; }
        public double Accuracy { get; set; }

        public WeaponCharacteristics(double fireRate, double damage, double accuracy)
        {
            FireRate = fireRate;
            Damage = damage;
            Accuracy = accuracy;
        }
    }
}
