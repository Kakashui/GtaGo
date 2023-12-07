using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Scenes.Configs;
using Newtonsoft.Json;
using System.Linq;
using Whistler.Entities;
using Whistler.SDK;

namespace Whistler.Scenes
{
    public delegate bool ActionCallbackDelegate(ExtPlayer player);
    static class SceneManager
    {
        private static ScenesConfig _config { get; set; }
        
        public static void Init()
        {
            _config = new ScenesConfig();
            _config.Parse();
        }

        public static void StartScene(ExtPlayer player, int id)
        {
            SafeTrigger.SetSharedData(player, "scene:current", id);
        }

        public static void StartScene(ExtPlayer player, SceneNames name)
        {
            SafeTrigger.SetSharedData(player, "scene:current", (int)name);
            _config[name].ActionOnStart?.Invoke(player);
        }

        public static void ReqestAction(ExtPlayer player)
        {           
            int scene = player.GetSharedData<int>("scene:current");
            _config[scene].InvokeActionCallback(player);
        }

        internal static void StopScene(ExtPlayer player)
        {
            int scene = player.GetSharedData<int>("scene:current");
            if (scene > 0)
                _config[scene].ActionOnFinish?.Invoke(player);

            player.Session.SceneItem = null;
            player.Session.SceneCount = 0;
            SafeTrigger.SetSharedData(player, "scene:current", (int)SceneNames.NoAction);
        }

        internal static void InvokeSequenceCallback(ExtPlayer player)
        {
            int scene = player.GetSharedData<int>("scene:current");
            _config[scene].InvokeSequenceCallback(player);
            StopScene(player);
        }

        internal static void DevScene(ExtPlayer player, int sceneId, int boneId)
        {
            var scene = _config[sceneId];
            SafeTrigger.ClientEvent(player,"devattach", scene.Base.Dictionary, scene.Base.Name, scene.Base.Flag, boneId, JsonConvert.SerializeObject(scene.BaseAttachs?.Select(a=>a.Model).ToList()));
        }
    }
}
