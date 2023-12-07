using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.SDK;

namespace Whistler.Core.QuestPeds
{

    internal class DialogPage
    {
        private readonly string _name;
        private readonly string _role;
        public int Id { get; set; }  
        public string IntroWords { get; }
        public List<DialogPageAnswer> Answers = new List<DialogPageAnswer>();
        public event Action<ExtPlayer> OpenedForPlayer;
        public static Dictionary<ExtPlayer, DialogPage> OpenedPages = new Dictionary<ExtPlayer, DialogPage>();
        
        public DialogPage(string introWords, string name, string role)
        {
            _name = name;
            _role = role;
            IntroWords = introWords;
            Id = 0;
        }
        
        public DialogPage AddAnswer(string answerText, Action<ExtPlayer> callback)
        {
            Answers.Add(new DialogPageAnswer(answerText, p =>
            {
                CloseForPlayer(p, true);
                callback?.Invoke(p);
            }));
            
            return this;
        }
        
        public DialogPage AddCloseAnswer(string answerText = "До встречи")
        {
            Answers.Add(new DialogPageAnswer(answerText, p => CloseForPlayer(p, true)));
            
            return this;
        }

        internal void OnPlayerClosedDialog(ExtPlayer player)
        {
            if (OpenedPages.ContainsKey(player))
            {
                OpenedPages.Remove(player);
            }
        }
        public DialogPage AddAnswer(string answerText, DialogPage pageToRedirect)
        {
            var answer = new DialogPageAnswer(answerText, p =>
            {
                //SafeTrigger.ClientEvent(p, "questPeds:setDataDialog", pageToRedirect.GetSerializedData());
                CloseForPlayer(p);
                pageToRedirect.OpenForPlayer(p);
                //if (OpenedPages.ContainsKey(p)) OpenedPages.Remove(p);
                //OpenedPages.Add(p, pageToRedirect);
            });
            Answers.Add(answer);
            
            return this;
        }

        public void OpenForPlayer(ExtPlayer player)
        {
            if (OpenedPages.ContainsKey(player)) return;
            OpenedForPlayer?.Invoke(player);
            OpenedPages.Add(player, this);
            
            SafeTrigger.ClientEvent(player,"questPeds:openDialog", GetSerializedData());
        }
        
        public void CloseForPlayer(ExtPlayer player, bool fullClose = false)
        {
            if (OpenedPages.ContainsKey(player))
            {
                OpenedPages.Remove(player);
                SafeTrigger.ClientEvent(player,"questPeds:closeDialog", fullClose);
            }
        }

        public void OnPlayerSelectedAnswer(ExtPlayer player, int answerId)
        {
            Answers[answerId].Callback.Invoke(player);
            // else
            // {
            //     var redirectedPage = Answers.FirstOrDefault(a => a.PageToRedirect?.Id == pageId)?.PageToRedirect;
            //     redirectedPage?.Answers[answerId]?.Callback(player);
            // }
        }

        public string GetSerializedData()
        {
            var list = Answers.Select((t, i) => 
                new DialogPageAnswerDTO {Id = i, Text = t.Header}).ToList();
            var dialogData = JsonConvert.SerializeObject(new
            {
                id = Id,
                name = _name,
                desc = _role,
                text = IntroWords,
                answers = list,
            });
            
            return dialogData;
        }
    }

    internal struct DialogPageAnswerDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
        
        [JsonProperty("redirectPageId")]
        public int? RedirectPageId { get; set; }
    }
}