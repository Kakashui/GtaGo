using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Phone.Messenger.Dtos
{
    internal class SubscriberDto
    {
        public int Id { get; set; }

        public string DisplayedName { get; set; }

        public int? LastVisitTime { get; set; }
        public bool IsOnline { get; set; }
    }
}
