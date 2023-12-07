using Whistler.Helpers;

namespace Whistler.Houses
{
    /// <summary>
    /// Жилец в доме
    /// </summary>
    internal class Roommate
    {
        /// <summary>
        /// UUID персонажа
        /// </summary>
        public int CharacterUUID { get; set; }
        
        /// <summary>
        /// Имеет ли доступ к сейфу
        /// </summary>
        public bool HasSafeAccess { get; set; }

        /// <summary>
        /// Имеет ли доступ к гардеробу
        /// </summary>
        public bool HasWardrobeAccess { get; set; }

        /// <summary>
        /// Имеет ли доступ к гаражу
        /// </summary>
        public bool HasGarageAccess { get; set; }
        
        /// <summary>
        /// Количество денег внесенных игроком за проживание
        /// </summary>
        private int Balance { get; set; }
        public Roommate(int uuid)
        {
            CharacterUUID = uuid;
        }

        public Roommate()
        {
            
        }

        public bool ChangeBalance(int amount)
        {
            if (Balance + amount < 0)
                return false;
            Balance += amount;
            //UpdatePlayerData();
            return true;
        }
        private void UpdatePlayerData()
        {
            var player = Main.GetPlayerByUUID(CharacterUUID);
            if (player.IsLogged())
            {
                player.TriggerCefEvent($"smartphone/bankPage/setHomeRentBalance", Balance);
            }
        }
    }
}