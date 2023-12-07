using System;
using System.Linq;
using System.Threading.Tasks;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Whistler.Core.Character;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.Phone.Messenger.Chat.Updates;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Chat
{
    class DisconnectInChat
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(DisconnectInChat));
        public static async Task DisconnectChats(Character character)
        {
            try
            { 
                var accountId = character.PhoneTemporary.Phone?.AccountId ?? 0;

                if (accountId == 0)
                    return;

                using (var context = DbManager.TemporaryContext)
                {
                    var accountChats = await context.AccountsToChats
                        .AsNoTracking()
                        .Where(ac => ac.AccountId == accountId)
                        .Where(ac => !ac.IsLeaved && !ac.IsBlocked)
                        .Include(ac => ac.Chat)
                        .ToListAsync();

                    var chatsIds = accountChats.Where(ac => ac.Chat.Type == Domain.Phone.Messenger.ChatType.Private).Select(ac => ac.Chat.Id)
                            .ToList();


                    var displayedNames = await context.AccountsToChats
                        .Where(ac => ac.AccountId != accountId)
                        .Where(ac => chatsIds.Contains(ac.ChatId))
                        .Include(ac => ac.Account)
                        .Select(ac => new { ac.ChatId, ac.Account })
                        .ToListAsync();

                    accountChats.ForEach(ac =>
                    {
                        var targetAcc = displayedNames.FirstOrDefault(item => item.ChatId == ac.Chat.Id);
                        if (targetAcc != null)
                        {
                            UpdatR.SendUpdate(targetAcc.Account, new UserOnlineUpdate(targetAcc.Account, ac.Chat, false));
                        }
                    });

                }
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() =>
                {
                    _logger.WriteError($"Unhandled exception catched on LoadChats ({character?.FullName}) - " + e.ToString());
                });
            }
        }
    }
}
