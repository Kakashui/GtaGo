using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whistler.NewDonateShop.Models
{
    public class RarityModelFlyoutMenuItem
    {
        public RarityModelFlyoutMenuItem()
        {
            TargetType = typeof(RarityModelFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}