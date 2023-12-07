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
using Whistler.Entities;

namespace Whistler.Businesses
{
    class BusinessEvents : Script
    {
        [Command("openbizsetts")]
        public static void OpenBusinessSettingsCommand(ExtPlayer player, int type)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "openbizsetts"))
                    return;

                var bizSettings = BusinessesSettings.GetBusinessSettings(type);
                if (bizSettings == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.Center, "Нет такого типа бизнеса", 3000);
                    return;
                }

                MenuSettingsDTO menuDTO = new MenuSettingsDTO()
                {
                    TypeName = bizSettings.TypeName,
                    Items = bizSettings.Products.ToArray()
                };

                SafeTrigger.SetData(player, "BIZSETTS:TYPE", type);
                SafeTrigger.ClientEvent(player,"bizsetts:open", JsonConvert.SerializeObject(menuDTO));
            }
            catch
            {
                return;
            }
        }

        [RemoteEvent("bizsetts::changeOrderPrice")]
        public static void RemoteEvent_ChangeOrderPrice(ExtPlayer player, int value, string productName)
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
        public static void RemoteEvent_ChangeMaxPrice(ExtPlayer player, int value, string productName)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "changeMaxPrice’"))
                    return;

                if (!BusinessesSettings.ChangeMaxPrice((int)player.GetData<int>("BIZSETTS:TYPE"), productName, value)){
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Не получилось изменить макс. цену, возможна вы указали цену, ниже минимальной", 3000);
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
        public static void RemoteEvent_ChangeMinPrice(ExtPlayer player, int value, string productName)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "changeMinPrice’"))
                    return;

                if (!BusinessesSettings.ChangeMinPrice((int)player.GetData<int>("BIZSETTS:TYPE"), productName, value))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Не получилось изменить мин. цену, возможна вы указали цену, выше максимальной", 3000);
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
        public static void RemoteEvent_ChangeStockCapactiy(ExtPlayer player, int value, string productName)
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
        public static void RemoteEvent_DeleteProduct(ExtPlayer player, string productName)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "deleteProduct"))
                    return;

                BusinessesSettings.DeleteProduct((int)player.GetData<int>("BIZSETTS:TYPE"), productName);
                UpdateBizSettingsData(player, (int)player.GetData<int>("BIZSETTS:TYPE"));

                Notify.Send(player, NotifyType.Success, NotifyPosition.Center, $"Выбранный продукт ({productName}) был успешно удален", 3000);
            }
            catch
            {
                return;
            }
        }

        [RemoteEvent("bizsetts::addnew")]
        public static void RemoteEvent_AddNewProduct(ExtPlayer player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "addNewProduct"))
                    return;

                SafeTrigger.ClientEvent(player,"bizsetts:close");
                SafeTrigger.ClientEvent(player,"openInput", "Добавить новый продукт", "Введите название", 20, "bizsettsAddNewProduct");
            }
            catch
            {
                return;
            }
        }

        private static List<int> _biztypesWithAbilityToAddProducts = new List<int>() { 0, 2, 3, 4, 5, 6, 8, 15, 16, 20, 21, 22, 23, 24, 25, 26, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 28, 39 };
        public static void InputCallback_AddNewProduct(ExtPlayer player, string productName)
        {
            var type = player.GetData<int>("BIZSETTS:TYPE");

            if (!_biztypesWithAbilityToAddProducts.Contains(type))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Невозможно добавить новый продукт для этого типа бизнеса", 3000);
                return;
            }

            BusinessesSettings.AddNewProduct(type, productName, "$");
            Notify.Send(player, NotifyType.Success, NotifyPosition.Center, $"Новый продукт ({productName}) был успешно добавлен", 3000);
        }



        private static void UpdateBizSettingsData(ExtPlayer player, int biztype)
        {
            var bizSettings = BusinessesSettings.GetBusinessSettings(biztype);
            var menuDTO = new MenuSettingsDTO()
            {
                TypeName = bizSettings.TypeName,
                Items = bizSettings.Products.ToArray()
            };

            SafeTrigger.ClientEvent(player,"bizsetts:updateData", JsonConvert.SerializeObject(menuDTO));
        }

        [Command("changeminpercent")]
        public static void CangeMinPercent(ExtPlayer player, int type, int newPercent)
        {
            if (!Group.CanUseAdminCommand(player, "changeminpercent"))
                return;

            var bizSettings = BusinessesSettings.GetBusinessSettings(type);
            if (bizSettings == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.Center, "Нет такого типа бизнеса", 3000);
                return;
            }
            bizSettings.ChangeMinimumPercentProduct(newPercent);
        }

        [Command("changebiztypename")]
        public static void ChangeBizTypeName(ExtPlayer player, int type, string name)
        {
            if (!Group.CanUseAdminCommand(player, "changeminpercent"))
                return;

            var bizSettings = BusinessesSettings.GetBusinessSettings(type);
            if (bizSettings == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.Center, "Нет такого типа бизнеса", 3000);
                return;
            }
            bizSettings.ChangeTypeName(name);
        }

        [Command("bcam")]
        public static void CMD_bcam(ExtPlayer player, int bid)
        {
            if (!Group.CanUseAdminCommand(player, "changeminpercent")) return;


            // Business biz = BizList[player.GetData<int>("BIZ_ID")];
            BusinessManager.GetBusiness(bid)?.SetCam(player.Position);
            //GetHouseById(houseid)?.SetCam(player.Position);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Камера установлена", 3000);
        }
    }
}
