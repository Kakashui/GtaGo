using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Whistler.Domain.Phone.Contacts
{
    [Table("phones_simcards")]
    public class SimCard
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Номер симкарты.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Список контактов.
        /// </summary>
        public List<Contact> Contacts { get; set; } = new List<Contact>();

        /// <summary>
        /// Номер счета
        /// </summary>
        public int BankNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public SimCard()
        {

        }
    }
}
