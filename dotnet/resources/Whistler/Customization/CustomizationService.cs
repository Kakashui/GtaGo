using GTANetworkAPI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Core.Character;
using Whistler.Customization.Models;
using Whistler.Helpers;
using Whistler.SDK;
using Newtonsoft.Json;
using System.Data;
using Whistler.Customization.Enums;
using System.IO;
using Whistler.Core;
using Whistler.Core.QuestPeds;
using Whistler.Fractions;
using Whistler.MoneySystem;
using Whistler.MoneySystem.Interface;
using Whistler.Entities;

namespace Whistler.Customization
{
    
    public static class CustomizationService
    {
        private static ConcurrentDictionary<int, CustomizationModel> _cache = new ConcurrentDictionary<int, CustomizationModel>();
        private static int DimensionID = 334455;
        private static Vector3 CreatorCharPos = new Vector3(-811.697, 175.1083, 76.74536);
        private const int PlasticSurgeryPrice = 1000000;

        private class TattooData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<TattooZones> Slots { get; set; }
            public int Price { get; set; }
            public uint Dict { get; set; }
            public uint Male { get; set; }
            public uint Female { get; set; }
        }

        public static void InitDoctor()
        {
            var qp = new QuestPed(PedHash.Doctor01SMM, new Vector3(321.8153, -565.7399, 43.1297), "House", "Доктор", 150);
            qp.PlayerInteracted += Qp_PlayerInteracted;
        }

        private static void Qp_PlayerInteracted(ExtPlayer player, QuestPed ped)
        {
            var InfoTab = new DialogPage($"Операция по смене внешности обойдется вам в {PlasticSurgeryPrice} $, а вот стоимость удаления татуировки будет зависеть от сложности работы.",
                ped.Name,
                ped.Role);

            var startPage = new DialogPage("Привет. Я лучший пластический хирург в штате. Вы хотите заказать какую-нибудь услугу?",
                ped.Name,
                ped.Role)
                .AddAnswer("Да. Я хочу изменить внешность.", SendToCreatorFromDoctor)
                .AddAnswer("Я хочу вывести татуировку.", OpenRemoveTattooMenu)
                .AddAnswer("Расскажите пожалуйста, сколько стоят ваши услуги?", InfoTab)
                .AddCloseAnswer("Я просто хотел узнать как ваши дела. До свидания.");

            InfoTab.AddAnswer("Понятно. Я хочу заказать у вас услугу.", startPage)
                .AddCloseAnswer("Спасибо за информацию. До свидания!");

            startPage.OpenForPlayer(player);
        }

        private static Vector3 _removeTattooPosition = new Vector3(323.8332, -582.7084, 43.12974);
        private static void OpenRemoveTattooMenu(ExtPlayer player)
        {
            player.Character.ExteriorPos = player.Position;            
            SafeTrigger.UpdateDimension(player,  Dimensions.RequestPrivateDimension());
            player.ChangePosition(_removeTattooPosition);
            player.Rotation = new Vector3(0, 0, 328.0603);
            SafeTrigger.ClientEvent(player,"tattoo:remove:open");
        }

        public static void SendToCreatorFromDoctor(ExtPlayer player)
        {
            var half = PlasticSurgeryPrice / 2;
            if (Wallet.TransferMoney(player.Character, new List<(IMoneyOwner, int)> 
            { 
                (Manager.GetFraction(6), half), 
                (Manager.GetFraction(8), half) 
            }, "Пластическая операция"))
            {
                SendToCreator(player, -1);
            }
            else Notify.SendError(player, $"Стоимость пластической операции {PlasticSurgeryPrice} $. У вас  не хватает денег.");
        }
        public static void ParseTattoo()
        {

            if (Directory.Exists("interfaces/gui/src/configs/tattooShop"))
            {
                List<string> zones = new List<string>()
                {
                    "torso",
                    "head",
                    "leftarm",
                    "rightarm",
                    "leftleg",
                    "rightleg",
                };

                var names = new Dictionary<string, List<TattooData>>();

                for (var i = 0; i < 6; i++)
                {
                    var index = 0;
                    var list = new List<TattooData>();
                    foreach (BusinessTattoo t in BusinessManager.BusinessTattoos[i])
                        list.Add(new TattooData
                        {
                            Id = index++,
                            Name = t.Name,
                            Slots = t.Slots,
                            Price = t.Price,
                            Male = t.MaleHash,
                            Female = t.FemaleHash,
                            Dict = t.Dictionary
                        });
                    names.Add(zones[i], list);
                }

                StreamWriter file = new StreamWriter("interfaces/gui/src/configs/tattooShop/config.js", false, Encoding.UTF8);
                file.Write($"export default {JsonConvert.SerializeObject(names)}");
                file.Close();
            }
        }

