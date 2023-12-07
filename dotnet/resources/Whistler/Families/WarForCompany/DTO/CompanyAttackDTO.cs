using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Common;
using Whistler.Families.WarForCompany.Models;
using Whistler.Fractions;

namespace Whistler.Families.WarForCompany.DTO
{
    class CompanyAttackDTO
    {
        public int id { get; set; }
        public string key { get; set; }
        public int timePassed { get; set; }
        public int time { get; set; }
        public int orgId { get; set; }
        public int orgType { get; set; }
        public string orgName { get; set; }
        public CompanyAttackDTO(Company company)
        {
            id = company.ID;
            key = company.Key.ToString();
            timePassed = (int)(DateTime.Now - company.StartTime).TotalSeconds;
            time = WarConfigs.SECONDS_FOR_CAPT_COMPANY;
            orgId = company.AttackID;
            orgType = (int)company.AttackType;
            if (company.AttackType == OwnerType.Family)
                orgName = FamilyManager.GetFamilyName(company.AttackID);
            else
                orgName = Manager.getName(company.AttackID);
        }
    }
}
