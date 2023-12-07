using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.SDK;
using Whistler.Businesses;
using Whistler.Houses;
using Whistler.Core;
using Newtonsoft.Json;
using Whistler.DTOs.Businesses;

namespace Whistler.Businesses
{
    class BusinessEvents : Script
    {
        [Command("openbizsetts")]
        public static void OpenBusinessSettingsCommand(Player player, int type)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "openbizsetts"))
                    return;

                var bizSettings = BusinessesSettings.GetBusinessSettings(type);
                if (bizSettings == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.Center, "local_82", 3000);
                    return;
                }

                var menuDTO = new MenuSettingsDTO()
                {
                    TypeName = bizSettings.TypeName,
                    Items = bizSettings.Products.ToArray()
                };

                player.SetData("BIZSETTS:TYPE", type);
                player.TriggerEvent("bizsetts:open", JsonConvert.SerializeObject(menuDTO));
            }
            catch
            {
                return;
            }
        }

        [RemoteEvent("bizsetts::changeOrderPrice")]
        public static void RemoteEvent_ChangeOrderPrice(Player player, int value, string productName)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "changeorderprice"))
                    return;

                if (BusinessesSettings.ChangeOrderPrice((int)player.GetData<int>("BIZSETTS:TYPE"), productName, value))
                    UpdateBizSettingsData(player, (int)player.GetData<int>("BIZSETTS:TYPE"));
            }
            catch
            {
                return;
            }
        }

        [RemoteEvent("bizsetts::changeMaxPrice")]
        public static void RemoteEvent_ChangeMaxPrice(Player player, int value, string productName)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "changeMaxPrice’"))
                    return;

                if (!BusinessesSettings.ChangeMaxPrice((int)player.GetData<int>("BIZSETTS:TYPE"), productName, value)){
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_83", 3000);
                    return;
                }
                UpdateBizSettingsData(player, (int)player.GetData<int>("BIZSETTS:TYPE"));
            }
            catch
            {
                return;
            }
        }

        [RemoteEvent("bizsetts::changeMinPrice")]
        public static void RemoteEvent_ChangeMinPrice(Player player, int value, string productName)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "changeMinPrice’"))
                    return;

                if (!BusinessesSettings.ChangeMinPrice((int)player.GetData<int>("BIZSETTS:TYPE"), productName, value))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_84", 3000);
                    return;
                }

                UpdateBizSettingsData(player, (int)player.GetData<int>("BIZSETTS:TYPE"));
            }
            catch
            {
                return;
            }
        }

        [RemoteEvent("bizsetts::changeStockCapacity")]
        public static void RemoteEvent_ChangeStockCapactiy(Player player, int value, string productName)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "changeStockCapacity’"))
                    return;

                BusinessesSettings.ChangeStockCapacity((int)player.GetData<int>("BIZSETTS:TYPE"), productName, value);
                UpdateBizSettingsData(player, (int)player.GetData<int>("BIZSETTS:TYPE"));
            }
            catch
            {
                return;
            }   
        }

        [RemoteEvent("bizsetts::delete")]
        public static void RemoteEvent_DeleteProduct(Player player, string productName)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "deleteProduct"))
                    return;

                BusinessesSettings.DeleteProduct((int)player.GetData<int>("BIZSETTS:TYPE"), productName);
                UpdateBizSettingsData(player, (int)player.GetData<int>("BIZSETTS:TYPE"));

                Notify.Send(player, NotifyType.Success, NotifyPosition.Center, "local_85".Translate( productName), 3000);
            }
            catch
            {
                return;
            }
        }

        [RemoteEvent("bizsetts::addnew")]
        public static void RemoteEvent_AddNewProduct(Player player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "addNewProduct"))
                    return;

                player.TriggerEvent("bizsetts:close");
                player.TriggerEvent("openInput", "bizset:prod:add", "bizset:prod:name", 20, "bizsettsAddNewProduct");
            }
            catch
            {
                return;
            }
        }

        private static List<int> _biztypesWithAbilityToAddProducts = new List<int>() { 0, 2, 3, 4, 5, 6, 8, 15, 16, 20, 21, 22, 23, 24, 25, 26, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 28, 39 };
        public static void InputCallback_AddNewProduct(Player player, string productName)
        {
            var type = player.GetData<int>("BIZSETTS:TYPE");

            if (!_biztypesWithAbilityToAddProducts.Contains(type))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_86", 3000);
                return;
            }

            BusinessesSettings.AddNewProduct(type, productName, "$");
            Notify.Send(player, NotifyType.Success, NotifyPosition.Center, "local_87".Translate( productName), 3000);
        }



        private static void UpdateBizSettingsData(Player player, int biztype)
        {
            var bizSettings = BusinessesSettings.GetBusinessSettings(biztype);
            var menuDTO = new MenuSettingsDTO()
            {
                TypeName = bizSettings.TypeName,
                Items = bizSettings.Products.ToArray()
            };

            player.TriggerEvent("bizsetts:updateData", JsonConvert.SerializeObject(menuDTO));
        }

        [Command("changeminpercent")]
        public static void CangeMinPercent(Player player, int type, int newPercent)
        {
            if (!Group.CanUseAdminCommand(player, "changeminpercent"))
                return;

            var bizSettings = BusinessesSettings.GetBusinessSettings(type);
            if (bizSettings == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.Center, "local_82", 3000);
                return;
            }
            bizSettings.ChangeMinimumPercentProduct(newPercent);
        }

        [Command("changebiztypename")]
        public static void ChangeBizTypeName(Player player, int type, string name)
        {
            if (!Group.CanUseAdminCommand(player, "changeminpercent"))
                return;

            var bizSettings = BusinessesSettings.GetBusinessSettings(type);
            if (bizSettings == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.Center, "local_82", 3000);
                return;
            }
            bizSettings.ChangeTypeName(name);
        }
    }
}
