using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Whistler.Entities;
using Whistler.Fractions.Models;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Fractions.PDA
{
    class PoliceArrests
    {

        private static Dictionary<int, ArrestedModel> _arrests = new Dictionary<int, ArrestedModel>();
        public static void LoadArrests()
        {
            DataTable result = MySQL.QueryRead("SELECT ar.*, ch.licenses, ch.gender FROM `arrests` ar JOIN `characters` ch ON ar.uuid = ch.uuid WHERE ar.arrestdate > @prop0 OR ar.arrestdate > ar.releasedate AND ar.arrestdate > @prop1", MySQL.ConvertTime(DateTime.Now.AddDays(-7)), MySQL.ConvertTime(DateTime.Now.AddDays(-30)));
            if (result == null || result.Rows.Count == 0)
            {
                return;
            }
            foreach (DataRow row in result.Rows)
            {
                var arrest = new ArrestedModel(row);
                _arrests.Add(arrest.Id, arrest);
            }
        }
        public static void NewArrest(ExtPlayer player, ExtPlayer target, string reason)
        {
            var arrest = new ArrestedModel(target, player, reason);
            _arrests.Add(arrest.Id, arrest);
            PersonalDigitalAssistant.UpdateArrestData(arrest);
        }

        public static ArrestedModel GetArrested(int id)
        {
            if (!_arrests.ContainsKey(id))
                return null;
            return _arrests[id];
        }

        public static IEnumerable<DTO.ArrestedModelDTO> GetArrestedModels()
        {
            return _arrests.Select(item => item.Value.GetArrestedModelDTO());
        }

        public static ArrestedModel GetLastArrestedModel(ExtPlayer player)
        {
            var playerArrests = _arrests.Where(item => item.Value.Uuid == player.Character.UUID);
            if (playerArrests.Count() > 0)
            {
                var currArrest = playerArrests.Last();
                if (currArrest.Value.ReleaseDate < currArrest.Value.ArrestDate)
                {
                    return currArrest.Value;
                }
            }
            return null;
        }

        public static void ReleasePlayer(ExtPlayer player, ExtPlayer bailPlayer, int price, ArrestedModel arrestedModel = null)
        {
            if (!player.IsLogged())
                return;
            player.Character.ResetArrestTimer(player);
            player.Character.ArrestDate = DateTime.Now;
            player.ChangePosition(FractionCommands.KpzOutPosition);
            Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Frac_239", 3000);
            SetRecordAboutReleasePlayer(player, bailPlayer, price, arrestedModel);
        }
        public static void SetRecordAboutReleasePlayer(ExtPlayer player, ExtPlayer bailPlayer, int price, ArrestedModel arrestedModel = null)
        {
            if (!player.IsLogged())
                return;
            SafeTrigger.ClientEvent(player,"hud:arrest:timer:reset");
            if (arrestedModel == null)
                GetLastArrestedModel(player)?.Release(price, bailPlayer?.Character?.UUID ?? -1);
            else
                arrestedModel.Release(price, bailPlayer?.Character?.UUID ?? -1);
        }
    }
}
