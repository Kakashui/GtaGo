using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Common
{
    public enum OwnerType
    {
        Personal = 0,
        Family = 1,
        Fraction = 2,
        Job = 3,
        Rent = 4,
        Temporary = 5,
        AdminSave = 6,
    }

    public enum OrganizationType
    {
        Family = 1,
        Fraction = 2,
    }

    public enum OrgActivityType
    {
        Invalid = -1,
        Unknown = 0,
        Neutral = 1,
        Crime = 2,
        Government = 3,
        Mafia = 4,
    }
}
