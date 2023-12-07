using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Whistler.Domain.Phone.Contacts
{
    [Table("phones_contacts")]
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        public int HolderSimCardId { get; set; }
        public SimCard HolderSimCard { get; set; }
        public int TargetNumber { get; set; }
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
