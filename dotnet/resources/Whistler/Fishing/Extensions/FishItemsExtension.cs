using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Inventory.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Fishing.Extensions
{
    static class FishItemsExtension
    {
        internal static Cage GetFreeCage(this List<BaseItem> items)
        {
            return items.FirstOrDefault(i => i.Type == ItemTypes.Cage && (i as Cage).GetFreeSpace() > 0) as Cage;
        }
        internal static Cage GetCage(this List<BaseItem> items)
        {
            return items.FirstOrDefault(i => i.Type == ItemTypes.Cage) as Cage;
        }
        internal static Cage GetNotFreeCage(this List<BaseItem> items)
        {
            return items.FirstOrDefault(i => i.Type == ItemTypes.Cage && (i as Cage).Fishings.Count > 0) as Cage;
        }
        internal static Rod GetActiveRod(this List<BaseItem> items)
        {
            return items.FirstOrDefault(i => i.Type == ItemTypes.Rod && (i.Name == ItemNames.MiddleRod || i.Name == ItemNames.LowRod || i.Name == ItemNames.HightRod || i.Name == ItemNames.PerfectRod)) as Rod;
        }
        internal static BaseItem GetBait(this List<BaseItem> items)
        {
            return items.FirstOrDefault(i => i.Name == ItemNames.FishingBait);
        }
        //internal static nItem GetMap(this List<nItem> items)
        //{
        //    return items.FirstOrDefault(i => i.Type == ItemType.FishingMap);
        //}
        //internal static bool HasMap(this List<nItem> items)
        //{
        //    return items.Any(i => i.Type == ItemType.FishingMap);
        //}
        //internal static bool HasCage(this List<nItem> items)
        //{
        //    return items.Any(i => i.Type == ItemType.LowFishingCage || i.Type == ItemType.MiddleFishingCage || i.Type == ItemType.HightFishingCage);
        //}
    }
}
