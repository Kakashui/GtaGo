using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Jobs.Farm.Configs.Models
{
    class BonusConfig
    {
        public BonusConfig(double timeCoeff, int expIncr, int fetusIncr)
        {
            TimeCoeff = timeCoeff;
            ExpIncr = expIncr;
            FetusIncr = fetusIncr;
        }

        /// <summary>
        /// Коэффициент созревания, по умолчанию 0
        /// </summary>
        public double TimeCoeff { get; set; }

        /// <summary>
        /// Бонус опыта с 1 растения
        /// </summary>
        public int ExpIncr { get; set; }

        /// <summary>
        /// Бонус плодов с растения
        /// </summary>
        public int FetusIncr { get; set; }
    }
}
