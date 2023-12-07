using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Whistler.Helpers;
using Whistler.MoneySystem;
using Whistler.MoneySystem.Models;

namespace Whistler.Core.ReportSystem.Models
{
    class MoneyTransfer
    {
        public int ID { get; set; }
        public ulong SocialClubFrom { get; set; } 
        public string FromName { get; set; }
        public string ToName { get; set; }
        [JsonIgnore]
        public CheckingAccount From { get; set; }
        [JsonIgnore]
        public CheckingAccount To { get; set; }
        public int Amount { get; set; }
        public string Comment { get; set; }
        public string Reason { get; set; }
        public MoneyTransfer(int id, ulong socialClubFrom, string fromName, string toName, CheckingAccount from, CheckingAccount to, int amount, string comment, string reason)
        {
            ID = id;
            SocialClubFrom = socialClubFrom;
            FromName = fromName;
            ToName = toName;
            From = from;
            To = to;
            Amount = amount;
            Comment = comment;
            Reason = reason;
        }
        public void Send(string eventName, object data)
        {
            ReportManager.Admins.ForEach(item => item.TriggerCefEvent(eventName, data));
        }

        public void Success(string adminName)
        {
            Wallet.TransferMoney(From, To, Amount, 0, $"Перевод на счет ({Comment})");
            MoneyManager.UpdateTransgerLimit(SocialClubFrom, Amount);
            Send("transfersConfirmation/deleteTransfersList", ID);
            GameLog.Admin(adminName, $"Transfer({From.ID}, {To.ID})", FromName);
        }

        public void Canceled(string adminName)
        {
            Send("transfersConfirmation/deleteTransfersList", ID);
            GameLog.Admin(adminName, $"NotTransfer({From.ID}, {To.ID})", FromName);
        }
    }
}
