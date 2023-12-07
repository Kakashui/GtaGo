using System.Collections.Generic;
using GTANetworkAPI;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.MoneySystem;
using Whistler.MP.Arena.Enums;
using Whistler.MP.Arena.Interfaces;
using Whistler.MP.Arena.Lobbies.Implementations;
using Whistler.SDK;

namespace Whistler.MP.Arena.Lobbies
{
    internal class StrikeLobby : IArenaLobby
    {
        public int Id { get; set; }
        public StrikeLobbySettings Settings { get; }
        public List<IArenaLobbyMember> Members { get; }

        public LobbyState CurrentState { get; set; }
        
        public LocationName LocationName => Settings.LocationName;

        public IArenaLobbyMember Leader { get; set; }
        
        public bool AutoStarted { get; set; }

        /// <summary>
        /// Сумма всех взносов всех игроков
        /// </summary>
        public int TotalContribution { get; set; }
        
        public bool TryAddMember(ExtPlayer player, TeamName team)
        {
            if (Members.Count >= Settings.MaxPlayers) return false;
            
            if (!Wallet.MoneySub(player.Character, Settings.EntryBet, "Money_Entrybet"))
            {
                Notify.SendError(player, "arena_dm_38");
                return false;
            }
            TotalContribution += Settings.EntryBet;
            
            if (player.HasData("kl:enabled") && player.GetData<bool>("kl:enabled"))
            {
                Notify.SendError(player, "arena_dm_39");
                return false;
            }
            
            Members.Add(new ArenaStrikeMember(player, this, team));
            
            return true;
        }

        public void DeleteMember(IArenaLobbyMember member)
        {
            Members.Remove(member);
            TotalContribution -= Settings.EntryBet;
        }

        /// <summary>
        /// Используйте <see cref="LobbyManager"/>
        /// </summary>
        public StrikeLobby(int id, IArenaLobbyMember creator, StrikeLobbySettings settings)
        {
            Id = id;
            Settings = settings;
            Leader = creator;
            Members = new List<IArenaLobbyMember> {Leader};
        }
        
        /// <summary>
        /// Используйте <see cref="LobbyManager"/>
        /// </summary>
        public StrikeLobby(int id, StrikeLobbySettings settings)
        {
            Id = id;
            AutoStarted = true;
            Settings = settings;
            Members = new List<IArenaLobbyMember>();
        }
    }
}