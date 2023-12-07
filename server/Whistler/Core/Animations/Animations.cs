using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Core.CustomSync;
using Whistler.Core.CustomSync.Attachments;
using Whistler.Helpers;
using Whistler.Scenes.Configs;
using Whistler.SDK;

namespace Whistler.Core.Animations
{
    internal class Animations : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Animations));
        private static string _droptimer = null;

        [RemoteEvent("animations:play")]
        public void HandlePlayPlayerAnimation(Player player, string animationKey)
        {
            try
            {
                if (player.HasData("AntiAnimDown") || player.GetCharacter().Following != null || player.IsInVehicle
                    || player.GetCharacter().ArrestDate > DateTime.UtcNow || player.GetCharacter().DemorganTime > 0) return;

                if (player.HasSharedData("scene:current") && player.GetSharedData<int>("scene:current") != (int)SceneNames.NoAction) return;

                var animation = AnimationsConfig.Animations[animationKey];

                switch (animation.Category)
                {
                    case "gaits":
                        var gaitIndex = Convert.ToInt32(animationKey.Last().ToString());
                        player.SetSharedData("playerws", gaitIndex - 1);
                        break;
                    case "moods":
                        var moodIndex = Convert.ToInt32(animationKey.Last().ToString());
                        player.SetSharedData("playermood", moodIndex);
                        break;
                    default:
                        player.PlayAnimGo(animation.Dictionary, animation.Name, (AnimFlag)animation.Flag);

                        if (animation.Dictionary == "random@arrests@busted" &&
                            animation.Name == "idle_c")
                        {
                            player.SetData("HANDS_UP", true);
                        }

                        player.SetData("LastAnimFlag", animation.Flag);
                        player.TriggerEvent("animations:setPlay", true);
                        break;
                }
            }
            catch (Exception e) { _logger.WriteError($"Animations:play unhandled error catched with animationKey = {animationKey}: " + e.ToString()); }
        }

        [RemoteEvent("animations:stop")]
        public void HandleStopPlayerAnimation(Player player)
        {
            try
            {
                if (player.HasData("AntiAnimDown") || player.GetCharacter().Following != null || player.IsInVehicle
                    || player.GetCharacter().ArrestDate > DateTime.UtcNow || player.GetCharacter().DemorganTime > 0) return;

                player.ResetData("HANDS_UP");
                player.StopAnimGo();

                if (player.HasData("LastAnimFlag") && player.GetData<int>("LastAnimFlag") == 39)
                    player.ChangePosition(player.Position + new Vector3(0, 0, 0.2));

                player.TriggerEvent("animations:setPlay", false);
            }
            catch (Exception e) { _logger.WriteError("Animations:stop unhandled error catched: " + e.ToString()); }
        }

        public static void PickUpItem(Player player)
        {
            Chat.Action(player, "inv_5");
            //if (_droptimer != null)
            //{
            //    Timers.Stop(_droptimer);
            //    _droptimer = null;
            //}
            //player.PlayAnimGo("anim@scripted@freemode@postertag@collect_can@heeled@", "poster_tag_collect_can_var02_female", 0);
            //_droptimer = Timers.StartOnce(3300, () => player.StopAnimGo());
        }

        public static void DropItem(Player player)
        {
            Chat.Action(player, "inv_6");
            //if (_droptimer != null)
            //{
            //    Timers.Stop(_droptimer);
            //    _droptimer = null;
            //}
            //player.PlayAnimGo("anim@narcotics@trash", "drop_front", 0);
            //_droptimer = Timers.StartOnce(2000, () => player.StopAnimGo());
        }
    }
}
