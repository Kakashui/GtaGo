using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Core.CustomSync;
using Whistler.Core.CustomSync.Attachments;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Scenes.Configs;
using Whistler.SDK;

namespace Whistler.Core.Animations
{
    internal class Animations : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Animations));

        [RemoteEvent("animations:play")]
        public void HandlePlayPlayerAnimation(ExtPlayer player, string animationKey)
        {
            try
            {
                if (player.HasData("AntiAnimDown") || player.Character.Following != null || player.IsInVehicle
                    || player.Character.ArrestDate > DateTime.Now || player.Character.DemorganTime > 0) return;

                if (player.HasSharedData("scene:current") && player.GetSharedData<int>("scene:current") != (int)SceneNames.NoAction) return;

                var animation = AnimationsConfig.Animations[animationKey];

                switch (animation.Category)
                {
                    case "gaits":
                        var gaitIndex = Convert.ToInt32(animationKey.Last().ToString());
                        SafeTrigger.SetSharedData(player, "playerws", gaitIndex - 1);
                        break;
                    case "moods":
                        var moodIndex = Convert.ToInt32(animationKey.Last().ToString());
                        SafeTrigger.SetSharedData(player, "playermood", moodIndex);
                        break;
                    default:
                        player.PlayAnimGo(animation.Dictionary, animation.Name, (AnimFlag)animation.Flag);

                        if (animation.Dictionary == "random@arrests@busted" &&
                            animation.Name == "idle_c")
                        {
                            SafeTrigger.SetData(player, "HANDS_UP", true);
                        }

                        SafeTrigger.SetData(player, "LastAnimFlag", animation.Flag);
                        SafeTrigger.ClientEvent(player,"animations:setPlay", true);
                        break;
                }
            }
            catch (Exception e) { _logger.WriteError($"Animations:play unhandled error catched with animationKey = {animationKey}: " + e.ToString()); }
        }

        [RemoteEvent("animations:stop")]
        public void HandleStopPlayerAnimation(ExtPlayer player)
        {
            try
            {
                if (player.HasData("AntiAnimDown") || player.Character.Following != null || player.IsInVehicle
                    || player.Character.ArrestDate > DateTime.Now || player.Character.DemorganTime > 0) return;

                player.ResetData("HANDS_UP");
                player.StopAnimGo();

                if (player.HasData("LastAnimFlag") && player.GetData<int>("LastAnimFlag") == 39)
                    player.ChangePosition(player.Position + new Vector3(0, 0, 0.2));

                SafeTrigger.ClientEvent(player,"animations:setPlay", false);
            }
            catch (Exception e) { _logger.WriteError("Animations:stop unhandled error catched: " + e.ToString()); }
        }

        public static void PickUpItem(ExtPlayer player)
        {
            Chat.Action(player, "поднял предмет с земли");

            if (player.Session.Timers.AnimationTimer != null) Timers.Stop(player.Session.Timers.AnimationTimer);

            player.PlayAnimGo("anim@scripted@freemode@postertag@collect_can@heeled@", "poster_tag_collect_can_var02_female", 0);
            player.Session.Timers.AnimationTimer = Timers.StartOnce(3300, () => player.StopAnimGo());
        }

        public static void DropItem(ExtPlayer player)
        {
            Chat.Action(player, "бросил предмет на землю");
            if (player.Session.Timers.AnimationTimer != null) Timers.Stop(player.Session.Timers.AnimationTimer);

            player.PlayAnimGo("anim@narcotics@trash", "drop_front", 0);
            player.Session.Timers.AnimationTimer = Timers.StartOnce(2000, () => player.StopAnimGo());
        }
    }
}