        private static float CreatorCharRot = 88.63f;
        
        public static void CreateNewCharacter(
            ExtPlayer player, 
            int slot,
            string firstName,
            string lastName,
            bool gender, 
            byte eyeColor, 
            ParentsDTO parents,
            List<FaceFeatureDTO> faceFeatures,
            Dictionary<int, HeadOverlayDTO> headOverlay,
            HairsDTO hairs,
            List<Decoration> tattoos,
            ClothesDTO clothes
        )
        {
            var customization = new CustomizationModel(gender, eyeColor, ConvertHeadOverlay(headOverlay), ConvertParrents(parents), faceFeatures, hairs, tattoos);
            SaveCustomization(customization);


            var account = player.Account;
            var character = new Character(firstName, lastName, account.Id, customization.Id, clothes);
            player.CreateCharacter(character);
            SafeTrigger.ClientEvent(player, "customization:destroy");
            account.Characters[slot] = character.UUID;
            MySQL.Query($"UPDATE `accounts` SET `character{slot + 1}`= @prop0 WHERE `login` = @prop1", character.UUID, account.Login);
            player.Spawn(1);
            player.Rotation = new Vector3(0, 0, 152.0855);
        }

        #region old data convertor
        private class HairData
        {
            public int Hair;
            public int Color;
            public int HighlightColor;
        }
        private class AppearanceItem
        {
            public int Value;
            public float Opacity;
        }
        private class ParentData
        {
            public int Father;
            public int Mother;
            public float Similarity;
            public float SkinSimilarity;

            public ParentData(int father, int mother, float similarity, float skinsimilarity)
            {
                Father = father;
                Mother = mother;
                Similarity = similarity;
                SkinSimilarity = skinsimilarity;
            }
        }
        internal static bool ConvertOldCustomization(ExtPlayer player)
        {
            var character = player.Character;
            var result = MySQL.QueryRead("SELECT * FROM customization WHERE `uuid`=@prop0", character.UUID);
            if (result == null || result.Rows.Count == 0)
                return false;

            var row = result.Rows[0];
            var gender = Convert.ToInt32(row["gender"]);
            var parents = JsonConvert.DeserializeObject<ParentData>(row["parents"].ToString());
            var features = JsonConvert.DeserializeObject<float[]>(row["features"].ToString());
            var aAppearance = JsonConvert.DeserializeObject<AppearanceItem[]>(row["appearance"].ToString());
            var hair = JsonConvert.DeserializeObject<HairData>(row["hair"].ToString());
            var eyebrowColor = Convert.ToInt32(row["eyebrowc"]);
            var beardColor = Convert.ToInt32(row["beardc"]);
            var eyeColor = Convert.ToInt32(row["eyec"]);
            var blushColor = Convert.ToInt32(row["blushc"]);
            var lipstickColor = Convert.ToInt32(row["lipstickc"]);
            var chestHairColor = Convert.ToInt32(row["chesthairc"]);
            var makeupColor = Convert.ToInt32(row["makeup"]);

            var headOvrlays = new Dictionary<int, HeadOverlay>();
            var faceFeatures = new List<FaceFeatureDTO>();
            var headBlebd = new HeadBlend { 
                ShapeFirst = (byte)parents.Mother,
                ShapeSecond = (byte)parents.Father,
                SkinFirst = (byte)parents.Mother,
                SkinSecond = (byte)parents.Father,
                ShapeMix = parents.Similarity,
                SkinMix = parents.SkinSimilarity
            };
            for (int i = 0; i < aAppearance.Length; i++)
            {
                var item = aAppearance[i];
                var headOverlay = new HeadOverlay { 
                    Index = (byte)item.Value,
                    Opacity = (byte)item.Opacity
                };
                if (i == 1) headOverlay.Color = (byte)beardColor;
                else if (i == 2) headOverlay.Color = (byte)eyebrowColor;
                else if (i == 5) headOverlay.Color = (byte)blushColor;
                else if (i == 8) headOverlay.Color = (byte)lipstickColor;
                else if (i == 10) headOverlay.Color = (byte)chestHairColor;
                else if (i == 4) headOverlay.Color = (byte)makeupColor;
                headOverlay.SecondaryColor = 100;
                headOvrlays.Add(i, headOverlay);
            }
            for (int i = 0; i < features.Length; i++) faceFeatures.Add(new FaceFeatureDTO(i, features[i]));
           
            var customiazation = new CustomizationModel(Main.PlayerSlotsInfo[character.UUID].Gender, (byte)eyeColor, headOvrlays, headBlebd, faceFeatures, new HairsDTO { Id = hair.Hair, Color1 = (byte)hair.Color, Color2 = (byte)hair.HighlightColor }, new List<Decoration>());
            SaveCustomization(customiazation);
            character.UpdateCustomization(customiazation.Id);
            customiazation.Apply(player);

            return true;
        }

