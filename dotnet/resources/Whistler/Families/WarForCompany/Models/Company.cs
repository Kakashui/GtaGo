using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Common;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Families.FamilyMenu;
using Whistler.Families.WarForCompany.DTO;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.Possessions;
using Whistler.SDK;

namespace Whistler.Families.WarForCompany.Models
{
    class Company
    {
        public int ID { get; set; }
        public Vector3 Position { get; set; }
        public double Rotation { get; set; }
        public WarCompanies Key { get; set; }
        public int OwnerId { get; set; }
        public OwnerType OwnerType { get; set; }
        public DateTime DateOfCapture { get; set; }


        public bool IsGoing;
        private InteractShape Shape;
        private ColShape ColShape;
        private Blip Blip;
        private string _timer = null;
        private ExtPlayer _attaker = null;
        public int AttackID = -1;
        public OwnerType AttackType = OwnerType.Family;
        public DateTime StartTime = DateTime.Now;
        public Company(DataRow row)
        {
            ID = Convert.ToInt32(row["id"].ToString());
            Position = JsonConvert.DeserializeObject<Vector3>(row["position"].ToString());
            Rotation = Convert.ToDouble(row["rotation"].ToString());
            Key = (WarCompanies)Convert.ToInt32(row["key"].ToString());
            OwnerId = Convert.ToInt32(row["ownerid"].ToString());
            OwnerType = (OwnerType)Convert.ToInt32(row["ownertype"].ToString());
            DateOfCapture = Convert.ToDateTime(row["datecapt"].ToString());
            IsGoing = false;
            CreateInteract();
        }
        public Company(Vector3 position, WarCompanies key)
        {
            Position = position;
            Rotation = 0;
            Key = key;
            OwnerId = 0;
            OwnerType = OwnerType.Family;
            DateOfCapture = DateTime.Now;
            IsGoing = false;
            var responce = MySQL.QueryRead("INSERT INTO `familycompanies`(`position`, `rotation`, `key`, `ownerid`, `ownertype`, `datecapt`) VALUES(@prop0, @prop1, @prop2, @prop3, @prop4, @prop5); SELECT @@identity;", JsonConvert.SerializeObject(Position), Rotation, (int)Key, OwnerId, (int)OwnerType, MySQL.ConvertTime(DateOfCapture));
            ID = Convert.ToInt32(responce.Rows[0][0]);
            CreateInteract();
        }
        private void CreateInteract()
        {
            Blip = NAPI.Blip.CreateBlip(181, Position, 1, 1, "Компания", 255, 0, true, 0, 0);
            Shape = InteractShape.Create(Position, 2, 2)
                .AddInteraction((player) =>
                {
                    ActionShape(player);
                }, "interact_45");
        }

        private void ActionShape(ExtPlayer player, Families.Models.Family family = null, Fractions.Models.Fraction fraction = null, bool selected = false)
        {
            if (!player.IsLogged()) return;

            if (!selected)
            {
                family = player.GetFamily();
                fraction = player.GetFraction();
            }
            if (family == null && fraction == null) return;

            int ownerId = -1;
            OwnerType ownerType = OwnerType.Family;
            if (family != null && fraction != null)
            {
                DialogUI.Open(player, $"От лица кого Вы хотите захватить компанию?", new List<DialogUI.ButtonSetting>
                {
                    new DialogUI.ButtonSetting
                    {
                        Name = "Семья",
                        Icon = null,
                        Action = (p) =>
                        {
                            ActionShape(p, family, null, true);
                        }
                    },
                    new DialogUI.ButtonSetting
                    {
                        Name = "Организация",
                        Icon = null,
                        Action = (p) =>
                        {
                            ActionShape(p, null, fraction, true);
                        }
                    },
                    new DialogUI.ButtonSetting
                    {
                        Name = "Отменить",
                        Icon = null,
                        Action = { }
                    }
                });
                return;
            }
            else if (family != null && fraction == null)
            {
                ownerId = family.Id;
                ownerType = OwnerType.Family;
            }
            else if (family == null && fraction != null)
            {
                ownerId = fraction.Id;
                ownerType = OwnerType.Fraction;
            }

            if (ownerId < 0)
            {
                Notify.SendError(player, "warCompany:badCapt");
                return;
            }
            if (ownerId == OwnerId && ownerType == OwnerType)
            {
                Notify.SendError(player, "warCompany:itsTourCompany");
                return;
            }
            if (_attaker.IsLogged())
            {
                Notify.SendError(player, "warCompany:companyIsCaptures");
                return;
            }
            ColShape = NAPI.ColShape.CreateCylinderColShape(Position, 50, 10, 0);
            ColShape.OnEntityExitColShape += ColShape_OnEntityExitColShape;
            AttackID = ownerId;
            AttackType = ownerType;
            _attaker = player;
            StartTime = DateTime.Now;
            _timer = Timers.StartOnce(1000 * WarConfigs.SECONDS_FOR_CAPT_COMPANY, CompleateCaptureCompany);
            IsGoing = true;
            Blip.Color = 46;
            Blip.Scale = 1.5F;
            player.TriggerCefEvent("hud/warForEnterprice/setWarForEnterprice", JsonConvert.SerializeObject(new 
            {
                time = WarConfigs.SECONDS_FOR_CAPT_COMPANY,
                currentEnterprice = Key.ToString(),
            }));
            SendCefActionForSubscribers("hud/warForEnterprice/addCapture", JsonConvert.SerializeObject(new CompanyAttackDTO(this)));
            WarCompanyManager.UpdateWar(this);
        }

