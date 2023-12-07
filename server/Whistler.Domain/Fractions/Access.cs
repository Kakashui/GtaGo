using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Whistler.Domain.Fractions
{
    [Table("fraction_access")]
    public class Access
    {
        [Key]
        public int Id { get; set; }
        public int FractionId { get; set; }
        public int FractionRank { get; set; }
        public List<AccessType> AccessList { get; set; }
    }
}
