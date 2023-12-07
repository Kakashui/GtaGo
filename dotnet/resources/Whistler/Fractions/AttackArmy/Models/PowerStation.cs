using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.Families;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.SDK;
using Whistler.Inventory.Enums;
using Whistler.Entities;

namespace Whistler.Fractions.AttackArmy.Models
{
    class PowerStation
    {
        private Vector3 Position { get; set; }
        public PowerStationStatus Status { get; private set; }
        private InteractShape Shape { get; set; }
        public ExtPlayer InteractedPlayer { get; private set; }
        private bool IsLeader { get; set; }

        public PowerStation(Vector3 position, bool isLeader)
        {
            Position = position;
            IsLeader = isLeader;
            Status = PowerStationStatus.Work;
            Shape = InteractShape.Create(Position, 1.5F, 2)
                .AddDefaultMarker()
                .AddInteraction(Interact, "pwrstation:9")
                .AddOnExitColshapeExtraAction(ExitShape);

        }

        private void Interact(ExtPlayer player)
        {
            if (!player.IsLogged())
                return;
            if (InteractedPlayer.IsLogged() && InteractedPlayer != player)
                return;
            switch (Status)
            {
                case PowerStationStatus.Work:
                    if (!AttackManager.CheckAccess(player, true, IsLeader))
                        return;
                    var equip = player.GetEquip();
                    if (!equip.Weapons.ContainsKey(equip.CurrentWeapon) || equip.Weapons[equip.CurrentWeapon].Name != ItemNames.Crowbar)
                    {
                        Notify.SendError(player, "pwrstation:1");
                        return;
                    }
                    if (!AttackManager.CanBreakingStation(Status))
                    {
                        Notify.SendError(player, "pwrstation:2");
                        return;
                    }
                    InteractedPlayer = player;
                    SetStatus(PowerStationStatus.Breaking);
                    if (!AttackManager.StartTimer(PowerStationStatus.Breaking, PowerStationStatus.Broken, "pwrstation:3"))
                        Notify.SendSuccess(player, "pwrstation:4");
                    break;
                case PowerStationStatus.Breaking:
                    if (!AttackManager.CheckAccess(player, true, IsLeader))
                        return;
                    InteractedPlayer = null;
                    Notify.SendSuccess(player, "pwrstation:5");
                    SetStatus(PowerStationStatus.Work);
                    AttackManager.StopTimer();
                    break;

                case PowerStationStatus.Repairing:
                    if (!AttackManager.CheckAccess(player, false, IsLeader))
                        return;
                    InteractedPlayer = null;
                    SetStatus(PowerStationStatus.Broken);
                    AttackManager.StopTimer();
                    break;
                case PowerStationStatus.Broken:
                    if (!AttackManager.CheckAccess(player, false, IsLeader))
                        return;
                    if (!AttackManager.CanBreakingStation(Status))
                    {
                        Notify.SendError(player, "pwrstation:6");
                        return;
                    }
                    InteractedPlayer = player;
                    SetStatus(PowerStationStatus.Repairing);
                    if (!AttackManager.StartTimer(PowerStationStatus.Repairing, PowerStationStatus.Work, "pwrstation:7"))
                        Notify.SendSuccess(player, "pwrstation:8");
                    break;
                default:
                    return;
            }
        }

        private void ExitShape(ColShape shape, ExtPlayer player)
        {
            if (!InteractedPlayer.IsLogged())
                return;
            if (InteractedPlayer == player)
            {
                if (Status == PowerStationStatus.Breaking)
                    SetStatus(PowerStationStatus.Work);
                if (Status == PowerStationStatus.Repairing)
                    SetStatus(PowerStationStatus.Broken);
                InteractedPlayer = null;
            }
        }

        private void UpdateMarker()
        {
            Shape.DeleteMarker();
            if (Status == PowerStationStatus.Work)
                Shape.AddDefaultMarker();
            else if (Status == PowerStationStatus.Broken)
                Shape.AddDefaultMarker(new Color(255, 0, 0));
            else
                Shape.AddDefaultMarker(new Color(255, 160, 0));
        }
        public void SetStatus(PowerStationStatus status)
        {
            Status = status;
            UpdateMarker();
        }
    }
}
