using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core.CustomSync.Attachments;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Configs.Models
{
    public class ItemBoxConfig: BaseConfig
    {
        public ItemTypes AvailableItem { get; set; }
        public AttachId AttachId { get; set; }
        public int CountGetItem { get; set; }
    }
}
