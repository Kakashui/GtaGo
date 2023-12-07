using GTANetworkAPI;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Core
{
    class AdminSP : Script
    {        
        public static void Spectate(ExtPlayer player, ExtPlayer target)
        {
            if (!player.IsLogged()) return;
            if (target == null) return;
            if (target == player)
            {
                Chat.SendTo(player, "Нельзя следить за самим собой");
                return;
            }
            if (!target.IsLogged())
            {
                Chat.SendTo(player, "Игрок под данным ID еще не авторизовался");
                return;
            }
            if (target.Session.SPActivated)
            {
                Chat.SendTo(player, "За этим администратором сейчас нельзя следить.");
                return;
            }

            if (!player.Session.SPActivated)
            {
                player.Session.SPActivated = true;
                player.Session.SPPosition = player.Position;
                player.Session.SPDimension = player.Session.Dimension;
                player.Session.SPInvisible = player.Session.Invisible;

                Admin.SetInvisible(player, true);
            }
            else SafeTrigger.ClientEvent(player, "spmode", null, false);

            SafeTrigger.UpdateDimension(player, target.Dimension);
            player.Session.SPClient = target.Value;
            player.ChangePosition(new Vector3(target.Position.X, target.Position.Y, (target.Position.Z + 3)));
            SafeTrigger.ClientEvent(player, "spmode", target, true);
            Chat.SendTo(player, $"Вы наблюдаете за {target.Name} [ID: {target.Character.UUID}].");
        }

        [RemoteEvent("UnSpectate")]
        public static void RemoteUnSpectate(ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            if (!Group.CanUseAdminCommand(player, "sp")) return;
            UnSpectate(player);
        }
        
        public static void UnSpectate(ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            if (!player.Session.SPActivated)
            {
                Chat.SendTo(player, "Вы не находитесь в режиме наблюдателя");
                return;
            }

            player.Session.SPActivated = false;
            player.Session.SPClient = -1;
            SafeTrigger.ClientEvent(player, "spmode", null, false);
            SafeTrigger.UpdateDimension(player, player.Session.SPDimension);
            player.ChangePosition(player.Session.SPPosition);
            Chat.SendTo(player, "Вы вышли из режима наблюдателя");

            NAPI.Task.Run(() =>
            {
                Admin.SetInvisible(player, player.Session.SPInvisible);
            }, 500);
        }
    }
}
