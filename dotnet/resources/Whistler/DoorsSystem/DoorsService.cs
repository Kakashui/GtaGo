using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Whistler.Entities;
using Whistler.Families;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.DoorsSystem
{
    public static class DoorsService
    {
        private static DoorsConfig _config = new DoorsConfig();
        private static Dictionary<int, string> _accessList = new Dictionary<int, string>();
        public static void ParseDoorsConfigs()
        {
            if (Directory.Exists("client/doors"))
            {
                using (var w = new StreamWriter("client/doors/configs.js"))
                {
                    w.Write($"module.exports = {_config.Serialize()}");
                }
            };
        }

        public static void SetDoorState(ExtPlayer player, int hash, bool state)
        {
            var door = _config[hash];   
            if (door == null) return;
            if (!HasDoorAccess(player, door))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Нет доступа", 3000);
                //Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"{door.Name} {door.Hash}", 3000);
                return;
            }
            if(door.Locked == state)
            {
                door.UpdateStateForPlayer(player);
                return;
            }
            door.SetState(state);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, door.Locked ? "Вы закрыли двери" : "Вы открыли двери", 3000);
        }

        public static void SyncDoorStateForPlayer(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player,"doors:state:sync", _config.Changed);
        }

        private static bool HasDoorAccess(ExtPlayer player, DoorComplect door)
        {
            // if (player.Character.AdminLVL > 2) return true;
            // if (player.Character.MediaHelper > 0) return true;
            if (!_accessList.ContainsKey(door.Hash)) return false;

            string access = _accessList[door.Hash];
            if(Fractions.Manager.CanUseCommand(player, access, false)) return true;

            if (FamilyManager.CanAccessToDoor(player, access)) return true;
            return false;
        }

        public static void LoadAccess()
        {
            var query = $"CREATE TABLE IF NOT EXISTS `dooraccess`(" +
                $"`id` int(11) NOT NULL AUTO_INCREMENT," +
                $"`uuid` int(11) NOT NULL," +
                $"`accessname` varchar(45)," +
                $"PRIMARY KEY(`id`)" +
                $")ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";
            MySQL.Query(query);

            query = "SELECT * FROM `dooraccess`";
            var responce = MySQL.QueryRead(query);
            if(responce.Rows.Count > 0)
            {
                foreach (DataRow row in responce.Rows)
                {
                    var uuid = Convert.ToInt32(row["uuid"]);
                    var accessname = row["accessname"].ToString();
                    _accessList.Add(uuid, accessname);
                }
            }
        }
        public static void AddDoorAccess(ExtPlayer player,int uuid, string access)
        {
            if (_accessList.ContainsKey(uuid))
            {
                _accessList[uuid] = access;
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Доступ для этой двери изменен на: {access}", 3000);
                MySQL.Query("UPDATE `dooraccess` SET `accessname`=@prop0 WHERE `uuid`=@prop1", access, uuid);
            }
            else
            {
                _accessList.Add(uuid, access);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Добавлен доступ для этой двери: {access}", 3000);
                MySQL.Query("INSERT INTO `dooraccess` (`uuid`,`accessname`) VALUES (@prop0, @prop1)", uuid, access);
            }
        }

        public static void CheckDoorAccess(ExtPlayer player, int uuid)
        {
            if (_accessList.ContainsKey(uuid))
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Доступ для этой двери: {_accessList[uuid]}", 3000);
            else
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Доступ для этой двери доступ не установлен", 3000);
        }
    }
}
