using GTANetworkAPI;
using Whistler.Core;
using Whistler.Helpers;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models;
using Whistler.VehicleSystem.Models.VehiclesData;
using Whistler.Inventory.Enums;
using Whistler.Common;
using Whistler.Entities;

namespace Whistler.Fractions.SupplyManagers
{
    public class MedkitsSupply : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(MedkitsSupply));
        private const int CLIENT_MARKER_UID = 717;

        private static readonly Vector3 PlaneLandingPoint = new Vector3(2120.978, 4805.3, 41.19);
        private static readonly Vector3 PlaneStartPoint = new Vector3(725.5725, 4137.339, 474.3434);

        private static int MedkitsLeftInSupplyPlane;

        private static InteractShape _interactShape = null;

        private static ExtVehicle _plane = null;

        [ServerEvent(Event.ResourceStart)]
        public void HandleResourceStart()
        {
            var colshape = NAPI.ColShape.CreateCylinderColShape(PlaneLandingPoint, 8, 5);
            colshape.OnEntityEnterColShape += PlayerLandPlane;
        }

        private void PlayerLandPlane(ColShape colShape, Player client)
        {
            if (!(client is ExtPlayer player)) return;

            if (!player.HasData("SUPPLYMEDS") || !player.IsInVehicle) 
                return;
            ExtVehicle extVehicle = player.Vehicle as ExtVehicle;
            if (extVehicle.Data.OwnerType != OwnerType.Temporary || (extVehicle.Data as TemporaryVehicle).Access != VehicleAccess.MedKits)
                return;

            Notify.Send(player, NotifyType.Alert, NotifyPosition.Bottom, "msply:1", 3000);

            player.DeleteClientMarker(CLIENT_MARKER_UID);
            player.ResetData("SUPPLYMEDS");

            NAPI.Task.Run(() =>
            {
                MedkitsLeftInSupplyPlane = 20;

                // к позиции самолёта прибавляем обратное направление самолёта,
                // умноженное на нужное нам расстояние от центра самолёта и потом опускаем на уровень дна самолёта
                var inPlaneTrunkPosition = extVehicle.Position + extVehicle.Rotation.GetDirectionFromRotation().Multiply(-1).Multiply(4) - new Vector3(0, 0, 0.45);

                _interactShape = InteractShape.Create(inPlaneTrunkPosition, 1, 2)
                    .AddDefaultMarker()
                    .AddInteraction(GetMedkits);

                VehicleStreaming.SetDoorState(extVehicle, DoorID.DoorTrunk, DoorState.DoorOpen);
            }, 3000);
        }

        private void GetMedkits(ExtPlayer player)
        {
            if (MedkitsLeftInSupplyPlane == 0)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.Bottom, "medsupply_1".Translate(), 3000);
                return;
            }

            if (player.IsPlayerHaveContainer())
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.Bottom, "medsupply_2".Translate(), 3000);
                return;
            }
            var boxName = ItemNames.MedkitBox;

            var box = SupplyConfig.GetRandomTypeBox(boxName);
            if (box == null)
                return;
            MedkitsLeftInSupplyPlane--;

            player.GiveContainerToPlayer(box, box.Config.AttachId);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "matsupply_5".Translate(), 3000);
        }

        [Command("endmedsupply")]
        public void EndMedkitsSupply(ExtPlayer player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "endmedsupply")) return;

                DestroyObjects();
            }
            catch (Exception e) { _logger.WriteError($"Unhandled error catched on EndMedkitsSupply: " + e.ToString()); }
        }

        private void DestroyObjects()
        {
            if (_interactShape != null)
            {
                _interactShape.Destroy();
                _interactShape = null;
            }
            if (_plane != null)
            {
                _plane.CustomDelete();
                _plane = null;
            }
        }

        [Command("supplymed")]
        public void StartMedkitsSupply(ExtPlayer player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "supplymed")) return;

                DestroyObjects();

                //player.ChangePosition(null);

                _plane = VehicleManager.CreateTemporaryVehicle(VehicleHash.Titan, PlaneStartPoint, new Vector3(0, 0, 300), "MEDSUPPLY", VehicleAccess.MedKits);
                VehicleStreaming.SetEngineState(_plane, true);
                VehicleStreaming.SetVehicleFuel(_plane, 10000);
                player.CustomSetIntoVehicle(_plane, VehicleConstants.DriverSeatClientSideBroken);
                //player.SetIntoVehicle(_plane, VehicleConstants.DriverSeat);
                SafeTrigger.SetData(player, "SUPPLYMEDS", true);

                player.CreateClientCheckpoint(CLIENT_MARKER_UID, 1, PlaneLandingPoint - new Vector3(0, 0, 3), 10, 0, new Color(255, 0, 0));
                Notify.Send(player, NotifyType.Info, NotifyPosition.Bottom, "msply:2", 3000);
            }
            catch (Exception e) { _logger.WriteError($"Unhandled error catched on StartMedkitsSupply: " + e.ToString()); }
        }
    }
}
