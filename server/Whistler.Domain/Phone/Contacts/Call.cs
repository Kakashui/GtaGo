using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Whistler.Domain.Phone.Contacts
{
    [Table("phones_callhistory")]
    public class Call
    {
        [Key]
        public int Id { get; set; }
        public int FromSimCardId { get; set; }
        public SimCard FromSimCard { get; set; }
        public int TargetNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public CallStatus CallStatus { get; set; }
        public int Duration { get; set; }
    }

    public enum CallStatus
    {
        Rejected,
        Accepted,
        Failed
    }
}
