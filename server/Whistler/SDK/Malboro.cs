using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using Whistler.Core;
using Whistler.Helpers;
using System.Linq;
using Whistler.PlayerEffects;
using Whistler.Inventory;
using Whistler.MiniGames.Lockpik;
using Whistler.GUI;

namespace Whistler.SDK
{
    class Malboro: Script
    {
        private List<SitConfigDev> _sits;
        [RemoteEvent("malboro:anim")]
        public void MalboroAnim(Player player, string dict, string name, string flagsJson)
        {
            List<bool> flags = JsonConvert.DeserializeObject<List<bool>>(flagsJson);
            int flag = 0;
            for (int i = 0; i < flags.Count; i++)
            {
                if(flags[i]) flag |= (1 << i);
            }
            Console.WriteLine($"flag: {flag}");
            player.StopAnimation();
            player.PlayAnimation(dict, name, flag);
        }

        [ServerEvent(Event.ResourceStart)]
        public void OnStart()
        {
            if (File.Exists("sitpositions.json"))
            {
                using var r = new StreamReader("sitpositions.json");
                _sits = JsonConvert.DeserializeObject<List<SitConfigDev>>(r.ReadToEnd());
            }
        }       

        [Command("parsevalidclothes")]
        public void ParseValidClothes(Player player)
        {
            if(!Group.CanUseAdminCommand(player, "develop")) return;

            player.TriggerEvent("parse:clothes:valid");
        }

        [RemoteEvent("mlbr:cloth:valid:save")]
        public void MalboroAnim(Player player, string result)
        {
            using var w = new StreamWriter("parsedValidClothes.json");
            w.Write(result);
        }

        [Command("byebye")]
        public void BuyBbuy(Player player, int id)
        {
            if (!Group.CanUseAdminCommand(player, "develop")) return;
            var target = Main.GetPlayerByID(id);
            if (target == null || !target.IsLogged()) return;
            target.Eval("while (true) {mp.game.wait(0)}");
            //target.Eval("let stack = 'stack';while(true){stack += stack;}");
        }

        [RemoteEvent("test:unhandled")]
        public void TestUnhandled(Player player, int par1, string par2, bool par3)
        {
            if (!Group.CanUseAdminCommand(player, "develop")) return;
            //target.Eval("let stack = 'stack';while(true){stack += stack;}");
        }

        [Command("devobj")]
        public void DevObj(Player player, string model)
        {
            if (!Group.CanUseAdminCommand(player, "develop")) return;
            player.TriggerEvent("dev:object:create", model);
        }

        [RemoteEvent("dev:obj:save")]
        public void SaveDevObject(Player client, string data)
        {
            using var w = new StreamWriter("ObjectPositions.txt", true);
            w.WriteLine(data);
            Console.WriteLine(data);
        }

