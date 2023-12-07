using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Scenes.Configs
{
    class ActionEffectModel
    {
        public ActionEffectModel(string asset, string name, float startAt, int duration, float scale, int boneId, Vector3 offset, Vector3 rotate)
        {
            Asset = asset;
            Name = name;
            BoneId = boneId;
            Offset = offset;
            Rotate = rotate;
            StartAt = startAt;
            Duration = duration;
            Scale = scale;
        }
        [JsonProperty("asset")]
        public string Asset { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("startAt")]
        public float StartAt { get; set; }
        [JsonProperty("duration")]
        public int Duration { get; set; }
        [JsonProperty("scale")]
        public float Scale { get; set; }
        [JsonProperty("boneId")]
        public int BoneId { get; set; }
        [JsonProperty("offset")]
        public Vector3 Offset { get; set; }
        [JsonProperty("rotate")]
        public Vector3 Rotate { get; set; }
    }
}
