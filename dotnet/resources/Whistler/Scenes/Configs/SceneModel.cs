using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Core.CustomSync;
using Whistler.Entities;

namespace Whistler.Scenes.Configs
{
    class SceneModel
    {
        #region anim sequence
        [JsonProperty("sequence")]
        public List<AnimationModel> Sequence { get; set; }
        [JsonIgnore]
        public ActionCallbackDelegate SequenceCallback { get; set; }
        public SceneModel AddSequence(string dict, string name, float start, float stop, params AnimFlag[] flags)
        {
            if(Sequence == null)
            {
                Sequence = new List<AnimationModel>();
            }
            var flag = (int)AnimFlag.Normal;
            foreach (var f in flags.ToList())
            {
                flag |= (int)f;
            }
            Sequence.Add(new AnimationModel(dict, name, flag, start, stop));
            return this;
        }
        public SceneModel AddSequenceCallback(ActionCallbackDelegate callback)
        {
            SequenceCallback = callback;
            return this;
        }   
        public void InvokeSequenceCallback(ExtPlayer player)
        {
            if (SequenceCallback == null) return;
            SequenceCallback.Invoke(player);
        }
        #endregion anim sequence

        #region attachs

        [JsonProperty("attachs")]
        public List<AttachedModel> BaseAttachs { get; set; }
        public SceneModel AddAttach(string model, int boneId, Vector3 pos, Vector3 rot, bool deleteBefoereExit = false)
        {
            if (BaseAttachs == null)
            {
                BaseAttachs = new List<AttachedModel>();
            }
            BaseAttachs.Add(new AttachedModel(model, boneId, pos, rot, deleteBefoereExit));
            return this;
        }
        #endregion attachs

        #region anim actions
        [JsonProperty("enter")]
        public AnimationModel Enter { get; set; }
        [JsonProperty("enterEffects")]
        public List<ActionEffectModel> EnterEffects { get; set; }
        [JsonProperty("action")]
        public AnimationModel Action { get; set; }
        [JsonProperty("actionEffects")]
        public List<ActionEffectModel> ActionEffects { get; set; }
        [JsonProperty("base")]
        public AnimationModel Base { get; set; }
        [JsonProperty("exit")]
        public AnimationModel Exit { get; set; }
        [JsonProperty("isLooped")]
        public bool IsLooped { get; set; } = false;
        [JsonProperty("canCancel")]
        public bool CanCancel { get; set; } = false;
        [JsonIgnore]
        public ActionCallbackDelegate ActionCallback { get; private set; }
        [JsonIgnore]
        public Action<ExtPlayer> ActionOnStart { get; private set; }
        [JsonIgnore]
        public Action<ExtPlayer> ActionOnFinish { get; private set; }
        public SceneModel AddEnterAnim(string dict, string name, float start, float stop, params AnimFlag[] flags)
        {
            var flag = (int)AnimFlag.Normal;
            foreach (var f in flags.ToList())
            {
                flag |= (int)f;
            }
            Enter = new AnimationModel(dict, name, flag, start, stop);
            return this;
        }

        public SceneModel AddEnterEffect(ActionEffectModel effect)
        {
            if (EnterEffects == null)
            {
                EnterEffects = new List<ActionEffectModel>();
            }
            EnterEffects.Add(effect);
            return this;
        }

        public SceneModel AddBaseAnim(string dict, string name, float start, float stop, params AnimFlag[] flags)
        {
            var flag = (int)AnimFlag.Normal;
            foreach (var f in flags.ToList())
            {
                flag |= (int)f;
            }
            Base = new AnimationModel(dict, name, flag, start, stop);
            return this;
        }
        public SceneModel AddAction(string dict, string name, float start, float stop, params AnimFlag[] flags)
        {
            var flag = (int)AnimFlag.Normal;
            foreach (var f in flags.ToList())
            {
                flag |= (int)f;
            }
            Action = new AnimationModel(dict, name, flag, start, stop);
            return this;
        }

        public SceneModel AddActionOnStart(Action<ExtPlayer> action)
        {
            ActionOnStart = action;
            return this;
        }

        public SceneModel AddActionOnFinish(Action<ExtPlayer> action)
        {
            ActionOnFinish = action;
            return this;
        }

        public SceneModel AddActionEffect(ActionEffectModel effect)
        {
            if(ActionEffects == null)
            {
                ActionEffects = new List<ActionEffectModel>();
            }
            ActionEffects.Add(effect);
            return this;
        }

        public SceneModel AddActionCallback(ActionCallbackDelegate actionCallback)
        {
            ActionCallback = actionCallback;
            return this;
        }

        public SceneModel AddExitAnim(string dict, string name, float start, float stop, params AnimFlag[] flags)
        {
            var flag = (int)AnimFlag.Normal;
            foreach (var f in flags.ToList())
            {
                flag |= (int)f;
            }
            Exit = new AnimationModel(dict, name, flag, start, stop);
            return this;
        }

        public SceneModel MarkAsLooped()
        {
            IsLooped = true;
            return this;
        }
        public SceneModel MarkAsCancelled()
        {
            CanCancel = true;
            return this;
        }

        public void InvokeActionCallback(ExtPlayer player)
        {
            if (ActionCallback == null) return;
            if (!ActionCallback.Invoke(player))
            {
                SceneManager.StopScene(player);
            }
            else
                NAPI.ClientEvent.TriggerClientEventInRange(player.Position, 10, "scene:doaction", player.Value);
        }

        #endregion anim actions

    }
}
