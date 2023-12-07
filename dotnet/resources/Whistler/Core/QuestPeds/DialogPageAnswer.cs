using System;
using GTANetworkAPI;
using Whistler.Entities;

namespace Whistler.Core.QuestPeds
{
    internal class DialogPageAnswer
    {
        public string Header { get; }

        public Action<ExtPlayer> Callback { get; set; }

        public string RedirectData { get; }

        public DialogPage PageToRedirect { get;}
        
        public DialogPageAnswer(string header, Action<ExtPlayer> callback, DialogPage pageToRedirect = null)
        {
            Header = header;
            Callback = callback;
            RedirectData = pageToRedirect?.GetSerializedData();
            PageToRedirect = pageToRedirect;
        }
    }
}