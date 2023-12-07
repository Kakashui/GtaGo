using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.ReferralSystem
{
    public class ReferralModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int ReferrerUUID { get; set; }
        public List<int> ReferralUUIDs { get; set; }
    }
}
