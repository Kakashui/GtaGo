using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Whistler.Domain.Phone.Taxi
{
    [Table("phones_taxi_orders")]
    public class OrderHistoryItem
    {
        [Key]
        public int Id { get; set; }

        public int DriverUuid { get; set; }

        public DateTime Date { get; set; }

        public int TotalPrice { get; set; }
    }
}
