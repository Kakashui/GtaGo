using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
using Whistler.VehicleSystem.Models;
using Whistler.VehicleSystem.Models.VehiclesData;

namespace Whistler.SDK.StockSystem
{
    class FractionStock : StockBase
    {
        private ExtPlayer _loader = null;
        private string _timerLoad = null;
        public FractionStock(DataRow row) : base(row)
        {
            TypeOwner = OwnerType.Fraction;
        }

        public FractionStock(int fractionId, string password, StockConfig config, Vector3 position, uint dimension, int size) : base(fractionId, password, config, position, dimension, OwnerType.Fraction, size)
        {

        }

        public override void OpenStock(ExtPlayer player)
        {
            if (!player.IsLogged())
                return;
            if (Manager.isLeader(player, OwnerId))
                InventoryService.OpenStock(player, InventoryId, StockTypes.Default);
            else if (OwnerId == 14 && Size > 2 && Fractions.AttackArmy.AttackManager.CheckLoadItems(player))
            {
                if (_loader.IsLogged() || _timerLoad != null)
                {
                    Notify.SendError(player, "stock:open:1");
                    return;
                }
                if (!player.IsInVehicle)
                {
                    Notify.SendError(player, "stock:open:2");
                    return;
                }
                ExtVehicle vehData = player.Vehicle as ExtVehicle;
                if (vehData.Data.OwnerType != OwnerType.Fraction || vehData.Data.OwnerID != 14 || vehData.Data.ModelName.ToLower() != "brickade")
                {
                    Notify.SendError(player, "stock:open:3");
                    return;
                }

                var inventory = InventoryService.GetById(InventoryId);
                var vehInv = InventoryService.GetById(vehData.Data.InventoryId);
                if (inventory == null || vehInv == null) return;
                var item = inventory.Items.FirstOrDefault(item => item.Type == ItemTypes.ItemBox);
                if (item == null)
                {
                    Notify.SendError(player, "stock:open:4");
                    return;
                }
                if (!vehInv.CanAddItem(item))
                {
                    Notify.SendError(player, "stock:open:5");
                    return;
                }
                _loader = player;
                Notify.SendSuccess(player, "stock:open:6");
                _timerLoad = Timers.StartOnce(15000, () => LoadItems(player, vehData.Data as FractionVehicle));
            }
            else
            {
                SafeTrigger.SetData(player, "fraction_stock_id", Id);
                player.OpenInput("Core1_81", "Core1_82", 8, "fraction_stock_input_password");
            }
        }
        private void LoadItems(ExtPlayer player, FractionVehicle vehData)
        {
            _timerLoad = null;
            _loader = null;
            if (!player.IsLogged())
                return;
            var inventory = InventoryService.GetById(InventoryId);
            var vehInv = InventoryService.GetById(vehData.InventoryId);
            if (inventory == null || vehInv == null) return;
            var firstBoxName = inventory.Items.FirstOrDefault(item => item.Type == ItemTypes.ItemBox)?.Name ?? ItemNames.Invalid;
            var item = inventory.SubItemByName(firstBoxName, 1);
            if (item == null)
            {
                Notify.SendError(player, "stock:load:1");
                return;
            }
            if (!vehInv.AddItem(item, log: LogAction.LoadFromStock))
            {
                inventory.AddItem(item);
                Notify.SendError(player, "stock:load:2");
            }
            else
            {
                Notify.SendSuccess(player, "stock:load:3");
                Fractions.AttackArmy.AttackManager.AddLoaderItems(player);
                vehData.LastLoadItems = DateTime.Now;
            }

        }

        public override void ExitShape(ColShape shape, ExtPlayer player)
        {
            if (_loader.IsLogged() && _loader == player)
            {
                _loader = null;
                if (_timerLoad != null)
                {
                    Timers.Stop(_timerLoad);
                    _timerLoad = null;
                }
                Notify.SendError(player, "stock:exit");
            }
        }

        public override void ChangePassword(ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            if (!Manager.isLeader(player, OwnerId)) return;

            SafeTrigger.SetData(player, "fraction_stock_id", Id);
            player.OpenInput("Core1_84".Translate(Password), "Core1_82", 5, "fraction_stock_change_password");
        }

        public override bool EnterPredicate(ColShape shape, ExtPlayer player)
        {
            if (!player.IsLogged())
                return false;
            if (player.Character.AdminLVL > 0)
                return true;
            if (player.Character.FractionID == OwnerId)
                return true;
            else
                return OwnerId == 14 && Size > 2 && Fractions.AttackArmy.AttackManager.CheckLoadItems(player);
        }

    }
}
