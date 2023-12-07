using GTANetworkAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Enums;
using Whistler.PlayerEffects;
using Whistler.Scenes.Configs;

namespace Whistler.Inventory.Configs.Models
{
    public abstract class BaseConfig
    {
        
        public ItemTypes Type { get; set; }
        public int Weight { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public uint ModelHash { get; set; }
        public bool Stackable { get; set; } = false;
        public SceneNames SceneName { get; set; }
        public int ActionsCount { get; set; } = 0;
        public List<List<EffectModel>> Effects { get; set; }
        public bool Disposable { get; set; } = true;    
        public Vector3 DropRotation { get; set; } = new Vector3();
        public Vector3 DropOffsetPosition { get; set; } = new Vector3();
        public bool CanUse { get; set; } = false;
        public sbyte ShopType { get; set; } = -1;
    }
}
