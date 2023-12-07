using System;
using GTANetworkAPI;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.MP.Arena.Enums;
using Whistler.MP.Arena.Helpers;
using Whistler.MP.Arena.Interfaces;
using Whistler.MP.Arena.Lobbies;
using Whistler.MP.Arena.UI;
using Whistler.SDK;

namespace Whistler.MP.Arena.Events
{
    /// <summary>
    /// Срабатывает при клике смены игроком голоса за новую карту 
    /// </summary>
    /// <param name="voter">Проголосовавший игрок</param>
    /// <param name="mapName">Название выбранной карты</param>
    internal delegate void LocationChangeVoteHandler(ExtPlayer voter, string mapName);
    
    /// <summary>
    /// Срабатывает при попытке создания игроком лобби
    /// </summary>
    /// <param name="creator">Игрок-создатель лобби</param>
    /// <param name="settings">Настройки лобби</param>
    internal delegate void LobbyCreateHandler(ExtPlayer creator, StrikeLobbySettings settings);
    
    /// <summary>
    /// Срабатывает при попытке присоединения игрока в лобби
    /// </summary>
    /// <param name="joiner">Игрок присоединяющийся в лобби</param>
    /// <param name="lobby">Лобби из спика куда присоединяется игрок</param>
    /// <param name="team">Команда</param>
    internal delegate void LobbyJoinHandler(ExtPlayer joiner, IArenaLobby lobby, TeamName team);
    
    /// <summary>
    /// Срабатывает при попытке старта лобби
    /// </summary>
    /// <param name="starter">Игрок запустивший лобби (может быть не владельцем)</param>
    /// <param name="lobby">Лобби</param>
    internal delegate void LobbyStartHandler(ExtPlayer starter, IArenaLobby lobby);
    
    /// <summary>
    /// Срабатывает когда игрок пытается покинуть лобби
    /// </summary>
    /// <param name="leaver">Игрок</param>
    /// <param name="lobby">Лобби</param>
    internal delegate void LobbyLeaveHandler(ExtPlayer leaver, IArenaLobby lobby);
    
    /// <summary>
    /// Срабатывает когда владелец лобби пытается кикнуть игрока
    /// </summary>
    /// <param name="victimName">Имя игрока которого кикают</param>
    /// <param name="lobby">Лобби</param>
    internal delegate void LobbyKickHandler(ExtPlayer leaver, string victimName, IArenaLobby lobby);
    
    internal class StrikeLobbyEvents : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(StrikeLobbyEvents));
        
        public static event LocationChangeVoteHandler MapVoteChanged;
        public static event LobbyCreateHandler LobbyCreated;
        public static event LobbyJoinHandler LobbyJoined;
        public static event LobbyStartHandler LobbyStarted;
        public static event LobbyLeaveHandler LobbyLeaved;
        public static event LobbyKickHandler LobbyKicked;
        
        private const string MapVoteChangedEvent = "ARENA::CHOOSE::MAP::NAME::CLIENT";
        private const string LobbyAddedEvent = "ARENA::ADD::LOBBY::CLIENT";
        private const string LobbyJoinedEvent = "ARENA::JOIN::LOBBY::CLIENT";
        private const string LobbyStartedEvent = "ARENA::START::LOBBY::CLIENT";
        private const string LobbyLeavedEvent = "ARENA::LOBBY::LEAVE::CLIENT";
        private const string LobbyKickedEvent = "ARENA::LOBBY::KICK::CLIENT";
        private const string ArenaMenuClosed = "ARENA:CLOSE";

        [RemoteEvent(MapVoteChangedEvent)]
        public void OnMapVoteChanged(ExtPlayer player, string mapName)
        {
            try
            {
                MapVoteChanged?.Invoke(player, mapName);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Exception catched at {nameof(OnMapVoteChanged)}: {ex}");
            }
        }
        
        [RemoteEvent(LobbyAddedEvent)]
        public void OnLobbyAdded(ExtPlayer player, string mode, string map, int maxPlayers, string weapon, int rounds, int bet)
        {
            try
            {
                var settings = new StrikeLobbySettings
                {
                    EntryBet = bet,
                    TotalRounds = rounds,
                    MaxPlayers = maxPlayers,
                    LocationName = Enum.Parse<LocationName>(map),

                    AvailableWeapon = weapon == "Random"
                        ? ArenaLobbyHelper.GetRandomAvailableWeapon()
                        : ArenaLobbyHelper.GetWeaponHashByName(weapon),
                    
                    BattleMode = mode switch
                    {
                        "Gun game" => StrikeBattleMode.GunGame,
                        "Death match" => StrikeBattleMode.DeathMatch,
                        _ => StrikeBattleMode.TeamFight
                    }
                };
                
                LobbyCreated?.Invoke(player, settings);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Exception catched at {nameof(OnLobbyAdded)}: {ex}");
            }
        }
        
        [RemoteEvent(LobbyJoinedEvent)]
        public void OnLobbyJoined(ExtPlayer player, string teamName, int lobbyId)
        {
            try
            {
                // Если лобби есть на сервере
                if (LobbyManager.Lobbies.TryGetValue(lobbyId, out var lobby))
                {
                    Enum.TryParse(teamName, true, out TeamName team);
                    LobbyJoined?.Invoke(player, lobby, team);
                }
                else throw new ArenaException("Lobby cannot be find");
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Exception catched at {nameof(OnLobbyJoined)}: {ex}");
                Notify.SendError(player, "error");
            }
        }
        
        [RemoteEvent(LobbyStartedEvent)]
        public void OnLobbyStarted(ExtPlayer player, int lobbyId)
        {
            try
            {
                // Если лобби есть на сервере
                if (LobbyManager.Lobbies.TryGetValue(lobbyId, out var lobby))
                    LobbyStarted?.Invoke(player, lobby);
                
                else throw new ArenaException("Lobby cannot be find");
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Exception catched at {nameof(OnLobbyStarted)}: {ex}");
                Notify.SendError(player, "error");
            }
        }
        
        [RemoteEvent(LobbyLeavedEvent)]
        public void OnLobbyLeaved(ExtPlayer player, int lobbyId)
        {
            try
            {
                // Если лобби есть на сервере
                if (LobbyManager.Lobbies.TryGetValue(lobbyId, out var lobby))
                    LobbyLeaved?.Invoke(player, lobby);
                
                else throw new ArenaException("Lobby cannot be find");
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Exception catched at {nameof(OnLobbyLeaved)}: {ex}");
                Notify.SendError(player, "error");
            }
        }
        
        [RemoteEvent(LobbyKickedEvent)]
        public void OnLobbyKicked(ExtPlayer player, string victimName, int lobbyId)
        {
            try
            {
                // Если лобби есть на сервере
                if (LobbyManager.Lobbies.TryGetValue(lobbyId, out var lobby))
                    LobbyKicked?.Invoke(player, victimName, lobby);
                
                else throw new ArenaException("Lobby cannot be find");
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Exception catched at {nameof(OnLobbyKicked)}: {ex}");
                Notify.SendError(player, "error");
            }
        }
        
        [RemoteEvent(ArenaMenuClosed)]
        public void OnArenaMenuClosed(ExtPlayer player)
        {
            try
            {
                ArenaUiUpdateHandler.UnsubscribePlayer(player);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Exception catched at {nameof(OnArenaMenuClosed)}: {ex}");
                Notify.SendError(player, "error");
            }
        }
    }
}