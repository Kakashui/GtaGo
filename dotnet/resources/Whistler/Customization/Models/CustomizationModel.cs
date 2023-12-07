using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.ClothesCustom;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.SDK;

namespace Whistler.Customization.Models
{
    public class CustomizationModel
    {
        public CustomizationModel(bool gender, byte eyeColor, Dictionary<int, HeadOverlay> headOverlays, HeadBlend headBlend, List<FaceFeatureDTO> faceFeatures, HairsDTO hairs, List<Decoration> tatoos, int id = -1)
        {
            Id = id;
            Gender = gender;
            EyeColor = eyeColor;
            HeadBlend = headBlend;
            HeadOverlays = headOverlays;
            FaceFeatures = faceFeatures;
            Tattoos = tatoos;
            Hairs = hairs;
        }

        public int Id { get; set; }
        public bool Gender { get; set; }
        public byte EyeColor { get; set; }
        public HairsDTO Hairs { get; set; }
        public Dictionary<int, HeadOverlay> HeadOverlays { get; set; }
        public HeadBlend HeadBlend { get; set; }
        public List<FaceFeatureDTO> FaceFeatures { get; set; }
        public List<Decoration> Tattoos { get; set; }
        public bool Created { get; set; }

        private static Dictionary<int, HeadOverlay> _maskHeadOverlays = new Dictionary<int, HeadOverlay>{
            {1, new HeadOverlay{Index=255, Color= 0, Opacity= 0, SecondaryColor= 0} },
            {2, new HeadOverlay{Index=255, Color= 0, Opacity= 0, SecondaryColor= 0} }
        };

        private static List<FaceFeatureDTO> _maskFeatures = new List<FaceFeatureDTO>
        {
            new FaceFeatureDTO(0,-1),
            new FaceFeatureDTO(2,-1),
            new FaceFeatureDTO(9,-1),
            new FaceFeatureDTO(10,-1),
            new FaceFeatureDTO(13,-1),
            new FaceFeatureDTO(14,-1),
            new FaceFeatureDTO(15,-1),
            new FaceFeatureDTO(16,-1),
            new FaceFeatureDTO(17,-1),
            new FaceFeatureDTO(18,-1)
        };

        public void Apply(ExtPlayer player)
        {
            UpdateGender(player);
            UpdateHeadBlendData(player);
            UpdateHeadOverlays(player);
            UpdateFaceFeatures(player);
            UpdateHairs(player);
            UpdateTattoos(player);
            NAPI.Player.SetPlayerEyeColor(player, EyeColor);
            player.GetEquip()?.Update(false);
        }

        public void SetMaskFace(ExtPlayer player, bool state)
        {
            if (state)
            {
                foreach (var feature in _maskFeatures)
                {
                    player.SetFaceFeature(feature.OverlayId, feature.Value);
                }
                //foreach (var overlay in _maskHeadOverlays)
                //{
                //    player.SetHeadOverlay(overlay.Key, overlay.Value);
                //}
                //player.SetWhistlerClothes(2, 0, 0);
            }
            else
            {
                UpdateFaceFeatures(player);
                //foreach (var overlay in _maskHeadOverlays)
                //{
                //    player.SetHeadOverlay(overlay.Key, HeadOverlays[overlay.Key]);
                //}
                //UpdateHairs(player);
            }
        }

        private void UpdateHeadBlendData(ExtPlayer player)
        {
            player.HeadBlend = HeadBlend;
        }
        private void UpdateHeadOverlays(ExtPlayer player)
        {
            foreach (var overlay in HeadOverlays)
            {
                player.SetHeadOverlay(overlay.Key, overlay.Value);
            }
        }
        private void UpdateFaceFeatures(ExtPlayer player)
        {
            foreach (var feature in FaceFeatures)
            {
                player.SetFaceFeature(feature.OverlayId, feature.Value);
            }
        }

        internal void UpdateHairsModel(ExtPlayer player, int style, int color1, int color2)
        {
            Hairs.Id = style;
            Hairs.Color1 = (byte)color1;
            Hairs.Color2 = (byte)color2;
            UpdateHairs(player);
            CustomizationService.UpdateHairs(this);
        }

        internal void EditHeadOverlay(ExtPlayer player, int v, int style, int color)
        {
            HeadOverlays[v] = new HeadOverlay
            {
                Index = (byte)style,
                Color = (byte)color,
                Opacity = 1
            };
            UpdateHeadOverlays(player);
            CustomizationService.UpdateHeadOverlays(this);
        }

        private void UpdateHairs(ExtPlayer player)
        {
            player.SetWhistlerClothes(2, Hairs.Id, 0);
            NAPI.Player.SetPlayerHairColor(player, Hairs.Color1, Hairs.Color2);
        }

        internal void EditEyes(ExtPlayer player, int style)
        {
            EyeColor = (byte)style;
            NAPI.Player.SetPlayerEyeColor(player, EyeColor);
            CustomizationService.UpdateEyes(this);
        }

        private void UpdateGender(ExtPlayer player)
        {
            var model = Gender ? PedHash.FreemodeMale01 : PedHash.FreemodeFemale01;
            player.SetSkin(model);
            if (!player.HasSharedData("GENDER") || player.GetSharedData<bool>("GENDER") != Gender)
                SafeTrigger.SetSharedData(player, "GENDER", Gender);
        }

        public void UpdateTattoos(ExtPlayer player)
        {
            player.ClearDecorations();
            NAPI.Task.Run(() => {
                foreach (var tattoo in Tattoos)
                {
                    player.SetDecoration(tattoo);
                }
            }, 500);           
            SafeTrigger.ClientEvent(player,"tattoo:list:update", Tattoos);
        }
    }
}
