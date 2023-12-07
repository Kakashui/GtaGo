using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Configs;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Models
{
    class InventoryClass
    {
        public List<ItemNames> _itemNamesWhiteList { get; set; }
        public List<ItemTypes> _itemTypesWhiteList { get; set; }
        public List<ItemNames> _itemNamesBlackList { get; set; }
        public List<ItemTypes> _itemTypesBlackList { get; set; }
        public InventoryClass()
        {
            _itemNamesWhiteList = null;
            _itemTypesWhiteList = null;
            _itemNamesBlackList = new List<ItemNames>();
            _itemTypesBlackList = new List<ItemTypes>();
        }

        public bool CanAddedItem(BaseItem item)
        {
            if (_itemNamesBlackList.Contains(item.Name))
                return false;
            if (_itemTypesBlackList.Contains(item.Type))
                return false;
            if (_itemNamesWhiteList != null)
            {
                if (_itemNamesWhiteList.Contains(item.Name))
                    return true;
            }
            if (_itemTypesWhiteList != null)
            {
                if (_itemTypesWhiteList.Contains(item.Type))
                    return true;
            }
            if (_itemNamesWhiteList == null && _itemTypesWhiteList == null)
                return true;
            else
                return false;
        }
    }
}
