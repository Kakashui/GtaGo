using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Whistler.Core;
using Whistler.Core.Character;
using Whistler.Domain.Phone.News;
using Whistler.Entities;
using Whistler.Fractions;
using Whistler.Fractions.LSNews;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.MoneySystem;
using Whistler.Phone.News.Dtos;
using Whistler.SDK;

namespace Whistler.Phone.News
{
    class AdvertHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(AdvertHandler));
        private static DateTime _loadDateAdvert = DateTime.Now.AddHours(-10);
        private static int priceSymbol = 3;
        private static int priceImage = 3000;

        public AdvertHandler()
        {
            PhoneLoader.PhoneReadyAsync += LoadAdvertsForPhone;
        }

        private static int GetPriceAdvert(string text, bool isPrime)
        {
            return text.Length * priceSymbol + (isPrime ? priceImage : 0);
        }

        [RemoteEvent("phone::news::createAdvert")]
        public void CreateAdvertEvent(ExtPlayer player, string text, string imageUrl)
        {
            var character = player.Character;
            if (character == null)
                return;
            CreateAdvert(player, character, text, imageUrl);
        }

        private static async void CreateAdvert(ExtPlayer player, Character character, string text, string imageUrl)
        {
            try
            {
                imageUrl = imageUrl == "" ? null : imageUrl;
                bool prime = imageUrl != null;
                var price = GetPriceAdvert(text, prime);
                if (!Wallet.TransferMoney(player.Character.BankModel, Manager.GetFraction(15), price, 0, "Новое объявление"))
                {
                    Notify.SendError(player, "phone:adv:add:err");
                    return;
                }
                Notify.SendSuccess(player, "phone:adv:add:ok");
                Advert advert = new Advert
                {
                    DateCreate = DateTime.Now,
                    SenderUUID = character.UUID,
                    EditorUUID = -1,
                    PhoneNumber = character.PhoneTemporary?.Phone?.SimCard?.Number ?? 0,
                    MessengerLogin = character.PhoneTemporary?.Phone?.Account?.Username,
                    ImageUrl = imageUrl,
                    Text = text,
                    Status = AdvertStatus.Created,
                    PrimeAdvert = prime
                };

                using (var context = DbManager.TemporaryContext)
                {
                    context.Adverts.Add(advert);
                    await context.SaveChangesAsync();
                }
                SubscribeToLSNews.TriggerCefEventSubscribers("news/updateAdvert", JsonConvert.SerializeObject(new AdvertDto(advert)));

            }
            catch (Exception e)
            {
                NAPI.Task.Run(() => _logger.WriteError($"CreateAdvert: {e.ToString()}"));
            }
        }

        public static async void LoadAdvertsForRedactor(ExtPlayer player)
        {
            try
            {
                using (var context = DbManager.TemporaryContext)
                {
                    var adverts = await context.Adverts
                        .Where(item => item.Status == AdvertStatus.Created || item.DateCompleate > _loadDateAdvert)
                        .ToListAsync();
                    player.TriggerCefEvent("news/setAdsList", JsonConvert.SerializeObject(adverts.Select(item => new AdvertDto(item))));
                }
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() => _logger.WriteError($"LoadAdvertsForRedactor: {e.ToString()}"));
            }
        }
        public static async Task LoadAdvertsForPhone(ExtPlayer player, Character character)
        {
            try
            {
                using (var context = DbManager.TemporaryContext)
                {
                    var adverts = await context.Adverts
                        .Where(item => item.Status == AdvertStatus.Compleate && item.DateCompleate > _loadDateAdvert)
                        .OrderByDescending(item => item.DateCompleate)
                        .ToListAsync();
                    player.TriggerCefEvent("smartphone/newsPage/setNewsData", JsonConvert.SerializeObject(adverts.Select(item => new AdvertDto(item))));
                }
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() => _logger.WriteError($"LoadAdvertsForPhone: {e.ToString()}"));
            }
        }

        public static async void UpdateAdvert(ExtPlayer editor, int advertId, AdvertStatus status, string text, string image)
        {
            try
            {
                using (var context = DbManager.TemporaryContext)
                {
                    var advert = context.Adverts.FirstOrDefault(item => item.Id == advertId);
                    if (advert.Status != AdvertStatus.Created)
                        return;
                    if (advert.PrimeAdvert)
                    {
                        if (!Manager.CanUseCommand(editor, "editpriveadvert"))
                            return;
                    }
                    else
                    {
                        if (!Manager.CanUseCommand(editor, "editnotpriveadvert"))
                            return;
                    }
                    advert.Status = status;
                    advert.EditorUUID = editor.Character.UUID;
                    advert.DateCompleate = DateTime.Now;
                    if (status == AdvertStatus.Compleate)
                    {
                        if (advert.ImageUrl != image)
                            advert.ImageUrl = null;
                        advert.Text = text;
                    }
                    await context.SaveChangesAsync();
                    var advertDTO = new AdvertDto(advert);
                    SubscribeToLSNews.TriggerCefEventSubscribers("news/updateAdvert", JsonConvert.SerializeObject(advertDTO));
                    if (advert.Status == AdvertStatus.Compleate)
                    {
                        NAPI.Task.Run(() =>
                        {
                            Chat.Advert(editor, advert.Text, advertDTO.Sender, advert.PhoneNumber);
                            SafeTrigger.ClientCefEventToAllPlayers("smartphone/newsPage/addNews", JsonConvert.SerializeObject(advertDTO));
                        });
                    }
                    else
                    {
                        NAPI.Task.Run(() => {
                            var price = GetPriceAdvert(advert.Text, advert.PrimeAdvert);
                            Wallet.TransferMoney(Manager.GetFraction(15), BankManager.GetAccountByUUID(advert.SenderUUID), price, 0, "Money_RevertAdvert");
                            var player = Trigger.GetPlayerByUuid(advert.SenderUUID);
                            if (player != null)
                                Notify.SendError(player, "phone:adv:add:cncl");
                        });
                    }
                }
                return;
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() => _logger.WriteError($"UpdateAdvert: {e.ToString()}"));
            }
        }

        public static List<Advert> GetAdverts()
        {
            try
            {
                using (var context = DbManager.TemporaryContext)
                {
                    var adverts = context.Adverts
                        .Where(item => item.Status != AdvertStatus.Created && item.DateCompleate > DateTime.Now.AddMonths(-1))
                        .ToList();
                    return adverts;
                }
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() => _logger.WriteError($"GetAdverts: {e.ToString()}"));
                return null;
            }
        }
    }
}
