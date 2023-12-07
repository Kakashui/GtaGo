using System;
using System.Numerics;
using GTANetworkAPI;
using Whistler.ClothesCustom;
using Whistler.Core;
using Whistler.Entities;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.Jobs.ImpovableJobs;
using Whistler.SDK;

namespace Whistler.Docks
{
    internal class DockLoader
    {
        private readonly ExtPlayer _player;
        public int CurrentPayment { get; set; } = 0;
        
        public DockLoader(ExtPlayer player)
        {
            _player = player;
        }

        public void StartWorkingDay()
        {
            _player.Character.WorkID = 7;
            MainMenu.SendStats(_player);
        }
        
        public void StopWorkingDay()
        {
            SafeTrigger.ClientEvent(_player, "dockLoader:stopedWorking");
            _player.Character.WorkID = 0;
            MainMenu.SendStats(_player);
        }
        
    }
}