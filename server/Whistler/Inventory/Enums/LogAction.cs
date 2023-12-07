using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Inventory.Enums
{
    public enum LogAction
    {
        None = 0,
        Create = 1,
        Move = 2,
        Use = 3,
        Delete = 4,
        LoadFromStock = 5,
    }
}