        #endregion

        public static void ChangeCharacterCustomization(
           ExtPlayer player,
           bool gender,
           byte eyeColor,
           ParentsDTO parents,
           List<FaceFeatureDTO> faceFeatures,
           Dictionary<int, HeadOverlayDTO> headOverlay,
           HairsDTO hairs
        )
        {
            var character = player.Character;
            var customiazation = character.Customization;
            bool alreadySaved = false;
            if (customiazation == null)
            {
                customiazation = new CustomizationModel(gender, eyeColor, ConvertHeadOverlay(headOverlay), ConvertParrents(parents), faceFeatures, hairs, new List<Decoration>());
                SaveCustomization(customiazation);
                character.UpdateCustomization(customiazation.Id);
                alreadySaved = true;
            }
            else
            {
                character.Customization.Gender = gender;
                character.Customization.EyeColor = eyeColor;
                character.Customization.HeadOverlays = ConvertHeadOverlay(headOverlay);
                character.Customization.HeadBlend = ConvertParrents(parents);
                character.Customization.FaceFeatures = faceFeatures;
                character.Customization.Hairs = hairs;
            }
            SendPlayerToWorld(player);
            Main.PlayerSlotsInfo[character.UUID].Gender = gender;
            character.Customization.Apply(player);

            if (!alreadySaved)
                SaveCustomization(character.Customization);
            MySQL.Query("UPDATE `characters` SET `gender`= @prop0 WHERE `uuid` = @prop1", gender, character.UUID);
        }

        public static CustomizationModel GetById(int id)
        {
            return _cache.GetOrAdd(id, LoadFromDB);
        }
       
        private static CustomizationModel LoadFromDB(int id)
        {
            var result = MySQL.QueryRead("SELECT * FROM `newcustomization` WHERE `id`=@prop0", id);
            if (result.Rows.Count == 0) return null;
            var row = result.Rows[0];
            var customization = new CustomizationModel(
                Convert.ToBoolean(row["gender"]),
                Convert.ToByte(row["eyecolor"]),
                JsonConvert.DeserializeObject<Dictionary<int, HeadOverlay>>(row["headoverlays"].ToString()),
                JsonConvert.DeserializeObject<HeadBlend>(row["headblend"].ToString()),
                JsonConvert.DeserializeObject<List<FaceFeatureDTO>>(row["facefeatures"].ToString()),
                JsonConvert.DeserializeObject<HairsDTO>(row["hairs"].ToString()),
                JsonConvert.DeserializeObject<List<Decoration>>(row["tattoos"].ToString()),
                id
            );
            _cache.TryAdd(customization.Id, customization);
            return customization;
        }

        private static HeadBlend ConvertParrents(ParentsDTO patent)
        {
            return new HeadBlend
            {
                ShapeFirst = (byte)patent.Mother,
                ShapeSecond = (byte)patent.Father,
                SkinFirst = (byte)patent.Mother,
                SkinSecond = (byte)patent.Father,
                SkinMix = patent.Skin,
                ShapeMix = patent.Similarity
            };
        }
        private static Dictionary<int, HeadOverlay> ConvertHeadOverlay(Dictionary<int, HeadOverlayDTO> dto)
        {
            var result = new Dictionary<int, HeadOverlay>();
            foreach (var item in dto)
            {
                result.Add(item.Key, new HeadOverlay { Index = (byte)item.Value.Index, Color = (byte)item.Value.Color1, SecondaryColor = (byte)item.Value.Color2, Opacity = item.Value.Opacity });
            }
            return result;
        }

