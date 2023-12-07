using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Families.WarForCompany.Models
{
    class CompanyConfig
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public CompanyConfig(string name, string image)
        {
            Name = name;
            Image = image;
        }
    }
}
