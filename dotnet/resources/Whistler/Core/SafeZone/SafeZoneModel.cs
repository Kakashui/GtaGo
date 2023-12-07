using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Core.SafeZone
{
    public class SafeZoneModel
    {
        public int Id { get; set; }
        public BaseColShape Shape { get; set; }
        public bool Active { get; set; }
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(SafeZoneModel));
        public SafeZoneModel(int id, BaseColShape shape, bool active)
        {
            Id = id;
            Shape = shape;
            Shape.OnEntityEnterColShape += OnEntityEnterColShape;
            Shape.OnEntityExitColShape += OnEntityExitColShape;
            Active = active;
        }
        private void OnEntityEnterColShape(ExtPlayer player)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                player.Character.InSaveZone = Id;
                SafeTrigger.ClientEvent(player,"safeZone", Id, true);
            }
            catch (Exception e) { _logger.WriteError($"SafeZoneEnter: {e.ToString()}"); }

        }
        private void OnEntityExitColShape(ExtPlayer player)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                if (player.Character.InSaveZone == Id)
                    player.Character.InSaveZone = -1;
                SafeTrigger.ClientEvent(player,"safeZone", Id, false);
            }
            catch (Exception e) { _logger.WriteError($"SafeZoneExit: {e.ToString()}"); }
        }
        public void SetActive(bool active)
        {
            if (Active == active)
                return;
            Active = active;
            SafeTrigger.ClientEventForAll("safeZones:setActiveZone", Id, Active);
        }
    }
}
