using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Whistler.Domain.Phone.Messenger
{
    [Table("posts")]
    public class Post : Message
    {
        public string Title { get; set; }

        public string Photo { get; set; }
    }
}