        private static void SaveCustomization(CustomizationModel customization)
        {
            if(customization.Id < 0)
            {
                var responce = MySQL.QueryRead("INSERT INTO `newcustomization` (`gender`, `eyecolor`, `headoverlays`, `headblend`, `facefeatures`, `hairs`, `tattoos`) VALUES(@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6);SELECT @@identity;",
                    customization.Gender,
                    customization.EyeColor,
                    JsonConvert.SerializeObject(customization.HeadOverlays),
                    JsonConvert.SerializeObject(customization.HeadBlend),
                    JsonConvert.SerializeObject(customization.FaceFeatures),
                    JsonConvert.SerializeObject(customization.Hairs),
                    JsonConvert.SerializeObject(customization.Tattoos)
                    );
                customization.Id = Convert.ToInt32(responce.Rows[0][0]);
                _cache.TryAdd(customization.Id, customization);
            }
            else
            {
                MySQL.QueryRead("UPDATE `newcustomization` SET `gender` = @prop0, `eyecolor`=@prop1, `headoverlays`=@prop2, `headblend`= @prop3, `facefeatures` = @prop4, `hairs`=@prop5 WHERE `id`=@prop6;",
                   customization.Gender,
                   customization.EyeColor,
                   JsonConvert.SerializeObject(customization.HeadOverlays),
                   JsonConvert.SerializeObject(customization.HeadBlend),
                   JsonConvert.SerializeObject(customization.FaceFeatures),
                   JsonConvert.SerializeObject(customization.Hairs),
                   customization.Id
                   );
            }
        }
        public static void SendToCreator(ExtPlayer player, int slot)
        {
            if (player.Session.InCreator) return;

            player.Session.InCreator = true;
            if (slot < 0) player.Session.PositionBeforeCreator = player.Position;

            SafeTrigger.UpdateDimension(player,  Convert.ToUInt32(DimensionID++));
            player.Rotation = new Vector3(0f, 0f, CreatorCharRot);
            SafeTrigger.UpdatePosition(player,  CreatorCharPos);


            SafeTrigger.ClientEvent(player, "customization:create", slot);
        }

        private static void SendPlayerToWorld(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player, "customization:destroy");
            Vector3 pos = player.Session.PositionBeforeCreator ?? new Vector3(-3029.699, 72.52473, 12.902239);
            player.Session.PositionBeforeCreator = null;
            player.Session.InCreator = false;
            SafeTrigger.UpdateDimension(player, 0);
            player.ChangePosition(pos); 
        }
        internal static void InitDB()
        {
            var query = $"CREATE TABLE IF NOT EXISTS `newcustomization`(" +
             $"`id` int(11) NOT NULL AUTO_INCREMENT," +
             $"`gender` TINYINT(1) NOT NULL," +
             $"`eyecolor` int(11) NOT NULL," +
             $"`headoverlays` TEXT NOT NULL," +
             $"`headblend` TEXT NOT NULL," +
             $"`facefeatures` TEXT NOT NULL,"+
             $"`hairs` TEXT NOT NULL," +
             $"`tattoos` TEXT NOT NULL," +
             $"PRIMARY KEY(`id`)" +
             $")ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";
            MySQL.Query(query);
        }

        internal static void UpdateTattoos(CustomizationModel customization)
        {
            MySQL.QueryRead("UPDATE `newcustomization` SET `tattoos` = @prop0 WHERE `id`=@prop1;",                 
                  JsonConvert.SerializeObject(customization.Tattoos),
                  customization.Id
                  );
        }
        internal static void UpdateHairs(CustomizationModel customization)
        {
            MySQL.QueryRead("UPDATE `newcustomization` SET `hairs` = @prop0 WHERE `id`=@prop1;",
                  JsonConvert.SerializeObject(customization.Hairs),
                  customization.Id
                  );
        }

        internal static void UpdateEyes(CustomizationModel customizationModel)
        {
            MySQL.QueryRead("UPDATE `newcustomization` SET `eyecolor` = @prop0 WHERE `id`=@prop1;",
                 customizationModel.EyeColor,
                 customizationModel.Id
                 );
        }

        internal static void UpdateHeadOverlays(CustomizationModel customizationModel)
        {
            MySQL.QueryRead("UPDATE `newcustomization` SET `headoverlays` = @prop0 WHERE `id`=@prop1;",
                 JsonConvert.SerializeObject(customizationModel.HeadOverlays),
                 customizationModel.Id
                 );
        }

        internal static void ConvertTattoo()
        {
            var result = MySQL.QueryRead("SELECT `id`, `tattoos` FROM `newcustomization`");
            if (result.Rows.Count == 0) return;
            foreach (DataRow row in result.Rows)
            {
                try
                {
                    var json = row["tattoos"].ToString();
                    if (json[0] == '[') continue;
                    var old = JsonConvert.DeserializeObject<Dictionary<int, List<TattooModel>>>(json);
                    var newTattoo = new List<Decoration>();
                    if (old.Count > 0)
                    {
                        foreach (var part in old)
                        {
                            foreach (var tattoo in part.Value)
                            {
                                newTattoo.Add(new Decoration { Collection = NAPI.Util.GetHashKey(tattoo.Dictionary), Overlay = NAPI.Util.GetHashKey(tattoo.Hash) });
                            }
                        }
                    };                    
                    var id = Convert.ToInt32(row["id"]);
                    MySQL.Query("UPDATE `newcustomization` SET `tattoos` = @prop0 WHERE `id`=@prop1;",
                        JsonConvert.SerializeObject(newTattoo),
                        id
                     );
                }
                catch
                {
                    continue;
                }
            }
        }
    }
}
