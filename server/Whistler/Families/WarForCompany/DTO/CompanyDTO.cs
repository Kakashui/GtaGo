using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Common;
using Whistler.Families.WarForCompany.Models;
using Whistler.Fractions;

namespace Whistler.Families.WarForCompany.DTO
{
    class CompanyDTO
    {
        public int id { get; set; }
        public string key { get; set; }
        public bool captureisInProgress { get; set; }
        public int orgId { get; set; }
        public int orgType { get; set; }
        public string date { get; set; }
        public string orgName { get; set; }


        public CompanyDTO(Company company)
        {
            id = company.ID;
            key = company.Key.ToString();
            captureisInProgress = company.IsGoing;
            orgId = company.OwnerId;
            orgType = (int)company.OwnerType;
            if (company.OwnerType == OwnerType.Family)
                orgName = FamilyManager.GetFamilyName(company.OwnerId);
            else
                orgName = Manager.getName(company.OwnerId);
            date = $"{company.DateOfCapture:g}";

        }
    }
}