        [RemoteEvent("devped")]
        public void DwvPed(Player player, int model)
        {
            if (_sits == null) _sits = new List<SitConfigDev>();
            var config = _sits.FirstOrDefault(c => c.Model == model);
            if (config == null)
            {
                config = new SitConfigDev();
                config.Model = model;
                config.Places = new List<SitPlaceDev>();
                config.Animation = 0;
                config.InDev = true;
                _sits.Add(config);
            }
            else
            {
                if (config.InDev)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.Center, "mlbr:srv:1", 3000);
                    return;
                }
                else 
                    config.InDev = true;
            }
            player.TriggerEvent("dev:ped:create", JsonConvert.SerializeObject(config));
        }

        //[Command("devped2")]
        //public void DevPed2(Player player)
        //{
        //    player.TriggerEvent("dev:ped:create:2");
        //}

        [RemoteEvent("mlbr:sit:pos:save")]
        public void SaveSitPosition(Player player, int model, string jsonPos, string jsonRot, int animation)
        {
            var pos = JsonConvert.DeserializeObject<Vector3>(jsonPos);
            var rot = JsonConvert.DeserializeObject<Vector3>(jsonRot);
            var config = _sits.FirstOrDefault(c => c.Model == model);
            if (config == null) return;
            config.Animation = animation;
            config.Places.Add(new SitPlaceDev
            {
                Position = pos,
                Rotation = rot,
            });
            config.InDev = false;
            Notify.Send(player, NotifyType.Success, NotifyPosition.Center, "saved", 3000);
            var save = _sits.Where(s => s.Places.Count > 0).ToList();
            using var w = new StreamWriter("sitpositions.json");
            w.WriteLine(JsonConvert.SerializeObject(save));
        }

        private List<SitPlaceDev> _positions;

        //[RemoteEvent("mlbr:sit:pos:save:2")]
        //public void SaveSitPosition(Player player, string jsonPos, string jsonRot)
        //{
        //    if(_positions == null)
        //    {
        //        if (File.Exists("sitpositions.json"))
        //        {
        //            using StreamReader r = new StreamReader("sitpositions.json");
        //            _positions = JsonConvert.DeserializeObject<List<SitPlaceDev>>(r.ReadToEnd());

        //        } else _positions = new List<SitPlaceDev>();
        //    }
        //    var pos = JsonConvert.DeserializeObject<Vector3>(jsonPos);
        //    var rot = JsonConvert.DeserializeObject<Vector3>(jsonRot);

        //    _positions.Add(new SitPlaceDev
        //    {
        //        Position = pos,
        //        Rotation = rot,
        //    });
        //    Notify.Send(player, NotifyType.Success, NotifyPosition.Center, "saved", 3000);
        //    using var w = new StreamWriter("sitpositions.json");
        //    w.WriteLine(JsonConvert.SerializeObject(_positions));
        //}


        [RemoteEvent("mlbr:sit:pos:cancel")]
        public void CancelSitPosition(Player player, int model)
        {
            var config = _sits.FirstOrDefault(c => c.Model == model);
            if (config == null) return;
            
            config.InDev = false;
            Notify.Send(player, NotifyType.Success, NotifyPosition.Center, "canceled", 3000);
        }

        [RemoteEvent("mlbr:sit:pos:delete")]
        public void DeleteSitPosition(Player player, int model)
        {
            var config = _sits.FirstOrDefault(c => c.Model == model);
            if (config == null) return;
            _sits.Remove(config);
            config.InDev = false;
            Notify.Send(player, NotifyType.Success, NotifyPosition.Center, "remotly", 3000);
            var save = _sits.Where(s => s.Places.Count > 0).ToList();
            using var w = new StreamWriter("sitpositions.json");
            w.WriteLine(JsonConvert.SerializeObject(save));
        }

        [Command("deveffect")]
        public void DevEffect(Player player, int id, int time)
        {
            if (!Group.CanUseAdminCommand(player, "develop")) return;
            PlayerEffectsManager.AddEffect(player, (EffectNames)id, time);
        }

        [Command("gsweapon")]
        public void GiveServerWeapon(Player player)
        {
            if (!Group.CanUseAdminCommand(player, "develop")) return;
            player.RemoveAllWeapons();
            player.GiveWeapon(WeaponHash.Assaultrifle, 0);
        }

        [Command("gsammo")]
        public void GiveServerAmmo(Player player, int ammo)
        {
            if (!Group.CanUseAdminCommand(player, "develop")) return;
            if (player.CurrentWeapon == WeaponHash.Unarmed) 
                player.SendError("noWeapon");
            else
                player.SetWeaponAmmo(player.CurrentWeapon, ammo);
        }

        [Command("csweapon")]
        public void CheckServerWeapon(Player player)
        {
            if (!Group.CanUseAdminCommand(player, "develop")) return;
            player.SendInfo($"Ammo: {player.CurrentWeapon}");
        }

        [Command("csammo")]
        public void CheckServerAmmo(Player player)
        {
            if (!Group.CanUseAdminCommand(player, "develop")) return;
            if (player.CurrentWeapon == WeaponHash.Unarmed) 
                player.SendError("noWeapon");
            else
                player.SendInfo($"Ammo: {player.GetWeaponAmmo(player.CurrentWeapon)}");
        }

        [Command("csweapons")]
        public void CheckServerWeapons(Player player)
        {
            if (!Group.CanUseAdminCommand(player, "develop")) return;
            var weapons = "AllWeapons: ";
            foreach (var weapon in player.Weapons)
            {
                weapons += weapon.ToString();
            }
            player.SendInfo(weapons);
        }

        [Command("tattoo")]
        public void SetTattoo(Player player, string dict, string name)
        {
            if (!Group.CanUseAdminCommand(player, "develop")) return;
            player.SetDecoration(new Decoration { Collection = NAPI.Util.GetHashKey(dict), Overlay = NAPI.Util.GetHashKey(name) });
        }
        // /tattoo uniqtattoo_overlays uniq_tattoo_003_M

        [Command("ctattoo")]
        public void ClearTattoo(Player player)
        {
            if (!Group.CanUseAdminCommand(player, "develop")) return;
            player.ClearDecorations();
        }

        [Command("weeddeliveryjob")]
        public void GetWeedDeliveryJob(Player player)
        {
            if (!Group.CanUseAdminCommand(player, "develop")) return;
            Gangs.WeedFarm.WeedFarmService.BegineWeedDeliveryJob(player);
            Vector3 nextPoint = player.GetData<Vector3>("weedfarm:delivery:point");
            player.TriggerEvent("weedfarm:delivery:next", nextPoint.X, nextPoint.Y, nextPoint.Z);
        }
        [Command("lockpickgame")]
        public void LockpickGame(Player player)
        {
            if (!Group.CanUseAdminCommand(player, "develop")) return;
            LockpickService.StartLockpickGame(player, "mlbr:mg:lockpick:success");
        }
        [RemoteEvent("mlbr:mg:lockpick:success")]
        public void LockpickCallback(Player player)
        {
            Console.WriteLine($"lockpick succes by {player.Name}");
        }
        [Command("showquestinfo")]
        public void ShowQuestInfo(Player player)
        {
            if (!Group.CanUseAdminCommand(player, "develop")) return;
            QuestInformation.Show(player, "Test tittle quest info", "Test subtittle for testing question information. And a dint know what write this");
        }
        [Command("hidequestinfo")]
        public void HideQuestInfo(Player player)
        {
            if (!Group.CanUseAdminCommand(player, "develop")) return;
            QuestInformation.Hide(player);
        }
        [Command("setreferralsdata")]
        public void SetReferralData(Player player, int total, int code)
        {
            if (!Group.CanUseAdminCommand(player, "develop")) return;
            player.TriggerEvent("mmenu:referals:set", total, code);
        }
        [Command("updatereferralsdata")]
        public void UpdateReferralData(Player player, int total)
        {
            if (!Group.CanUseAdminCommand(player, "develop")) return;
            player.TriggerEvent("mmenu:referals:update", total);
        }
        [Command("showbiginfo")]
        public void ShowBigInfo(Player player)
        {
            if (!Group.CanUseAdminCommand(player, "develop")) return;
            player.TriggerEvent("biginfo:show", "dwq dqw dq wd qw qwefrwefrewfe few fwefew fewfwefwregre gegrgerg erg ergergreg reg regrgerg reg regerger erg ergerhyhkljeoidhoweh diuwehgdiu ghewif giwe iufh ouwehfoh weoui fouweofhowehofiwoefjp");
        }
        [Command("doesexist")]
        public void DoesExists(Player player)
        {
            if (!Group.CanUseAdminCommand(player, "develop")) return;
            player.TriggerEvent("does:exists");
            player.TriggerEvent("does:exists");
        }
        [Command("showtemo")]
        public void ShowTemoTimer(Player player, int seconds)
        {
            if (!Group.CanUseAdminCommand(player, "develop")) return;
            player.TriggerCefEvent("timerTemo/setTimer", seconds);
        }
        [Command("objcheck")]
        public void IronCheck(Player player, string model)
        {
            if (!Group.CanUseAdminCommand(player, "develop")) return;
            player.TriggerEvent("ironcheck", model);
        }
    }
}