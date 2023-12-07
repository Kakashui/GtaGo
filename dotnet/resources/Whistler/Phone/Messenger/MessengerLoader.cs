using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Whistler.Core.Character;
using Whistler.Domain.Phone.Messenger;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.SDK;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger
{
    internal class MessengerLoader : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(MessengerLoader));

        public MessengerLoader()
        {
            PhoneLoader.PhoneReadyAsync += LoadMessengerSettings;
        }

        private async Task LoadMessengerSettings(ExtPlayer player, Character character)
        {
            try
            {
                var account = character.PhoneTemporary.Phone?.Account;

                if (account == null)
                    return;

                player.TriggerCefEvent("smartphone/messagePage/msg_setAccountSettings", JsonConvert.SerializeObject(new
                {
                    account.Id,
                    account.Username,
                    account.DisplayedName,
                    account.IsNumberHided
                }));

                UpdatR.Subscribe(player, account);
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() =>
                {
                    _logger.WriteError(
                        $"Unhandled exception catched on LoadPlayerMessengerSettings ({player?.Name}) - " +
                        e.ToString());
                });
            }
        }
    }
}
