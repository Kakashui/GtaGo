using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Entities;

namespace Whistler.MP.OrgBattle.BattleModels
{
    abstract class BattleBase
    {
        protected Action<int> EndBattleAction = null;
        protected Action StartBattleAction = null;
        internal BattleBase AddStartAction(Action startAction)
        {
            StartBattleAction = startAction;
            return this;
        }
        internal BattleBase AddEndAction(Action<int> endAction)
        {
            EndBattleAction = endAction;
            return this;
        }
        public void BattleStart()
        {
            StartBattleAction?.Invoke();
            StartBattle();
        }
        protected abstract void StartBattle();
        protected void BattleEnd(int winner)
        {
            EndBattleAction?.Invoke(winner);
            OrgBattleManager.RemoveBattle(this);
        }
        internal abstract bool PlayerDeath(ExtPlayer player, ExtPlayer killer, uint weapon);
        internal abstract void PlayerDisconnected(ExtPlayer player);
    }
}
