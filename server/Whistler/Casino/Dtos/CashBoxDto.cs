using System.Collections.Generic;
using ServerGo.Casino.ChipModels;

namespace Whistler.Casino.Dtos
{
    internal class CashBoxDto
    {
        public int Black { get; set; }
        public int Red { get; set; }
        public int Blue { get; set; }
        public int Green { get; set; }
        public int Yellow { get; set; }

        public int TotalCount => Black + Red + Blue + Green + Yellow;

        public List<Chip> CreateChipList()
        {
            var chips = new List<Chip>();
            for (var i = 0; i < Black; i++)
                chips.Add(new Black());
            for (var i = 0; i < Red; i++)
                chips.Add(new Red());
            for (var i = 0; i < Blue; i++)
                chips.Add(new Blue());
            for (var i = 0; i < Green; i++)
                chips.Add(new Green());
            for (var i = 0; i < Yellow; i++)
                chips.Add(new Yellow());

            return chips;
        }

        public static CashBoxDto CreateDto(IEnumerable<Chip> chips)
        {
            var dto = new CashBoxDto();
            foreach (var chip in chips)
            {
                switch (chip.ChipType)
                {
                    case ChipType.Blue:
                        dto.Blue++;
                        break;
                    case ChipType.Green:
                        dto.Green++;
                        break;
                    case ChipType.Yellow:
                        dto.Yellow++;
                        break;
                    case ChipType.Red:
                        dto.Red++;
                        break;
                    case ChipType.Black:
                        dto.Black++;
                        break;
                }
            }

            return dto;
        }
    }
}