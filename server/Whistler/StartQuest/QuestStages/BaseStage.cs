using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;

namespace Whistler.StartQuest.QuestStages
{
    abstract class BaseStage
    {
        protected static WhistlerLogger _logger = new WhistlerLogger(typeof(BaseStage));
        public StartQuestNames StageName { get; set; } = StartQuestNames.Invalid;
        protected virtual void StartStage(PlayerGo player)
        {
        }
        protected virtual void FinishStage(PlayerGo player)
        {
        }
        protected virtual void StopStage(PlayerGo player)
        {
        }
        public void QuestStart(PlayerGo player)
        {
            StartStage(player);
        }
        public void QuestFinish(PlayerGo player)
        {
            FinishStage(player);
            player.GetCharacter().QuestStage++;
            StartQuestManager.TryingStartNextQuest(player);
        }
        public void BreakStage(PlayerGo player)
        {
            StopStage(player);
            StartQuestManager.TryingStartNextQuest(player);
        }
    }
}
