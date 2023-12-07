using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.MoneySystem.Models;
using Whistler.SDK;

namespace Whistler.Houses
{
    /// <summary>
    /// Жилец в доме
    /// </summary>
    internal class Roommate
    {
        /// <summary>
        /// Модель игрока
        /// </summary>
        [JsonIgnore]
        public ExtPlayer Character { get; set; }
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
        
        public Roommate(int uuid)
        {
            CharacterUUID = uuid;
        }

        public void SetCharacter(ExtPlayer player)
        {
            Character = player;
        }
    }
}