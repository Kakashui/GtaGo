using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Fractions.PDA.Models;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Fractions.PDA
{
    class PoliceCalls
    {
        private static int _counter = 0;
        private static Dictionary<int, PoliceCall> _policeCalls = new Dictionary<int, PoliceCall>();

        public static string GetAllPoliceCallsDTO()
        {
            return JsonConvert.SerializeObject(_policeCalls.Select(item => item.Value.GetDTO()));
        }
        public static void CreatePoliceCall(ExtPlayer player, int code)
        {
            if (Manager.countOfFractionMembers(7) == 0 && Manager.countOfFractionMembers(9) == 0)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_455", 3000);
                return;
            }
            if (player.HasData("NEXTCALL_POLICE") && DateTime.Now < player.GetData<DateTime>("NEXTCALL_POLICE"))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_456", 3000);
                return;
            }
            SafeTrigger.SetData(player, "NEXTCALL_POLICE", DateTime.Now.AddMinutes(7));

            DeletePoliceCallOfPlayer(player, true);
            int id = _counter++;
            PoliceCall call = new PoliceCall(player, id, code, player.Name, player.Character.UUID, player.Position);
            _policeCalls.Add(id, call);
            SubscribeToPda.TriggerEventToSubscribers(PDAConstants.NEW_POLICE_CALL, JsonConvert.SerializeObject(call.GetDTO()));

            string message = code == 999 ? $"Frac_547".Translate(player.Character.UUID) : "Frac_459".Translate(player.Character.UUID, $"Code {code}");
            Chat.SendFractionMessage(7, message, false);
            Chat.SendFractionMessage(9, message, false);
        }

        public static void DeletePoliceCallOfPlayer(ExtPlayer player, bool accept = false)
        {
            var call = _policeCalls.FirstOrDefault(item => item.Value.CallerUUID == player.Character.UUID);
            if (call.Value != null)
            {
                _policeCalls[call.Key].Destroy(accept);
                SubscribeToPda.TriggerEventToSubscribers(PDAConstants.DELETE_POLICE_CALL, call.Key);
                _policeCalls.Remove(call.Key);
            }
        }

        public static void AcceptCall(ExtPlayer player, int callId)
        {
            if (!_policeCalls.ContainsKey(callId))
                return;
            if (_policeCalls[callId].AddHelper(player))
                SubscribeToPda.TriggerEventToSubscribers(PDAConstants.UPDATE_HELPERS, callId, JsonConvert.SerializeObject(_policeCalls[callId].Helpers));
        }

        public static void SubHelperForAllCalls(ExtPlayer player, bool trigger)
        {
            foreach (var call in _policeCalls)
            {
                if (call.Value.SubHelper(player, trigger))
                    SubscribeToPda.TriggerEventToSubscribers(PDAConstants.UPDATE_HELPERS, call.Key, JsonConvert.SerializeObject(call.Value.Helpers));
            }
        }
    }
}
