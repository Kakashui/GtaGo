using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Phone.Messenger.Dtos
{
    internal class BlockDto
    {
        public int AccountId { get; set; }
        public string DisplayedName { get; set; }
        public string BlockedBy { get; set; }
    }
}
