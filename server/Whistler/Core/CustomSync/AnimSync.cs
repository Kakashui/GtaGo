using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Whistler.Core.CustomSync
{
    public static class AnimSync
    {
        public static void PlayAnimGo(this Player player, string dictionary, string name, params AnimFlag[] flags)
        {
            var flag = AnimFlag.Normal;
            foreach (var f in flags)
            {
                flag |= f;
            }
            var animation = new Animation
            {
                Name = name,
                Dictionary = dictionary,
                Flag = flag
            };

            player.SetSharedData("animSync:animation", JsonConvert.SerializeObject(animation));
        }
        public static void StopAnimGo(this Player player)
        {
            player.ResetSharedData("animSync:animation");
        }

        private class Animation
        {
            public string Name { get; set; }
            public string Dictionary { get; set; }
            public AnimFlag Flag { get; set; }
        }
    }
    
    [Flags]
    public enum AnimFlag : int
    {
        Normal = 0, //-1 
        Looped = 1, //0
        StopLastFrame = 2, //1
        Unknown3 = 4,//2
        Unknown4 = 8,//3
        UpperBody = 16,//4
        CanMove = 32,//5
        Unknown1 = 53,//-
        Unknown7 = 64,//6
        CancelableOnMove = 128,//7
        Additive = 256,//8
        NonCollisionAndPhysics = 512,//9
        NonCollisionAndPhysicsWithInitialOffset = 1024,//10
        NonCollisionAndPhysics2 = 2048,//11
        Unknown13 = 4096,//12
        Unknown14 = 8192,//13
        Unknown15 = 16384,//14
        Unknown16 = 32768,//15
    }
}
