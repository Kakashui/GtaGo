using System;
using System.Collections.Generic;
using System.Linq;
using Whistler.Core.Character;
using Newtonsoft.Json;
using ServerGo.Casino.ChipModels;
using ServerGo.Casino.Business;
using Whistler.SDK;

namespace ServerGo.Casino.Gamblers
{
    /// <summary>
    /// Gambler's bank 
    /// </summary>
    public class ChipBank
    {
        public int TotalValue => _totalValue;
        public int SoldValue => (int) (_totalValue * (1 - CasinoManager.CasinoShare));

        private int _totalValue;
        private Character _character;
        public List<Chip> Chips { get; set; }
        public ChipBank(Character character)
        {
            Chips = new List<Chip>();
            _character = character;
        }

        public ChipBank(Character character, IEnumerable<Chip> list)
        {
            Chips = new List<Chip>();
            _character = character;
            var totalCost = 0;
            foreach (var chip in list)
            {
                totalCost += chip.Value;
                Chips.Add(chip);
            }

            _totalValue += totalCost;
        }

        /// <summary>
        /// Supplements current chip bank,
        /// returns cost of chips
        /// </summary>
        public int Charge(IEnumerable<Chip> chips)
        {
            var totalCost = 0;
            foreach (var chip in chips)
            {
                totalCost += chip.Value;
                Chips.Add(chip);
                _character.CasinoChips[(int) chip.ChipType]++;
            }

            _totalValue += totalCost;
            return totalCost;
        }

        public int Check(IEnumerable<Chip> chips)
        {
            var totalCost = 0;
            foreach (var chip in chips)
            {
                totalCost += chip.Value;
            }
            return totalCost;
        }

        public void Withdraw(IEnumerable<Chip> chips)
        {
            var totalCost = 0;
            
            foreach (var chip in chips)
            {
                totalCost += chip.Value;
                Chips.Remove(Chips.First(x => x.ChipType == chip.ChipType));
                _character.CasinoChips[(int) chip.ChipType]--;
            }

            _totalValue -= totalCost;
        }

        /// <summary>
        /// Removes all player chips
        /// </summary>
        public void Reset()
        {
            Chips = new List<Chip>();
            _totalValue = 0;
            _character.CasinoChips = new int[5];
            MySQL.Query("UPDATE characters SET chips = @prop0 WHERE uuid = @prop1 ",JsonConvert.SerializeObject(_character.CasinoChips), _character.UUID);
        }

        /// <summary>
        /// Convert money to chips
        /// </summary>
        public int ConvertMoneyToChips(int bank)
        {
            var chips = new List<Chip>();
            int money = bank;
            for (var type = ChipType.Yellow; type > ChipType.Undefined; type --)
            {
                Chip chip = ChipFactory.Create(type);
                while (money >= chip.Value)
                {
                    chips.Add(chip);
                    money-=chip.Value;
                }
            }
            Charge(chips);
            return money;
        }

        public bool Verify(IEnumerable<Chip> chips)
        {
            var totalCost = 0;
            foreach (var chip in chips) totalCost += chip.Value;
            
            if (_totalValue - totalCost < 0) return false; 
            
            if (chips.Select(x => x.ChipType == ChipType.Black).Count()
                > Chips.Select(c => c.ChipType == ChipType.Black).Count()) return false;
            
            if (chips.Select(x => x.ChipType == ChipType.Red).Count()
                > Chips.Select(c => c.ChipType == ChipType.Red).Count()) return false;
            
            if (chips.Select(x => x.ChipType == ChipType.Blue).Count()
                > Chips.Select(c => c.ChipType == ChipType.Blue).Count()) return false;
            
            if (chips.Select(x => x.ChipType == ChipType.Green).Count()
                > Chips.Select(c => c.ChipType == ChipType.Green).Count()) return false;
            
            if (chips.Select(x => x.ChipType == ChipType.Yellow).Count()
                > Chips.Select(c => c.ChipType == ChipType.Yellow).Count()) return false;

            return true;
        }
    }
}