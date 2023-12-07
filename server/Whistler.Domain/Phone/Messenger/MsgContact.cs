using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Whistler.Domain.Phone.Messenger
{
    [Table("phones_msg_contacts")]
    public class MsgContact
    {
        [Key]
        public int ContactId { get; set; }

        public int HolderAccountId { get; set; }
        public Account HolderAccount { get; set; }

        public int TargetAccountId { get; set; }
        public Account TargetAccount { get; set; }

        public string Name { get; set; }
    }
}
