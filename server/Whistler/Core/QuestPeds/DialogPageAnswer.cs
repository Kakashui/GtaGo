using System;
using GTANetworkAPI;
using Whistler.Entities;

namespace Whistler.Core.QuestPeds
{
    internal class DialogPageAnswer
    {
        public string Header { get; }

        public Action<PlayerGo> Callback { get; set; }

        public string RedirectData { get; }

        public DialogPage PageToRedirect { get;}
        
        public DialogPageAnswer(string header, Action<PlayerGo> callback, DialogPage pageToRedirect = null)
        {
            Header = header;
            Callback = callback;
            RedirectData = pageToRedirect?.GetSerializedData();
            PageToRedirect = pageToRedirect;
        }
    }
}