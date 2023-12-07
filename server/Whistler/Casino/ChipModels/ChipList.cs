using System.Collections.Generic;
using System.Linq;

namespace ServerGo.Casino.ChipModels
{
    public class ChipList
    {
        public ChipList(IEnumerable<Chip> chips)
        {
            Chips = chips.ToList();
        }

        public ChipList()
        {
            Chips = new List<Chip>();
        }
        public List<Chip> Chips { get; set; }

        public void AddChipsToList(int multiplier)
        {
            var types = new List<ChipType>();
            foreach (var chip in Chips)
            {
                if (types.Contains(chip.ChipType)) continue;
                types.Add(chip.ChipType);
            }
            var chipsToAdd = new List<Chip>();
            foreach (var type in types)
            {
                var count = Chips.Count(x => x.ChipType == type);
                for (var i = 0; i < count * multiplier; i++)
                    chipsToAdd.Add(ChipFactory.Create(type));
            }

            Chips = chipsToAdd;
        }

        public void AddChipsToList(IEnumerable<Chip> chips)
        {
            foreach (var chip in chips)
                Chips.Add(chip);
        }
    }
}