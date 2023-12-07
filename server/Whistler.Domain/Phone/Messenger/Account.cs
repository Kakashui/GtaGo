using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Whistler.Domain.Phone.Contacts;

namespace Whistler.Domain.Phone.Messenger
{
    [Table("phones_msg_accounts")]
    public class Account
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; }

        public int SimCardId { get; set; }
        public SimCard SimCard { get; set; }

        public string DisplayedName { get; set; }

        public bool IsNumberHided { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? LastVisit { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Account account &&
                   Id == account.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
