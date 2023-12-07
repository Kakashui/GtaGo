using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Scenes.Configs
{
    class AttachedModel
    {
        public AttachedModel(string model, int boneId, Vector3 offset, Vector3 rotate, bool deleteBefore)
        {
            Model = model;
            BoneId = boneId;
            Offset = offset;
            Rotate = rotate;
            DeleteBefore = deleteBefore;
        }
        [JsonProperty("model")]
        public string Model { get; set; }
        [JsonProperty("boneId")]
        public int BoneId { get; set; }
        [JsonProperty("offset")]
        public Vector3 Offset { get; set; }
        [JsonProperty("rotate")]
        public Vector3 Rotate { get; set; }
        [JsonProperty("deleteBefore")]
        public bool DeleteBefore { get; set; }
    }
}
