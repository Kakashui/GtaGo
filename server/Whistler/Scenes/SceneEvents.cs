using System;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Scenes.Actions;
using Whistler.Scenes.Configs;

namespace Whistler.Scenes
{
    class SceneEvents:Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void OnStart()
        {
            SceneManager.Init();
            ItemActions.Load();
        }

        [RemoteEvent("scene:action:request")]
        public void RequestSceneAction(PlayerGo player)
        {
            if (!player.IsLogged()) return;
            if (!player.HasSharedData("scene:current") || player.GetSharedData<int>("scene:current") == (int)SceneNames.NoAction)
            {
                player.TriggerEvent("scene:reset:local");
                return;
            }
            SceneManager.ReqestAction(player);
        }

        [RemoteEvent("scene:action:cancel")]
        public void CancelSceneAction(PlayerGo player)
        {
            if (!player.IsLogged()) return;
            SceneManager.StopScene(player);
        }

        [RemoteEvent("scene:seqence:callback")]
        public void SequenceCallback(PlayerGo player)
        {
            if (!player.IsLogged()) return;
            SceneManager.InvokeSequenceCallback(player);
        }

        [Command("scenestart")]
        public void PlayScene(PlayerGo player, int sceneId)
        {
            if (!player.IsLogged()) return;
            if (!Group.CanUseAdminCommand(player, "playscene")) return;
            SceneManager.StartScene(player, sceneId);
        }

        [Command("scenestop")]
        public void StopScene(PlayerGo player)
        {
            if (!player.IsLogged()) return;
            if (!Group.CanUseAdminCommand(player, "playscene")) return;
            SceneManager.StopScene(player);
        }      

        [Command("devattach")]
        public void DevAttach(PlayerGo player, int sceneId, int boneId)
        {
            if (!player.IsLogged()) return;
            if (!Group.CanUseAdminCommand(player, "develop")) return;
            SceneManager.DevScene(player, sceneId, boneId);
        }
      
    }
}
