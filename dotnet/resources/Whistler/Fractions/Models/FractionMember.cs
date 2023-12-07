using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Common.Interfaces;
using Whistler.Core.Character;
using Whistler.Entities;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Fractions.Models
{
    internal class FractionMember : IMember
    {
        public int PlayerUUID { get; set; }
        public int Rank { get; set; }

        public FractionMember(Fraction fraction, int playerUUID, int level)
        {
            PlayerUUID = playerUUID;
            Rank = level;
            MySQL.Query(
                "UPDATE `characters` " +
                "SET `fraction` = @prop1, `fractionlvl` = @prop2 " +
                "WHERE `uuid` = @prop0",
                PlayerUUID, fraction.Id, Rank);
            var player = Trigger.GetPlayerByUuid(PlayerUUID);
            if (player != null)
            {
                player.Character.FractionID = fraction.Id;
                player.Character.FractionLVL = Rank;
            }
            MainMenu.SendStats(player);
        }
        public FractionMember(int playerUUID, int level)
        {
            PlayerUUID = playerUUID;
            Rank = level;
        }
        public void ChangeRank(int newLevel)
        {
            Rank = newLevel;
            MySQL.Query("UPDATE `characters` SET `fractionlvl` = @prop0 WHERE `uuid` = @prop1", Rank, PlayerUUID);
            ExtPlayer target = Trigger.GetPlayerByUuid(PlayerUUID);
            if (target == null) return;

            target.Character.FractionLVL = Rank;
            MainMenu.SendStats(target);
        }
    }
}
