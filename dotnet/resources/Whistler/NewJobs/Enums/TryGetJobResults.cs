using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.NewJobs.Enums
{
    public enum TryGetJobResults
    {
        OnOtherJob,
        BadCondition,
        Success,
        Already,
        Limit
    }
}
