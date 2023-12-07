using GTANetworkAPI;
using Whistler.MP.Arena.Enums;

namespace Whistler.MP.Arena.Lobbies
{
    /// <summary>
    /// Настройки лобби
    /// </summary>
    internal class StrikeLobbySettings
    {
        public StrikeBattleMode BattleMode { get; set; }

        public LocationName LocationName { get; set; }

        public int MaxPlayers { get; set; }

        /// <summary>
        /// Количество раундов
        /// </summary>
        public int TotalRounds { get; set; }

        /// <summary>
        /// Начальный взнос за вход в комнату
        /// </summary>
        public int EntryBet { get; set; }

        public WeaponHash AvailableWeapon { get; set; }
    }
}