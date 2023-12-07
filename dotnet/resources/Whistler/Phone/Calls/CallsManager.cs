using GTANetworkAPI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Whistler.Core.Character;
using Whistler.Helpers;
using Whistler.SDK;
using Whistler.Domain.Phone.Contacts;
using Newtonsoft.Json;
using Whistler.Core;
using Whistler.Entities;

namespace Whistler.Phone.Calls
{
    internal class CallsManager : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(CallsManager));

        private static readonly ConcurrentDictionary<ExtPlayer, CallProcess> _currentCalls = new ConcurrentDictionary<ExtPlayer, CallProcess>();

        public CallsManager()
        {
            Timers.Start(1000, DropAutoDeclinedCalls);
            Main.PlayerPreDisconnect += HandlePlayerDisconnect;
        }

        private async void DropAutoDeclinedCalls()
        {
            try
            {
                var currentCalls = new Dictionary<ExtPlayer, CallProcess>(_currentCalls);
                foreach ((var player, var process) in currentCalls)
                {
                    if (!_currentCalls.ContainsKey(player))
                        continue;

                    if (!(process.CurrentState is CallingState))
                        continue;

                    var callingState = (CallingState)process.CurrentState;
                    if (callingState.AutoDeclineAt < DateTime.Now)
                    {
                        var dropData = DropCall(player, "phone:call:drop");
                        if (dropData != null)
                        {
                            await CallHistoryManager.AddCallHistoryItem(dropData.Value.call);
                            CallHistoryManager.SendPlayerCallDto(dropData.Value.from, dropData.Value.call);
                            CallHistoryManager.SendPlayerCallDto(dropData.Value.target, dropData.Value.call);
                        }
                    }
                }
            }
            catch (Exception e) { _logger.WriteError("Unhandled exception catched on DropAutoDeclinedCalls - " + e.ToString()); }
        }

        private async void HandlePlayerDisconnect(ExtPlayer player)
        {
            try
            {
                var dropData = DropCall(player, "phone:call:end");
                if (dropData != null)
                {
                    await CallHistoryManager.AddCallHistoryItem(dropData.Value.call);
                    CallHistoryManager.SendPlayerCallDto(dropData.Value.from, dropData.Value.call);
                    CallHistoryManager.SendPlayerCallDto(dropData.Value.target, dropData.Value.call);
                }
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() =>
                {
                    _logger.WriteError($"Unhandled exception catched on HandlePlayerDisconnect ({player?.Name}) - " + e.ToString());
                });
            }
        }
        public static bool SendCall(int targetNumber, ExtPlayer from, int fromNumber)
        {
            ExtPlayer target = Main.GetExtPlayerByPredicate(item => (item.Character.PhoneTemporary?.Phone?.SimCard?.Number ?? -1) == targetNumber);

            if (!target.IsLogged())
                return false;

            if (_currentCalls.ContainsKey(target))
                return false;

            if (target.HasData("IS_DYING") || target.Character.Cuffed)
            {
                return false;
            }
            // TODO: [MK] check avia mode target

            CallProcess callProcess = new CallProcess
            {
                Target = target,
                TargetNumber = targetNumber,
                From = from,
                FromNumber = fromNumber,
                CreatedAt = DateTime.Now,
                CurrentState = new CallingState { AutoDeclineAt = DateTime.Now.AddSeconds(10) }
            };

            if (!_currentCalls.TryAdd(target, callProcess))
            {
                return false;
            }

            if (!_currentCalls.TryAdd(from, callProcess))
            {
                _currentCalls.TryRemove(target, out CallProcess outTargetValue);
                return false;
            }

            target.TriggerEventSafe("phone:calls:incomeCall", fromNumber);
            return true;
        }

        public static bool TakeCall(ExtPlayer player)
        {
            _currentCalls.TryGetValue(player, out var callProcess);
            if (callProcess == null)
            {
                return false;
            }

            callProcess.From.TriggerEventSafe("voice.phoneCall", callProcess.Target);
            callProcess.Target.TriggerEventSafe("voice.phoneCall", callProcess.From);

            callProcess.From.TriggerCefEvent("smartphone/setCallActive", callProcess.TargetNumber);
            callProcess.Target.TriggerCefEvent("smartphone/setCallActive", callProcess.FromNumber);

            callProcess.CurrentState = new SpeakingState
            {
                StartedAt = DateTime.Now
            };

            return true;
        }

        public static (Call call, ExtPlayer from, ExtPlayer target)? DropCall(ExtPlayer player, string reason)
        {
            _currentCalls.TryGetValue(player, out var callProcess);
            if (callProcess == null)
            {
                return null;
            }

            callProcess.From.TriggerEventSafe("voice.phoneStop");
            callProcess.Target.TriggerEventSafe("voice.phoneStop");

            var reasonDto = JsonConvert.SerializeObject(new { reason });
            callProcess.From.TriggerCefEvent("smartphone/dropCall", reasonDto);
            callProcess.Target.TriggerCefEvent("smartphone/dropCall", reasonDto);

            _currentCalls.TryRemove(callProcess.From, out var d1);
            _currentCalls.TryRemove(callProcess.Target, out var d2);

            var fromCharacter = callProcess.From.Character;
            var call = new Call
            {
                FromSimCardId = fromCharacter.PhoneTemporary.Phone.SimCard.Id,
                FromSimCard = fromCharacter.PhoneTemporary.Phone.SimCard,
                TargetNumber = callProcess.TargetNumber,
                CreatedAt = callProcess.CreatedAt,
                CallStatus = CalculateCallStatus(player, callProcess),
                Duration = CalculateDuration(callProcess)
            };

            return (call, callProcess.From, callProcess.Target);
        }

        private static CallStatus CalculateCallStatus(ExtPlayer player, CallProcess callProcess)
        {
            if (callProcess.CurrentState is SpeakingState)
                return CallStatus.Accepted;
            else
            {
                if (callProcess.From == player)
                    return CallStatus.Failed;
                else
                    return CallStatus.Rejected;
            }
        }

        private static int CalculateDuration(CallProcess callProcess)
        {
            return callProcess.CurrentState is SpeakingState speakState ?
                (int)(DateTime.Now - speakState.StartedAt).TotalSeconds : 0;
        }

        private class CallProcess
        {
            public ExtPlayer Target { get; set; }
            public int TargetNumber { get; set; }

            public ExtPlayer From { get; set; }
            public int FromNumber { get; set; }
            public DateTime CreatedAt { get; set; }

            public CallState CurrentState { get; set; }
        }

        private abstract class CallState { }

        private class CallingState : CallState
        {
            public DateTime AutoDeclineAt { get; set; }
        }

        private class SpeakingState : CallState
        {
            public DateTime StartedAt { get; set; }
        }
    }
}
