using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Scenes.Configs;
using Newtonsoft.Json;
using System.Linq;
using Whistler.Entities;

namespace Whistler.Scenes
{
    public delegate bool ActionCallbackDelegate(PlayerGo player);
    static class SceneManager
    {
        private static ScenesConfig _config { get; set; }
        
        public static void Init()
        {
            _config = new ScenesConfig();
            _config.Parse();
        }

        public static void StartScene(PlayerGo player, int id)
        {
            player.SetSharedData("scene:current", id);
        }

        public static void StartScene(PlayerGo player, SceneNames name)
        {
            player.SetSharedData("scene:current", (int)name);
            _config[name].ActionOnStart?.Invoke(player);
        }

        public static void ReqestAction(PlayerGo player)
        {           
            int scene = player.GetSharedData<int>("scene:current");
            _config[scene].InvokeActionCallback(player);
        }

        internal static void StopScene(PlayerGo player)
        {
            int scene = player.GetSharedData<int>("scene:current");
            if (scene > 0)
                _config[scene].ActionOnFinish?.Invoke(player);
            player.ResetData("scene:action:count");
            player.ResetData("scene:action:item");
            player.SetSharedData("scene:current", (int)SceneNames.NoAction);
        }

        internal static void InvokeSequenceCallback(PlayerGo player)
        {
            int scene = player.GetSharedData<int>("scene:current");
            _config[scene].InvokeSequenceCallback(player);
            StopScene(player);
        }

        internal static void DevScene(PlayerGo player, int sceneId, int boneId)
        {
            var scene = _config[sceneId];
            player.TriggerEvent("devattach", scene.Base.Dictionary, scene.Base.Name, scene.Base.Flag, boneId, JsonConvert.SerializeObject(scene.BaseAttachs?.Select(a=>a.Model).ToList()));
        }
    }
}
