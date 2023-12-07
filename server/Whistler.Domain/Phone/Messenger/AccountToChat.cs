using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Whistler.Domain.Phone.Messenger
{
    [Table("phones_msg_accountstochat")]
    public class AccountToChat
    {
        // Composite PK
        public int AccountId { get; set; }
        public Account Account { get; set; }

        // Composite PK
        public int ChatId { get; set; }
        public Chat Chat { get; set; }

        public bool IsLeaved { get; set; }
        public bool IsMuted { get; set; }

        public int? LastReadMessageId { get; set; }
        public Message LastReadMessage { get; set; }

        public AdminLvl AdminLvl { get; set; }
        public bool IsBlocked { get; set; }
        public int? BlockedById { get; set; }
        public Account BlockedBy { get; set; }

        public List<Permission> Permissions { get; set; } = new List<Permission>();
    }

    public enum AdminLvl
    {
        None,
        Administrator,
        Owner
    }

    public enum Permission
    {
        None,
        ChangingGroupProfile,
        DeletingMessages,
        BlockingAccounts,
        PurposingAdmins,
        CreatingPosts
    }
}