        private void ColShape_OnEntityExitColShape(ColShape colShape, Player player)
        {
            if (_attaker == player)
            {
                _attaker.TriggerCefEvent("hud/warForEnterprice/setWarForEnterprice", JsonConvert.SerializeObject(new
                {
                    time = (string)null,
                    currentEnterprice = Key.ToString(),
                }));
                ClearData();
            }
        }

        private void CompleateCaptureCompany()
        {
            if (!_attaker.IsLogged())
                return;
            _attaker.TriggerCefEvent("hud/warForEnterprice/setWarForEnterprice", JsonConvert.SerializeObject(new
            {
                time = (string)null,
                currentEnterprice = Key.ToString(),
            }));
            OwnerId = AttackID;
            OwnerType = AttackType;
            DateOfCapture = DateTime.Now;
            MySQL.Query("UPDATE `familycompanies` SET `datecapt` = @prop0, `ownerid` = @prop1, `ownertype` = @prop2 WHERE `id` = @prop3", MySQL.ConvertTime(DateOfCapture), OwnerId, OwnerType, ID);
            ClearData();
        }

        private void ClearData()
        {
            _attaker = null;
            if (_timer != null)
            {
                Timers.Stop(_timer);
                _timer = null;
            }
            if (ColShape != null)
            {
                ColShape.Delete();
                ColShape = null;
            }
            IsGoing = false;
            Blip.Color = 1;
            Blip.Scale = 1;
            SendCefActionForSubscribers("hud/warForEnterprice/deleteCapture", JsonConvert.SerializeObject(ID));
            WarCompanyManager.UpdateWar(this);
        }

        private void SendCefActionForSubscribers(string eventName, object data)
        {
            FamilyMenu.SubscribeSystem.TriggerCefEventToSubscribeAllFamily(eventName, data);
            Fractions.FractionSubscribeSystem.TriggerCefEventSubscribers(WarCompanyManager._fractionsInWar, eventName, data);
        }

        public void Destroy()
        {
            Shape?.Destroy();
            Blip?.Delete();
        }

        public bool DisconnectedPlayer(ExtPlayer player)
        {
            if (_attaker == player)
            {

                ClearData();
                return true;
            }
            return false;
        }

        public bool PlayerDeath(ExtPlayer player)
        {
            if (_attaker == player)
            {
                _attaker.TriggerCefEvent("hud/warForEnterprice/setWarForEnterprice", JsonConvert.SerializeObject(new
                {
                    time = (string)null,
                    currentEnterprice = Key.ToString(),
                }));
                ClearData();
                return true;
            }
            return false;
        }

        public void ChangePosition(ExtPlayer player)
        {
            if (IsGoing) return;
            Position = player.Position;
            Rotation = player.Rotation.Z;
            Destroy();
            CreateInteract();
            MySQL.Query("UPDATE `familycompanies` SET `position` = @prop0, `rotation` = @prop1 WHERE `id` = @prop2", JsonConvert.SerializeObject(Position), Rotation, ID);

            NAPI.ClientEvent.TriggerClientEventForAll("warForEnterprice:updatePeds", JsonConvert.SerializeObject(new { id = ID, position = Position, heading = Rotation }));

        }
    }
}
