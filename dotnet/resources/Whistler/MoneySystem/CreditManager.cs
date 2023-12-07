using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Common;
using Whistler.Common.Interfaces;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Houses;
using Whistler.MoneySystem.DTO;
using Whistler.MoneySystem.Models;
using Whistler.MoneySystem.Settings;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models.VehiclesData;

namespace Whistler.MoneySystem
{
    class CreditManager
    {
        public static void CreateCredit(ExtPlayer player, int amount, int days, CreditTypePayment typePayment, int pledgeId, PropertyType pledgeType)
        {
            var character = player.Character;
            if (character == null)
                return;
            if (pledgeType != PropertyType.Invalid)
            {
                IWhistlerProperty property = PropertyMethods.GetWhistlerPropertyByName(pledgeType, pledgeId);
                if (property == null)
                    return;
                switch (property.OwnerType)
                {
                    case OwnerType.Family:
                        switch (property.PropertyType)
                        {
                            case PropertyType.Vehicle:
                                if (!((property as PersonalBaseVehicle)?.CanAccessVehicle(player, AccessType.SellDollars) ?? false))
                                {
                                    Notify.SendSuccess(player, "bank:menu:226");
                                    return;
                                }
                                if ((property as PersonalBaseVehicle).TradePoint != -1)
                                {
                                    Notify.SendSuccess(player, "bank:menu:226");
                                    return;
                                }
                                break;
                            case PropertyType.House:
                                if (!((property as House)?.GetAccess(player, Families.FamilyHouseAccess.FullAccess) ?? false))
                                {
                                    Notify.SendSuccess(player, "bank:menu:226");
                                    return;
                                }
                                break;
                            default:
                                return;
                        }
                        break;
                    case OwnerType.Personal:
                        if (property.OwnerID != character.UUID)
                        {
                            Notify.SendSuccess(player, "bank:menu:226");
                            return;
                        }
                        break;
                    default:
                        return;
                }
                if (property.Pledged)
                {
                    Notify.SendSuccess(player, "bank:menu:225");
                    return;
                }
                int price = (int)(property.CurrentPrice * 0.4);
                if (price < amount)
                {
                    Notify.SendSuccess(player, "bank:menu:227");
                    return;
                }
                property.SetPledged(true);
            }
            else
            {
                if (amount > BankSettings.CreditFastMaxAmount || character.LVL < BankSettings.CreditFastMinLevel)
                {
                    Notify.SendSuccess(player, "bank:menu:228".Translate(BankSettings.CreditFastMinLevel, BankSettings.CreditFastMaxAmount));
                    return;
                }
                if (CheckCreditWithoutPledge(character.UUID))
                {
                    Notify.SendSuccess(player, "bank:menu:239");
                    return;
                }
            }
            var rate = BankSettings.GetPercentByParams(days, amount, pledgeType != PropertyType.Invalid, typePayment);
            var credit = new CreditModel(character.UUID, BankManager.HeadBank.ID, typePayment, amount, days / BankSettings.IntervalCalculatePercent, pledgeId, pledgeType, rate);
            Wallet.MoneyAdd(character.BankModel, amount, "Оформление кредита");
            using (var context = EFCore.DBManager.TemporaryContext)
            {
                context.Credits.Add(credit);
                context.SaveChanges();
                EFCore.DBManager.GlobalContext.Credits.Attach(credit);
            }
            UpdateCredit(player, credit);
            Notify.SendSuccess(player, "bank:menu:229");
        }

        public static void PayCredit(ExtPlayer player, int id, int amount)
        {
            var credit = GetCreditModel(id);
            if (credit != null)
            {
                amount = Math.Min(credit.GetMaxPay(), amount);
                if (Wallet.MoneySub(player.Character.BankModel, amount, "Money_PayCredit".Translate(credit.ID)))
                {
                    credit.PayCredit(amount);
                    credit.CalculatePercent();
                    UpdateCredit(player, credit);
                    Notify.SendSuccess(player, "bank:menu:224");
                }
                else
                    Notify.SendError(player, "bank:menu:218");
            }
        }

        public static void AccrualPercent()
        {
            EFCore.DBManager.GlobalContext.Credits.Where(item => !item.ClosedCredit).ToList().ForEach(item => item.CalculatePercent());

        }

        public static CreditModel GetCreditModel(int id)
        {
            return EFCore.DBManager.GlobalContext.Credits.Find(id);
        }

        public static List<CreditDTO> GetCreditDTOs(ExtPlayer player)
        {
            var credits = EFCore.DBManager.GlobalContext.Credits.Where(item => item.UUID == player.Character.UUID && !item.ClosedCredit);
            return credits.ToList().Where(item => !item.ClosedCredit).Select(item => new CreditDTO(item)).ToList();
        }
        public static void CalculateCreditParams(ExtPlayer player, int amount, int days, CreditTypePayment typePayment, bool isPledge)
        {
            var rate = BankSettings.GetPercentByParams(days, amount, isPledge, typePayment);
            var summ = CreditModel.GetFullAmountCredit(amount, 0, rate, (int)Math.Ceiling((double)days / BankSettings.IntervalCalculatePercent), typePayment);
            player.TriggerCefEvent("bank/credit/setCurrentCreditOptionsResult", JsonConvert.SerializeObject(new { rate = rate, calculatedAmount = summ }));
            
        }
        private static void UpdateCredit(ExtPlayer player, CreditModel credit)
        {
            player.TriggerCefEvent("bank/credit/updateCredit", JsonConvert.SerializeObject(new CreditDTO(credit)));
        }
        
        private static bool CheckCreditWithoutPledge(int uuid)
        {
            return EFCore.DBManager.GlobalContext.Credits.Where(item => item.UUID == uuid && !item.ClosedCredit).FirstOrDefault() != null;
        }
    }
}