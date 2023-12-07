using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Whistler.Domain.Fractions;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;

namespace Whistler.Fractions
{
    class AccessManager
    {
        public static bool CheckAccess(ExtPlayer player, AccessType accessType)
        {
            if (!player.IsLogged())
                return false;
            var character = player.Character;
            var access = GetOrCreateAccess(character.FractionID, character.FractionLVL);
            return access.AccessList.Contains(accessType);
        }

        public static void AddAccess(int fractionId, int fractionLvl, AccessType accessType)
        {
            var access = GetOrCreateAccess(fractionId, fractionLvl);
            if (!access.AccessList.Contains(accessType))
                access.AccessList.Add(accessType);
        }

        public static void RemoveAccess(int fractionId, int fractionLvl, AccessType accessType)
        {
            var access = GetOrCreateAccess(fractionId, fractionLvl);
            if (access.AccessList.Contains(accessType))
                access.AccessList.Remove(accessType);
        }

        public static List<Access> GetFractionAccesses(int fractionId)
        {
            using (var context = DbManager.FractionContext)
            {
                var accesses = context.Accesses.Where(item => item.FractionId == fractionId).ToList();
                return accesses;
            }
        }
        private static Access GetOrCreateAccess(int fractionId, int fractionLvl)
        {
            using (var context = DbManager.FractionContext)
            {
                var access = context.Accesses.FirstOrDefault(item => item.FractionId == fractionId && item.FractionRank == fractionLvl);
                if (access == null)
                {
                    access = new Access()
                    {
                        FractionId = fractionId,
                        FractionRank = fractionLvl,
                        AccessList = new List<AccessType>()
                    };
                    context.Accesses.Add(access);
                }
                return access;
            }
        }
    }
}
