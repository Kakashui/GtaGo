using System;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.GUI.Tips;
using Whistler.SDK;

namespace Whistler.Core.QuestPeds
{
    internal class QuestPed
    {
        [JsonProperty("id")]
        public int Id { get; }
        
        [JsonProperty("name")]
        public string Name { get; }
        
        [JsonProperty("hash")]
        public uint Hash { get; }
        
        [JsonProperty("position")]
        public Vector3 Position { get; }

        [JsonProperty("heading")]
        public float Heading { get; }
        
        [JsonProperty("dimension")]
        public uint Dimension { get; }
        
        public event Action<ExtPlayer, QuestPed> PlayerInteracted;
        public string Role { get; }

        public QuestPed(PedHash hash, Vector3 position, string name, string role, float heading = 0, uint dimension = 0, int interactionRange = 1)
        {
            Id = QuestPedManager.QuestPeds.Count;
            Position = position;
            Hash = (uint) hash;
            Dimension = dimension;
            Heading = heading;
            Name = name;
            Role = role;
            QuestPedManager.QuestPeds.Add(this);
            InteractShape.Create(Position, interactionRange, 2)
                .AddInteraction(OnPlayerInteractedWithPed, "Чтобы начать диалог")
                .AddOnEnterColshapeExtraAction(ShowTip);
            NAPI.TextLabel.CreateTextLabel(Name, Position + new Vector3(0, 0, 1.2f), 4, 1, 4, new Color(255, 255, 255));
        }
        public QuestPed(QuestPedParamModel questPedParamModel)
        {
            Id = QuestPedManager.QuestPeds.Count;
            Position = questPedParamModel.Position;
            Hash = (uint)questPedParamModel.Hash;
            Dimension = questPedParamModel.Dimension;
            Heading = questPedParamModel.Heading;
            Name = questPedParamModel.Name;
            Role = questPedParamModel.Role;
            QuestPedManager.QuestPeds.Add(this);
            InteractShape.Create(Position, questPedParamModel.Range, 2)
                .AddInteraction(OnPlayerInteractedWithPed, "Чтобы начать диалог")
                .AddOnEnterColshapeExtraAction(ShowTip);
            NAPI.TextLabel.CreateTextLabel(Name, Position + new Vector3(0, 0, 1.2f), 4, 1, 4, new Color(255, 255, 255));
        }

        private static void ShowTip(ColShape _, ExtPlayer player)
        {
            Tip.SendTip(player, "Квестовые персонажи будут встречать вас на протяжении всей игры на сервере. Нажмите Е чтобы начать диалог");
        }
        
        private void OnPlayerInteractedWithPed(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player,"questPeds:interacted", Id);
            PlayerInteracted?.Invoke(player, this);
        }
    }
}