using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Whistler.Core.Character;
using Whistler.Domain.Phone;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.SDK;

namespace Whistler.Phone.AppStore
{
    public class AppStore : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(AppStore));

        public AppStore()
        {
            Main.OnPlayerReadyAsync += LoadPlayerApps;
        }

        private async Task LoadPlayerApps(ExtPlayer player, Character character)
        {
            try
            {
                if (player == null || character == null) return;
                var uuid = character.UUID;

                using (var context = DbManager.TemporaryContext)
                {
                    Domain.Phone.Phone phone = await context.Phones.FindAsync(uuid);
                    if (phone == null)
                        return;

                    player.TriggerCefEvent("smartphone/appPage/setInstalledApps", JsonConvert.SerializeObject(phone.InstalledAppsIds));
                }
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() => _logger.WriteError($"Unhandled exception catched on LoadPlayerApps ({player?.Name}) - " + e.ToString()));
            }
        }

        [RemoteEvent("phone:appStore:install")]
        public void InstallPlayerApplication(ExtPlayer player, int applicationId)
        {
            try
            {
                var character = player.Character;

                Domain.Phone.Phone phone = DbManager.GlobalContext.Phones.Find(character.UUID);
                if (phone == null)
                    return;

                phone.InstalledAppsIds.Add((AppId)applicationId);
                DbManager.GlobalContext.Phones.Update(phone);
                player.TriggerCefEvent("smartphone/appPage/setAppStoreItemIsInstalled", JsonConvert.SerializeObject(new { id = applicationId, value = true }));
                
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone:appStore:install ({applicationId}) - " + e.ToString()); }
        }

        [RemoteEvent("phone:appStore:remove")]
        public void RemovePlayerApplication(ExtPlayer player, int applicationId)
        {
            try
            {
                var character = player.Character;

                Domain.Phone.Phone phone = DbManager.GlobalContext.Phones.Find(character.UUID);
                if (phone == null)
                    return;

                phone.InstalledAppsIds.Remove((AppId)applicationId);
                DbManager.GlobalContext.Phones.Update(phone);
                player.TriggerCefEvent("smartphone/appPage/setAppStoreItemIsInstalled", JsonConvert.SerializeObject(new { id = applicationId, value = false }));
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone:appStore:remove ({applicationId}) - " + e.ToString()); }
        }
    }
}
