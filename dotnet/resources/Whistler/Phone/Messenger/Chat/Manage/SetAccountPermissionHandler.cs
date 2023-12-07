using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Whistler.Domain.Phone.Messenger;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.Phone.Messenger.Accounts;
using Whistler.SDK;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Chat.Manage
{
    internal class SetAccountPermissionHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(SetAccountPermissionHandler));

        [RemoteEvent("phone::msg::manage::setAccountPermission")]
        public async Task HandleSetAccountPermission(ExtPlayer player, int chatId, int adminId, int intPermission, bool newValue)
        {
            try
            {
                var character = player.Character;
                var account = character.PhoneTemporary.Phone?.Account;

                if (account == null)
                    return;

                if (account.Id == adminId)
                    return;

                using (var context = DbManager.TemporaryContext)
                {
                    var accountToChat = await context.AccountsToChats.FindAsync(account.Id, chatId);
                    if (accountToChat == null || !accountToChat.Permissions.Contains(Permission.PurposingAdmins))
                        return;

                    var adminToChat = await context.AccountsToChats
                        .Include(a => a.Account)
                        .FirstOrDefaultAsync(ac => ac.AccountId == adminId && ac.ChatId == chatId);

                    if (adminToChat == null)
                        return;

                    var permission = (Permission)intPermission;
                    var newPermissions = new List<Permission>(adminToChat.Permissions);

                    if (newValue)
                    {
                        if (adminToChat.Permissions.Contains(permission))
                            return;

                        newPermissions.Add(permission);
                    }
                    else
                    {
                        if (!adminToChat.Permissions.Contains(permission))
                            return;

                        newPermissions.Remove(permission);
                    }

                    adminToChat.Permissions = newPermissions;

                    await context.SaveChangesAsync();
                    await UpdatR.SendUpdate(adminToChat.Account, new ChangedAdminPermissionsUpdate(adminToChat.Account, chatId, adminToChat));
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::manage::removeAdmin ({chatId}, {adminId}) - " + e.ToString()); }
        }
    }
}
