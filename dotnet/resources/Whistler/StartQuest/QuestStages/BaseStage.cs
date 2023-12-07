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
        protected virtual void StartStage(ExtPlayer player)
        {
        }
        protected virtual void FinishStage(ExtPlayer player)
        {
        }
        protected virtual void StopStage(ExtPlayer player)
        {
        }
        public void QuestStart(ExtPlayer player)
        {
            StartStage(player);
        }
        public void QuestFinish(ExtPlayer player)
        {
            FinishStage(player);
            player.Character.QuestStage++;
            StartQuestManager.TryingStartNextQuest(player);
        }
        public void BreakStage(ExtPlayer player)
        {
            StopStage(player);
            StartQuestManager.TryingStartNextQuest(player);
        }
    }
}
