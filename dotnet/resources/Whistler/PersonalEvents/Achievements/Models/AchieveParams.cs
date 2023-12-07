using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.PersonalEvents.Achievements.AchieveModels;
using Whistler.PersonalEvents.DTO;
using Whistler.PersonalEvents.Models.Rewards;
using Whistler.SDK;

namespace Whistler.PersonalEvents.Achievements.Models
{
    class AchieveParams
    {
        public bool IsActive { get; set; }
        public bool IsHidden { get; set; }
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public string Desc { get; set; }
        public int MaxLevel { get; set; }
        public AchieveNames AchieveName { get; set; }
        public PlayerActions PlayerAction { get; set; }
        public string PlayerActionName => PlayerAction.ToString();
        public List<RewardBase> Rewards { get; set; }
        public bool IsClient { get; set; }
        public AchieveParams(bool isActive, bool isHidden, string name, string shortDesc, string desc, int maxLevel, AchieveNames achieveName, PlayerActions playerActions, List<RewardBase> rewards, bool isClient)
        {
            IsActive = isActive;
            IsHidden = isHidden;
            Name = name;
            ShortDesc = shortDesc;
            Desc = desc;
            MaxLevel = maxLevel;
            PlayerAction = playerActions;
            Rewards = rewards;
            EventManager.AddEvent(PlayerAction, AchieveProgress);
            AchieveName = achieveName;
            IsClient = isClient;
        }

        public void AchieveProgress(ExtPlayer player, int progress)
        {
            AchieveProgress achieve = player.Character.EventModel.AchieveProgress(player, this, progress);
            if (achieve != null)
                UpdateAchieve(player, achieve);
        }

        public void GiveReward(ExtPlayer player)
        {
            var achieve = player.Character.EventModel.GiveReward(player, this);
            if (achieve != null)
                UpdateAchieve(player, achieve);
        }

        public void UpdateAchieve(ExtPlayer player, AchieveProgress achieve)
        {
            SafeTrigger.ClientEvent(player,"personalEvents:updateAchieve", JsonConvert.SerializeObject(new AchieveProgressDTO(achieve, AchieveName)));
        }
    }
}
