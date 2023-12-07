using System;
using GTANetworkAPI;
using Whistler.ClothesCustom;
using Whistler.Core;
using Whistler.Helpers;
using Whistler.Jobs.ImpovableJobs;

namespace Whistler.Docks
{
    internal class DockLoader
    {
        private readonly Player _player;
        public int CurrentPayment { get; set; }
        
        public DockLoader(Player player)
        {
            _player = player;
        }

        public void StartWorkingDay()
        {
            _player.GetCharacter().WorkID = 7;
        }
        
        public void StopWorkingDay()
        {
            _player.TriggerEvent("dockLoader:stopedWorking");
            _player.GetCharacter().WorkID = 0;
        }
        
    }
}