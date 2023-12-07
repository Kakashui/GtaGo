using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Whistler.Core.Character;
using Whistler.Infrastructure.DataAccess;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GTANetworkAPI;

namespace Whistler.Phone.Messenger.Accounts
{
    class CharacterDisconnect
    {
        private static Whistler.Helpers.WhistlerLogger _logger = new Helpers.WhistlerLogger(typeof(CharacterDisconnect));
        public static async Task DisconnectCharacter(Character character)
        {
            try
            {
                var accountId = character.PhoneTemporary.Phone?.AccountId ?? 0;

                if (accountId == 0)
                    return;

                using (var context = DbManager.TemporaryContext)
                {
                    var account = await context.Accounts.FirstOrDefaultAsync(item => item.Id == accountId);
                    account.LastVisit = DateTime.Now;
                    context.SaveChanges();
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
