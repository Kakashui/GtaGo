using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Common.Interfaces
{
    interface IMember
    {
        public int PlayerUUID { get; set; }
        public int Rank { get; set; }
    }
}
