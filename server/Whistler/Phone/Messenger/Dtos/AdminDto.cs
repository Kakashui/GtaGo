using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Phone.Messenger.Dtos
{
    internal class AdminDto
    {
        public int Id { get; set; }

        public string DisplayedName { get; set; }

        public bool IsOnline { get; set; }

        public int AdminLvl { get; set; }

        public List<int> Permissions { get; set; }
    }
}
