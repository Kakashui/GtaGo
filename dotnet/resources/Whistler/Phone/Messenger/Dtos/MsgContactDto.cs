using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Phone.Messenger.Dtos
{
    internal class MsgContactDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public int? Time { get; set; }

        public int AccountId { get; set; }
        public bool IsOnline { get; set; }
    }
}
