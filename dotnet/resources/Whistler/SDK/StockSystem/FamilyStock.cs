using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Whistler.Common;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Fractions;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.SDK;

namespace Whistler.SDK.StockSystem
{
    class FamilyStock : StockBase
    {
        public FamilyStock(DataRow row) : base(row)
        {
            TypeOwner = OwnerType.Family;
        }

        public FamilyStock(int fractionId, string password, StockConfig config, Vector3 position, uint dimension) : base(fractionId, password, config, position, dimension, OwnerType.Family)
        {

        }

        public override void OpenStock(ExtPlayer player)
        {
            if (!player.IsLogged())
                return;
            var family = player.GetFamily();
            if (family != null && OwnerId == family.Id && family.IsLeader(player))
                InventoryService.OpenStock(player, InventoryId, StockTypes.Default);
            else
            {
                SafeTrigger.SetData(player, "fraction_stock_id", Id);
                player.OpenInput("Core1_81", "Core1_82", 8, "fraction_stock_input_password");
            }
        }
        public override void ExitShape(ColShape shape, ExtPlayer player)
        {

        }

        public override void ChangePassword(ExtPlayer player)
        {
            if (!player.IsLogged())
                return;
            SafeTrigger.SetData(player, "fraction_stock_id", Id);
            var family = player.GetFamily();
            if (family == null)
                return;
            if (OwnerId == family.Id && family.IsLeader(player))
            {
                player.OpenInput("Core1_84".Translate(Password), "Core1_82", 5, "fraction_stock_change_password");
            }
        }

        public override bool EnterPredicate(ColShape shape, ExtPlayer player)
        {
            if (!player.IsLogged())
                return false;
            if (player.Character.AdminLVL > 0)
                return true;
            if (player.Character.FamilyID == OwnerId)
                return true;
            else
                return false;
        }

    }
}
