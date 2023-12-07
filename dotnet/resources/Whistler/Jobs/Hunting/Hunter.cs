using GTANetworkAPI;
using Whistler.Jobs.AbstractEntity;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;

namespace Whistler.Jobs.Hunting
{
    internal class Hunter : AbstractWorker
    {
        public Hunter(ExtPlayer client) : base(client) { }
    }
}
