using GTANetworkAPI;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Core
{
    class AdminSP : Script
    {        
        public static void Spectate(Player player, Player target)
        {
            if (!player.IsLogged())
                return;
                if (target != null)
                {
                    if (target != player)
                    {
                        if (target.IsLogged())
                        {
                            if (target.GetData<bool>("spmode") == false)
                            {
                                if (player.GetData<bool>("spmode") == false)
                                {
                                    player.SetData("sppos", player.Position);
                                    player.SetData("spdim", player.Dimension);
                                }
                                else player.TriggerEvent("spmode", null, false);
                                player.SetSharedData("INVISIBLE", true);
                                player.SetData("spmode", true);
                                player.SetData("spclient", target.Value);
                                player.Transparency = 0;
                                player.Dimension = target.Dimension;
                                player.ChangePosition(new Vector3(target.Position.X, target.Position.Y, (target.Position.Z + 3)));
                                player.TriggerEvent("spmode", target, true);
                                Chat.SendTo(player, "Core_109@" + target.Name + " [ID: " + target.GetCharacter().UUID + "].");
                            }
                        }
                        else Chat.SendTo(player, "Core_110");
                    }   
                    else Chat.SendTo(player, "Core_111");
                }
        }

        [RemoteEvent("UnSpectate")]
        public static void RemoteUnSpectate(Player player)
        {
            if (!player.IsLogged()) return;
            if (!Group.CanUseAdminCommand(player, "sp")) return;
            UnSpectate(player);
        }
        
        public static void UnSpectate(Player player)
        {
            if (player.IsLogged())
            {
                if (player.GetData<bool>("spmode"))
                {
                    player.TriggerEvent("spmode", null, false);
                    player.SetData("spclient", -1);
                    WhistlerTask.Run(() => {
                        player.Dimension = player.GetData<uint>("spdim");
                        player.ChangePosition(player.GetData<Vector3>("sppos"));
                        player.Transparency = 255;
                        player.SetSharedData("INVISIBLE", false);
                        player.SetData("spmode", false);
                        Chat.SendTo(player,"Core_114");
                    }, 400);
                }
                else Chat.SendTo(player,"Core_115");
            }
        }
    }
}
