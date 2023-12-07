using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using Whistler.Entities;
using Whistler.SDK.CustomColShape.DTO;

namespace Whistler.SDK
{
    public abstract class BaseColShape
    {
        protected uint _dimension;
        public event BaseColShapeEvent OnEntityEnterColShape;
        public event BaseColShapeEvent OnEntityExitColShape;
        public delegate void BaseColShapeEvent(PlayerGo player);

        public void EnterColShape(PlayerGo player)
        {
            OnEntityEnterColShape?.Invoke(player);
        }
        public void ExitColShape(PlayerGo player)
        {
            OnEntityExitColShape?.Invoke(player);
        }

        internal abstract BaseDTO GetDTO(int id);
    }
}
