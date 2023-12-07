using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Common.Interfaces;
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
            var player = Main.GetPlayerByUUID(PlayerUUID);
            if (player != null)
            {
                player.GetCharacter().FractionID = fraction.Id;
                player.GetCharacter().FractionLVL = Rank;
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
            var character = Main.GetCharacterByUUID(PlayerUUID);

            if (character != null)
            {
                character.FractionLVL = Rank;
                var player = Main.GetPlayerByUUID(PlayerUUID);
                if (player != null)
                {
                    MainMenu.SendStats(player);
                }
            }
        }
    }
}
