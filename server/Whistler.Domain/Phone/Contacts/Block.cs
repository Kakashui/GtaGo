using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Whistler.Domain.Phone.Contacts
{
    [Table("phones_blocks")]
    public class Block
    {
        public int SimCardId { get; set; }

        public int TargetNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
