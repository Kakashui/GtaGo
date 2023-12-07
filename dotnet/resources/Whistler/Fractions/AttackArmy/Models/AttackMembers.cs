using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core;
using Whistler.Helpers;
using Whistler.MoneySystem;
using Whistler.SDK;

namespace Whistler.Fractions.AttackArmy.Models
{
    class AttackMembers
    {
        private static List<int> _crimeBrokenStation { get; set; }
        private static List<int> _crimeLoadItems { get; set; }
        private static List<int> _armyRepairStation { get; set; }
        public AttackMembers()
        {
            _crimeBrokenStation = new List<int>();
            _crimeLoadItems = new List<int>();
            _armyRepairStation = new List<int>();
        }
        public void SetCrimeBrokenStation(List<int> members)
        {
            _crimeBrokenStation = members;
        }
        public void SetArmyRepairStation(List<int> members)
        {
            _armyRepairStation = members;
        }
        public void AddCrimeLoadItem(int member)
        {
            _crimeLoadItems.Add(member);
        }
        public void Payment()
        {
            foreach (var item in _crimeBrokenStation)
            {
                Payment(item, Constants.CrimeStationBrokenMoney, "elstation:prize:1".Translate(Constants.CrimeStationBrokenMoney), "Награда за разрушение электростанции");
            }
            foreach (var item in _crimeLoadItems)
            {
                Payment(item, Constants.CrimeLoadItemsMoney, "elstation:prize:2".Translate(Constants.CrimeLoadItemsMoney), "Награда за загрузку оружия");
            }
            foreach (var item in _armyRepairStation)
            {
                Payment(item, Constants.ArmyRepairStationMoney, "elstation:prize:3".Translate(Constants.ArmyRepairStationMoney), "Награда за ремонт электростанции");
            }
        }
        private void Payment(int member, int money, string message, string reason)
        {
            Wallet.MoneyAdd(BankManager.GetAccountByUUID(member), money, reason);
            var player = Trigger.GetPlayerByUuid(member);
            if (player.IsLogged())
            {
                Notify.SendSuccess(player, message);
            }
        }
    }
}
