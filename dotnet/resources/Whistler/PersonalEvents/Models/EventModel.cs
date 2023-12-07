using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using Whistler.Core.Character;
using Whistler.Entities;
using Whistler.PersonalEvents.Achievements;
using Whistler.PersonalEvents.Achievements.AchieveModels;
using Whistler.PersonalEvents.Achievements.Models;
using Whistler.PersonalEvents.Contracts;
using Whistler.PersonalEvents.Contracts.Models;
using Whistler.PersonalEvents.DTO;
using Whistler.SDK;

namespace Whistler.PersonalEvents.Models
{
    class EventModel
    {
        public Dictionary<AchieveNames, AchieveProgress> MyAchieves { get; set; }
        private bool _achieveIsChanged = false;
        public ContractInfo Contracts { get; set; }
        public EventModel()
        {
            MyAchieves = new Dictionary<AchieveNames, AchieveProgress>();
            Contracts = new ContractInfo();
        }
        public EventModel(Dictionary<AchieveNames, AchieveProgress> myAchieves, ContractInfo contracts)
        {
            MyAchieves = myAchieves;
            Contracts = contracts;
        }
        public EventModel(DataRowCollection rowCollection, ContractInfo contracts)
        {
            Contracts = contracts;
            MyAchieves = new Dictionary<AchieveNames, AchieveProgress>();
            foreach (DataRow row in rowCollection)
            {
                MyAchieves.Add((AchieveNames)Convert.ToInt32(row["achieveName"]), new AchieveProgress(row));
            }
        }
        public string GetSerializeDTOString()
        {
            Dictionary<int, AchieveProgressDTO> achievesDTO = new Dictionary<int, AchieveProgressDTO>();
            foreach (var item in MyAchieves)
            {
                achievesDTO.Add((int)item.Key, new AchieveProgressDTO(item.Value, item.Key));
            }
            return JsonConvert.SerializeObject(achievesDTO);
        }
        public AchieveProgress AchieveProgress(ExtPlayer player, AchieveParams achieveParams, int progress)
        {

            if (MyAchieves.TryGetValue(achieveParams.AchieveName, out AchieveProgress achieve))
            {
                if (achieve.DateCompleted < DateTime.Now)
                    return null;
                achieve.AddProgress(progress);
                _achieveIsChanged = true;
            }
            else
            {
                achieve = new AchieveProgress(progress);
                MyAchieves.Add(achieveParams.AchieveName, achieve);
                MySQL.Query("INSERT INTO `achievements` (currentLevel, givenReward, dateCompleted, achieveName, uuid) " +
                    "VALUES (@prop0, @prop1, @prop2, @prop3, @prop4)",
                    achieve.CurrentLevel,
                    achieve.GivenReward,
                    MySQL.ConvertTime(achieve.DateCompleted),
                    (int)achieveParams.AchieveName,
                    player.Character.UUID);
            }
            if (achieve.CurrentLevel >= achieveParams.MaxLevel)
            {
                achieve.DateCompleted = DateTime.Now;
                Save(player.Character.UUID, achieveParams.AchieveName);
            }
            return achieve;
        }

        public void Save(int uuid)
        {
            if (_achieveIsChanged)
            {
                _achieveIsChanged = false;
                foreach (var achieve in MyAchieves)
                {
                    if (achieve.Value._isChanged)
                    {
                        achieve.Value._isChanged = false;
                        MySQL.Query("UPDATE `achievements` SET " +
                            "currentLevel = @prop0, " +
                            "givenReward = @prop1, " +
                            "dateCompleted = @prop2 " +
                            "WHERE uuid = @prop3 AND " +
                            "achieveName = @prop4",
                            achieve.Value.CurrentLevel,
                            achieve.Value.GivenReward,
                            MySQL.ConvertTime(achieve.Value.DateCompleted),
                            uuid,
                            (int)achieve.Key);
                    }
                }
            }
            Contracts.Save(ContractTypes.Single, uuid);
        }
        public void Save(int uuid, AchieveNames achieveName)
        {
            var achieveProgress = MyAchieves[achieveName];
            achieveProgress._isChanged = false;
            MySQL.Query("UPDATE `achievements` SET " +
                "currentLevel = @prop0, " +
                "givenReward = @prop1, " +
                "dateCompleted = @prop2 " +
                "WHERE uuid = @prop3 AND " +
                "achieveName = @prop4",
                achieveProgress.CurrentLevel,
                achieveProgress.GivenReward,
                MySQL.ConvertTime(achieveProgress.DateCompleted),
                uuid,
                (int)achieveName);
        }

        public AchieveProgress GiveReward(ExtPlayer player, AchieveParams achieveParams)
        {
            if (player.Character.EventModel.MyAchieves.TryGetValue(achieveParams.AchieveName, out AchieveProgress achieve))
            {
                if (achieve.CurrentLevel < achieveParams.MaxLevel)
                {
                    Notify.SendError(player, "options_program_20");
                    return null;
                }
                if (achieve.GivenReward)
                {
                    Notify.SendError(player, "options_program_21");
                    return null;
                }
                achieve.GivenReward = true;
                Save(player.Character.UUID, achieveParams.AchieveName);
                foreach (var reward in achieveParams.Rewards)
                {
                    reward.GiveReward(player, achieveParams.AchieveName.ToString());
                }
                return achieve;
            }
            return null;
        }
    }
}