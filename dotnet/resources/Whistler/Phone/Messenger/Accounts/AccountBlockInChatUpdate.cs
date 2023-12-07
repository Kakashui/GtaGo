using GTANetworkAPI;
using Whistler.Domain.Phone.Messenger;
using Whistler.UpdateR;
using Whistler.Helpers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Whistler.Entities;

namespace Whistler.Phone.Messenger.Accounts
{
    internal class AccountBlockInChatUpdate : IUpdate<Account>
    {
        public Account UpdateTarget { get; }
        public int ChatId { get; }
        public bool IsBlocked { get; }

        public AccountBlockInChatUpdate(Account blockedAccount, int chatId, bool isBlocked)
        {
            UpdateTarget = blockedAccount;
            ChatId = chatId;
            IsBlocked = isBlocked;
        }
    }

    internal class AccountBlockedInChatHandler : IUpdateHandler<AccountBlockInChatUpdate, Account>
    {
        public async Task Handle(Player subscriber, AccountBlockInChatUpdate update)
        {
            if (!(subscriber is ExtPlayer player)) return;

            player.TriggerCefAction("smartphone/messagePage/msg_setInChatBlocked", JsonConvert.SerializeObject(new { update.ChatId, update.IsBlocked }));
            UpdatR.Unsubscribe(subscriber, update.UpdateTarget);
        }
    }
}
