using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Core;
using Whistler.Entities;
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
        private ExtPlayer _target { get; }

        public PoliceCall(ExtPlayer player, int id, int code, string caller, int callerUUID, Vector3 position)
        {
            Id = id;
            Time = DateTime.Now;
            Code = code;
            Caller = caller;
            CallerUUID = callerUUID;
            Position = position;
            Helpers = new List<HelperCall>();
            _target = player;
            CallBlip = NAPI.Blip.CreateBlip(0, position, 1, 38, "Call from " + caller.Replace('_', ' ') + $" ({player.Character.UUID})", 0, 0, true, 0, 0);
            CallBlip.Transparency = 0;

            Shape = NAPI.ColShape.CreateCylinderColShape(position, 70, 4, 0);
            Shape.OnEntityExitColShape += (s, client) =>
            {
                if (!(client is ExtPlayer e)) return;
                if (e == player)  PoliceCalls.DeletePoliceCallOfPlayer(player);
            };

            Shape.OnEntityEnterColShape += (s, client) =>
            {
                if (!(client is ExtPlayer e)) return;
                if (!e.IsLogged()) return;

                int uuid = e.Character.UUID;
                if (Helpers.FirstOrDefault(item => item.UUID == uuid) == null) return;
                PoliceCalls.DeletePoliceCallOfPlayer(player, true);
            };
        }
        public PoliceCallDTO GetDTO()
        {
            return new PoliceCallDTO(this);
        }

        public bool AddHelper(ExtPlayer player)
        {
            if (!player.IsLogged())
                return false;
            if (Helpers.FirstOrDefault(item => item.UUID == player.Character.UUID) != null)
                return false;
            Helpers.Add(new HelperCall(player));
            SafeTrigger.ClientEvent(player,"changeBlipAlpha", CallBlip, 255);
            SafeTrigger.ClientEvent(player, "createWaypoint", CallBlip.Position.X, CallBlip.Position.Y);

            Chat.SendFractionMessage(7, "Frac_462".Translate(player.Name.Replace('_', ' '), _target.Character.UUID), true);
            Notify.Send(_target, NotifyType.Info, NotifyPosition.BottomCenter, "Frac_463".Translate(player.Character.UUID), 3000);

            return true;
        }

        public bool SubHelper(ExtPlayer player, bool trigger)
        {
            if (!player.IsLogged())
                return false;
            var helper = Helpers.FirstOrDefault(item => item.id == player.Character.UUID);
            if (helper == null)
                return false;
            Helpers.Remove(helper);
            if (trigger)
                SafeTrigger.ClientEvent(player,"changeBlipAlpha", CallBlip, 0);
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
