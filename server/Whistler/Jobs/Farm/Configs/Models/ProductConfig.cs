using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Jobs.Farm.Configs.Models
{
    class ProductConfig
    {
        public string Type { get; set; }
        public string Parametr { get; set; }
        public string ParametrType { get; set; }
        public int Experience { get; set; }
        public int TimeBonus { get; set; }
        public int FetusBonus { get; set; }
        public FertilizerType FertilizerType { get; set; }
        public ProductConfig(string type, string parametr, string parametrType, FertilizerType fertilizerType)
        {
            Type = type;
            Parametr = parametr;
            ParametrType = parametrType;
            Experience = FarmConfigs.FertilizerConfigsList[fertilizerType].ExpIncr;
            TimeBonus = (int)(FarmConfigs.FertilizerConfigsList[fertilizerType].TimeCoeff * 100);
            FetusBonus = FarmConfigs.FertilizerConfigsList[fertilizerType].FetusIncr;
            FertilizerType = fertilizerType;
        }
    }
}
