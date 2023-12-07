using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Whistler.Domain.Phone.Messenger
{
    [Table("phones_msg_chats")]
    public class Chat
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public ChatType Type { get; set; }

        public string InviteCode { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Avatar { get; set; }

        public ICollection<AccountToChat> AccountToChats { get; set; } = new List<AccountToChat>();

        public override bool Equals(object obj)
        {
            return obj is Chat chat &&
                   Id == chat.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public enum ChatType
    {
        Private,
        Group,
        Channel
    }
}
