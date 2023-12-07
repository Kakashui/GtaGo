using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Whistler.MiniGames.MakeWeapon
{
    public class WeaponsConfig
    {
        private List<WeaponModel> _weapons = new List<WeaponModel>
        {
            new WeaponModel(1, "Saiga", 250),
            new WeaponModel(2, "AKS", 260),
            new WeaponModel(3, "AK-19", 270),
            new WeaponModel(4, "KR-2", 280),
            new WeaponModel(5, "M16A4", 290),
            new WeaponModel(6, "AK-74", 300),
            new WeaponModel(7, "AK-103", 310),
            new WeaponModel(8, "Vepr", 320)
        };

        public WeaponModel this[int id]
        {
            get
            {
                return _weapons.FirstOrDefault(w => w.Id == id);
            }
        }
        public void Parse()
        {
            if (Directory.Exists("interfaces/gui/src/configs/gameMakeWeapon"))
            {
                using var w = new StreamWriter("interfaces/gui/src/configs/gameMakeWeapon/guns.js");
                w.Write($"export default {JsonConvert.SerializeObject(_weapons)}");
            }
        }
    }
}
