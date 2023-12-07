using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Fractions.Models;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Fractions
{
    class ArmorPoints : Script
    {
        private static Dictionary<int, ArmorGivePoint> _armorPoints = new Dictionary<int, ArmorGivePoint>();
        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {

            DataTable result = MySQL.QueryRead("SELECT * FROM `armorpoints`");
            if (result == null || result.Rows.Count == 0)
            {
                return;
            }
            ArmorGivePoint armorPoint;
            foreach (DataRow row in result.Rows)
            {
                armorPoint = new ArmorGivePoint(row);
                if (armorPoint != null)
                    _armorPoints.Add(armorPoint.Id, armorPoint);
            }
        }
        [Command("setarmpoint")]
        public static void CMD_CreateFractionStock(ExtPlayer player, int fracId)
        {
            if (!player.IsLogged())
                return;
            if (!Group.CanUseAdminCommand(player, "setarmpoint"))
                return;
            if (Manager.GetFraction(fracId) == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "armpoints_2", 3000);
                return;
            }
            var armorPoint = new ArmorGivePoint(fracId, player.Position - new Vector3(0, 0, 1), player.Dimension);
            _armorPoints.Add(armorPoint.Id, armorPoint);
        }
        [Command("delarmpoint")]
        public static void CMD_DeleteStock(ExtPlayer player, int armorPoint)
        {
            if (!player.IsLogged())
                return;
            if (!Group.CanUseAdminCommand(player, "armpoint"))
                return;
            if (!_armorPoints.ContainsKey(armorPoint))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "armpoints_1".Translate(armorPoint), 3000);
                return;
            }
            _armorPoints[armorPoint].Destroy();
            _armorPoints.Remove(armorPoint);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "armpoints_3".Translate(armorPoint), 3000);
        }
    }
}
