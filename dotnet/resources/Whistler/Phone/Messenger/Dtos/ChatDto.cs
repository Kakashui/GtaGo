using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Domain.Phone.Messenger;

namespace Whistler.Phone.Messenger.Dtos
{
    internal class ChatDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Domain.Phone.Messenger.ChatType Type { get; set; }

        public int NonReadMessagesCount { get; set; }

        public MessageDto LastMessage { get; set; }

        public int CreatedTime { get; set; }

        public int AccountId { get; set; } = -1;

        public string IconColors { get; set; }

        public bool IsMuted { get; set; }
        public bool IsOnline { get; set; }
        public ChatDto()
        {

        }
    }
}
