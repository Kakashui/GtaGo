using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.MP.Arena.Battles.Teamfight;
using Whistler.MP.Arena.Enums;
using Whistler.MP.Arena.Helpers;
using Whistler.MP.Arena.Interfaces;
using Whistler.MP.Arena.Lobbies;
using Whistler.MP.Arena.Locations;

namespace Whistler.MP.Arena.Battles
{
    internal class BattleManager : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(BattleManager));
        public static int IdCounter { get; set; }
        public static IReadOnlyDictionary<int, IArenaBattle> Battles => 
            _battles.ToDictionary(key => key.Id);
        private static List<IArenaBattle> _battles = new List<IArenaBattle>();

        public static event Action<IArenaBattle> BattleCreated;
        
        public BattleManager()
        {
            Main.PlayerPreDisconnect += OnPlayerPreDisconnect;
            var lobby1 = LobbyManager.CreateStrikeLobby(new StrikeLobbySettings
            {
                AvailableWeapon = WeaponHash.Revolver,
                BattleMode = StrikeBattleMode.DeathMatch,
                EntryBet = 100,
                LocationName = LocationName.Camp,
                MaxPlayers = 30
            });
            
            var lobby2 = LobbyManager.CreateStrikeLobby(new StrikeLobbySettings
            {
                AvailableWeapon = WeaponHash.Combatpdw,
                BattleMode = StrikeBattleMode.DeathMatch,
                EntryBet = 100,
                LocationName = LocationName.Island,
                MaxPlayers = 30
            });
            
            var lobby3 = LobbyManager.CreateStrikeLobby(new StrikeLobbySettings
            {
                AvailableWeapon = WeaponHash.Carbinerifle,
                BattleMode = StrikeBattleMode.DeathMatch,
                EntryBet = 100,
                LocationName = LocationName.Sawmill,
                MaxPlayers = 30
            });

            CreateStrikeBattle(lobby1);
            CreateStrikeBattle(lobby2);
            CreateStrikeBattle(lobby3);
        }

        public static void CreateBattle(IArenaLobby lobby)
        {
            switch (lobby)
            {
                case StrikeLobby concreteStrikeLobby:
                    CreateStrikeBattle(concreteStrikeLobby);
                    break;
                case RacingLobby concreteRacingLobby:
                    CreateRacingBattle(concreteRacingLobby);
                    break;
            }
        }

        private static void CreateStrikeBattle(StrikeLobby lobby)
        {
            IArenaBattle battle = lobby.Settings.BattleMode switch
            {
                StrikeBattleMode.DeathMatch => new StrikeDeathmatchBattle(lobby, LoadLocation(lobby)),
                StrikeBattleMode.GunGame => new StrikeGunGameBattle(lobby, LoadLocation(lobby)),
                _ => new TeamfightBattle(lobby, LoadLocation(lobby))
            };

            _battles.Add(battle);
            BattleCreated?.Invoke(battle);
        }

        private static void CreateRacingBattle(RacingLobby lobby)
        {
            
        }

        private static ArenaLocation LoadLocation(IArenaLobby lobby)
        {
            var location = ArenaLocationFactory.CreateLocation(lobby.LocationName);
            location.LoadForLobby(lobby);

            return location;
        }

        public static void OnPlayerDead(ExtPlayer victim, ExtPlayer killer, uint reason)
        {
            try
            {
                var killerBattle = GetPlayerBattle(killer, out var killerMemberModel);
                var victimBattle = GetPlayerBattle(victim, out var victimMemberModel);

                //self kill
                if (victimBattle != null && killerBattle == null)
                    victimBattle.InvokeBattleMemberKilled(victimMemberModel, victimMemberModel, reason);
                else if (ArenaBattleHelper.IsPlayersInSameBattle(victim, killer))
                    killerBattle.InvokeBattleMemberKilled(killerMemberModel, victimMemberModel, reason);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Unhandled exception catched at {nameof(OnPlayerDead)}: {ex}");
            }
        }

        public static void PlayerLeave(IBattleMember member)
        {
            try
            {
                member.Delete();
                LobbyManager.LeaveLobby(member.Player, member.CurrentBattle.Lobby);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Unhandled exception catched at {nameof(PlayerLeave)}: {ex}");
            }
        }

        private static void OnPlayerPreDisconnect(ExtPlayer player)
        {
            try
            {
                var battle = GetPlayerBattle(player, out var memberModel);

                battle?.OnBattleMemberDisconnected(memberModel);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Unhandled exception catched at {nameof(OnPlayerPreDisconnect)}: {ex}");
            }
        }
              
        public static IArenaBattle GetPlayerBattle(ExtPlayer player, out IBattleMember member)
        {
            var battle = _battles.FirstOrDefault(b 
                => b.Members.Any(m => m.Player == player));
            
            member = null;
            if (battle != null) member = battle.Members.FirstOrDefault(m => m.Player == player);

            return battle;
        }
    }
}