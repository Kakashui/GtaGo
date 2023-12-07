using System;
using System.Collections.Generic;
using GTANetworkAPI;
using Whistler.Entities;
using Whistler.SDK;

namespace Whistler.Core.UserDialogs
{
    internal class UserDialog
    {
        public static Dictionary<int, UserDialog> AllDialogs = new Dictionary<int, UserDialog>();
        public int Id { get; }

        public event Action PlayerAgreed;
        public event Action PlayerDisAgreed;
        
        public UserDialog(ExtPlayer player, string suggestionMessage)
        {
            Id = AllDialogs.Count;
            AllDialogs.Add(Id, this);
            SafeTrigger.ClientEvent(player, "openDialog", "NewUserDialog" + Id, suggestionMessage);
        }

        public void InvokeOnPlayerDisAgreed()
        {
            PlayerDisAgreed?.Invoke();
        }
        
        public void InvokeOnPlayerAgreed()
        {
            PlayerAgreed?.Invoke();
        }
    }
}