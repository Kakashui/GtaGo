using System;
using System.Data;
using GTANetworkAPI;
using Whistler;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace ServerGo.Casino.Business
{
    internal class CashBox
    {
        public long Amount { get; private set; }
        private int _casinoId;
        private DateTime _minusDateTime;
        public CashBoxState State { get; private set; } = CashBoxState.Safe;
        public long MafiaShare => (long)(Amount * 0.3); // money that mobs can steal from casino

        public CashBox(int casinoId)
        {
            _casinoId = casinoId;
            var table = MySQL.QueryRead("SELECT * FROM `casino` WHERE id = @prop0", casinoId);
            foreach (DataRow Row in table.Rows)
            {
                Games.Roulette.RouletteGame.MaxWin = Convert.ToInt32(Row["maxWinOfBet"]);
                Amount = Convert.ToInt64(Row["amount"]);
                if (Row["minusDate"] == DBNull.Value) return;
                else
                {
                    State = CashBoxState.UnderThreat;
                    _minusDateTime = DateTime.Parse(Row["minusDate"].ToString());
                }
            }
        }
        
        public void Withdraw(int dollars, ExtPlayer player = null)
        {
            Amount -= dollars;
            UpdateDbAmount();
            if (player != null)
                GameLog.Casino(nameof(Withdraw), dollars, player.Character.FullName, DateTime.Now);
        }

        public void Charge(int dollars, ExtPlayer player = null)
        {
            Amount += dollars - (long) (dollars * CasinoManager.StateShare);
            UpdateDbAmount();
            if (player != null)
                GameLog.Casino(nameof(Charge), dollars, player.Character.FullName, DateTime.Now);
        }

        private void UpdateDbAmount()
        {
            MySQL.Query("UPDATE casino SET amount = @prop0 WHERE id = @prop1", Amount, _casinoId);
        }

        public void Reset()
        {
            Amount = 0;
            ResetMinusDate();
            UpdateDbAmount();
        }
        private void SetDbDMinusDate() =>
            MySQL.Query("UPDATE casino SET minusDate = @prop0  WHERE id = @prop1", MySQL.ConvertTime(DateTime.Now), _casinoId);

        private void ResetMinusDate() =>
            MySQL.Query("UPDATE casino SET minusDate = @prop0 WHERE id = @prop1", DBNull.Value, _casinoId);
        
        public void PayDayCallback(int tax)
        {
            Withdraw(tax);
            if (Amount < 0 && State == CashBoxState.Safe) // if first time
            {
                State = CashBoxState.UnderThreat;
                SetDbDMinusDate();
            }

            if (Amount > 0 && State == CashBoxState.UnderThreat)
            {
                State = CashBoxState.Safe;
                ResetMinusDate();
            }
            
            if (Amount < 0 && State == CashBoxState.UnderThreat)
            {
                if (DateTime.Now.DayOfYear - _minusDateTime.DayOfYear <= 1 
                    && DateTime.Now.Hour - _minusDateTime.Hour >= 0) // if the time has come
                {
                    CasinoManager.FindFirstCasino().OnOwnerLostBusiness();
                }
            }
        }
    }
}