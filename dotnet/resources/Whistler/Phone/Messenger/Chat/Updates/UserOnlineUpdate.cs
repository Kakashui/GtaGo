using System.Threading.Tasks;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Domain.Phone.Messenger;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Chat.Updates
{
    internal class UserOnlineUpdate : IUpdate<Account>
    {
        public Account UpdateTarget { get; }
        public Domain.Phone.Messenger.Chat Chat { get; }
        public bool IsOnline { get; }

        public UserOnlineUpdate(Account acc, Domain.Phone.Messenger.Chat chat, bool isOnline)
        {
            UpdateTarget = acc;
            Chat = chat;
            IsOnline = isOnline;
        }
    }

    internal class UserOnlineUpdateHandler : IUpdateHandler<UserOnlineUpdate, Account>
    {
        private static string _json;
        public async Task Handle(Player subscriber, UserOnlineUpdate update)
        {
            if (!(subscriber is ExtPlayer player)) return;

            var json = _json ?? (_json = GetJson(update));
            player.TriggerCefEvent("smartphone/messagePage/msg_setChatIsOnline", json);
        }

        private string GetJson(UserOnlineUpdate update)
        {
            return JsonConvert.SerializeObject(new { ChatId = update.Chat.Id, IsOnline = update.IsOnline });
        }
    }
}
