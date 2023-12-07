using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Phone.Messenger.Dtos
{
    internal class FullAccountDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string DisplayedName { get; set; }

        public int Number { get; set; }

        public bool IsNumberHided { get; set; }

        public bool IsMuted { get; set; }

        public bool IsBlocked { get; set; }

        public int? LastVisitTime { get; set; }
        public bool IsOnline { get; set; }
    }
}
