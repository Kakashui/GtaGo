using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Core;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Fractions.PDA.Models
{
    class PoliceCall
    {
        public int Id { get; }
        public DateTime Time { get; }
        public int Code { get; }
        public string Caller { get; }
        public int CallerUUID { get; }
        public Vector3 Position { get; }
        public List<HelperCall> Helpers { get; set; }
        private Blip CallBlip { get; set; }
        private ColShape Shape { get; set; }
        private Player _target { get; }

        public PoliceCall(Player player, int id, int code, string caller, int callerUUID, Vector3 position)
        {
            Id = id;
            Time = DateTime.Now;
            Code = code;
            Caller = caller;
            CallerUUID = callerUUID;
            Position = position;
            Helpers = new List<HelperCall>();
            _target = player;
            CallBlip = NAPI.Blip.CreateBlip(0, position, 1, 38, "Call from " + caller.Replace('_', ' ') + $" ({player.GetCharacter().UUID})", 0, 0, true, 0, 0);
            CallBlip.Transparency = 0;

            Shape = NAPI.ColShape.CreateCylinderColShape(position, 70, 4, 0);
            Shape.OnEntityExitColShape += (s, e) =>
            {
                try
                {
                    if (e == player)
                        PoliceCalls.DeletePoliceCallOfPlayer(player);
                }
                catch (Exception ex) { }
            };

            Shape.OnEntityEnterColShape += (s, e) =>
            {
                if (e.IsLogged())
                {
                    var uuid = e.GetCharacter().UUID;
                    if (Helpers.FirstOrDefault(item => item.UUID == uuid) != null)
                        PoliceCalls.DeletePoliceCallOfPlayer(player, true);
                }
            };
        }
        public PoliceCallDTO GetDTO()
        {
            return new PoliceCallDTO(this);
        }

        public bool AddHelper(Player player)
        {
            if (!player.IsLogged())
                return false;
            if (Helpers.FirstOrDefault(item => item.UUID == player.GetCharacter().UUID) != null)
                return false;
            Helpers.Add(new HelperCall(player));
            player.TriggerEvent("changeBlipAlpha", CallBlip, 255);
            Trigger.ClientEvent(player, "createWaypoint", CallBlip.Position.X, CallBlip.Position.Y);

            Chat.SendFractionMessage(7, "Frac_462".Translate(player.Name.Replace('_', ' '), _target.GetCharacter().UUID), true);
            Notify.Send(_target, NotifyType.Info, NotifyPosition.BottomCenter, "Frac_463".Translate(player.GetCharacter().UUID), 3000);

            return true;
        }

        public bool SubHelper(Player player, bool trigger)
        {
            if (!player.IsLogged())
                return false;
            var helper = Helpers.FirstOrDefault(item => item.id == player.GetCharacter().UUID);
            if (helper == null)
                return false;
            Helpers.Remove(helper);
            if (trigger)
                player.TriggerEvent("changeBlipAlpha", CallBlip, 0);
            return true;
        }

        public void Destroy(bool accept)
        {
            CallBlip.Delete();
            Shape.Delete();

            if (!accept)
            {
                Chat.SendFractionMessage(7, "Frac_457".Translate(Caller.Replace('_', ' ')), false);
                Chat.SendFractionMessage(9, "Frac_457".Translate(Caller.Replace('_', ' ')), false);
            }
        }
    }
}
