using System;
using GTANetworkAPI;
using Whistler;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;
using Player = GTANetworkAPI.Player;

namespace ServerGo.Casino.Business
{
    /// <summary>
    /// Represents casino room with unique Dimension
    /// </summary>
    internal class CasinoRoom
    {
        public uint Dimension { get; }
        private readonly int _globalBizId;
        public CasinoRoom(int bizId)
        {
            Dimension = 0;
            _globalBizId = bizId;
            InitColshapes();
        }
        
        public void LetPlayerIn(ExtPlayer player)
        {
            SafeTrigger.UpdateDimension(player,  Dimension);
            player.ChangePosition(CasinoManager.RoomEnterPoint);
            SafeTrigger.SetData(player, "CURRENTCASINO_ID", _globalBizId);
        }

        private void InitColshapes()
        {
            InteractShape.Create(CasinoManager.CashBoxPoint, 2, 3, Dimension)
                .AddInteraction((player) =>
                {
                    CasinoManager.FindCasinoByBizId(_globalBizId)
                            .OnPlayerCashBoxPressed(player);
                });

        }
        public void LetPlayerOut(ExtPlayer player)
        {
            SafeTrigger.UpdateDimension(player,  0);
            var point = BusinessManager.BizList[_globalBizId].EnterPoint;
            player.ChangePosition(new Vector3(point.X, point.Y, point.Z + 1));
            player.ResetData("CURRENTCASINO_ID");
        }
    }
}