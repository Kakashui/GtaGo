using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Domain.Phone.Messenger;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Contacts.Updates
{
    internal class ContactOnlineUpdate : IUpdate<Account>
    {
        public Account UpdateTarget { get; }
        public int ContactId { get; }
        public bool IsOnline { get; }

        public ContactOnlineUpdate(Account acc, int contactId, bool isOnline)
        {
            UpdateTarget = acc;
            ContactId = contactId;
            IsOnline = isOnline;
        }
    }

    internal class ContactOnlineUpdateHandler : IUpdateHandler<ContactOnlineUpdate, Account>
    {
        private static string _json;
        public async Task Handle(Player subscriber, ContactOnlineUpdate update)
        {
            if (!(subscriber is ExtPlayer player)) return;

            var json = _json ?? (_json = GetJson(update));
            player.TriggerCefEvent("smartphone/messagePage/msg_setChatIsOnline", json);
        }

        private string GetJson(ContactOnlineUpdate update)
        {
            return JsonConvert.SerializeObject(new { contactId = update.ContactId, isOnline = update.IsOnline });
        }
    }
}
