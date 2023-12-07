using System;
using System.Linq;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Internal;
using Whistler.Entities;
using Whistler.MP.Arena.Configs;
using Whistler.MP.Arena.Enums;
using Whistler.MP.Arena.Interfaces;
using Whistler.MP.Arena.Lobbies;

namespace Whistler.MP.Arena.Helpers
{
    internal static class ArenaLobbyHelper
    {
        public static WeaponHash GetWeaponHashByName(string weaponName) => Enum.Parse<WeaponHash>(weaponName, true);

        public static WeaponHash GetRandomAvailableWeapon() => 
            StrikeConfig.AvailableForSelectWeaponsForLoby[new Random()
                .Next(0, StrikeConfig.AvailableForSelectWeaponsForLoby.Length)];

        public static string GetModeName(StrikeBattleMode mode) =>
            mode switch
            {
                StrikeBattleMode.GunGame => "Gun game",
                StrikeBattleMode.DeathMatch => "Death match",
                _ => "Team fight"
            };

        public static bool IsPlayerInAnyLobby(ExtPlayer player) => 
            LobbyManager.Lobbies.Any(l => l.Value.Members.Any(m => m.Player == player));

        public static IArenaLobby GetPlayersLobbyOrDefault(ExtPlayer player) => 
            LobbyManager.Lobbies.FirstOrDefault(l => l.Value.Members.Any(m => m.Player == player)).Value;
        
    }
}