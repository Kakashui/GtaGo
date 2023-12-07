using System.Collections.Generic;
using Whistler.Businesses;

namespace Whistler.Docks
{
    internal class DockProductOrder
    {
        public int Id { get; set; }
        
        public List<ProductSettings> Products = new List<ProductSettings>();

        public int BusinessOwnerId { get; set; }
    }
}