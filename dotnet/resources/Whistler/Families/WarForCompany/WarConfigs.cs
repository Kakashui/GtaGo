using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Whistler.Families.WarForCompany.Models;

namespace Whistler.Families.WarForCompany
{
    class WarConfigs
    {
        public const int SECONDS_FOR_CAPT_COMPANY = 300;
        public static readonly Dictionary<WarCompanies, CompanyConfig> CompaniesConfig = new Dictionary<WarCompanies, CompanyConfig>
        {
            {WarCompanies.DavisQuartz, new CompanyConfig("Davis Quartz", "davisquartz") },
            {WarCompanies.TheCayoPerico, new CompanyConfig("The Cayo Perico", "thecayoperico") },
            {WarCompanies.ElBurroHeights, new CompanyConfig("El Burro Heights", "elburroheights") },
            {WarCompanies.Sawmill, new CompanyConfig("Sawmill", "sawmill") },
            {WarCompanies.MurrietaHeights, new CompanyConfig("Murrieta Heights", "murrietaheights") },
            {WarCompanies.UnionGrainSupply, new CompanyConfig("Union Grain Supply", "uniongrainsupply") },
        };
        public static void Parse()
        {
            if (Directory.Exists("interfaces/gui/src/configs/families"))
            {
                using var w = new StreamWriter("interfaces/gui/src/configs/families/warcompanies.js");
                w.Write($"export default {JsonConvert.SerializeObject(CompaniesConfig)}");
            }
        }
    }
}
