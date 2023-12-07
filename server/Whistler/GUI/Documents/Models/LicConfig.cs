using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.GUI.Documents.Models
{
    class LicConfig
    {
        public int DayDuration { get; }
        public int Price { get; }
        public string Word { get; }
        public LicConfig(int dayDuration, int price, string word)
        {
            DayDuration = dayDuration;
            Price = price;
            Word = word;
        }
    }
}
