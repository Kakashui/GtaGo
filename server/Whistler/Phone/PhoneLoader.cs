using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Whistler.Core.Character;
using Whistler.Domain.Phone;
using Whistler.Domain.Phone.Contacts;
using Whistler.Domain.Phone.Messenger;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.MoneySystem;
using Whistler.MoneySystem.Models;
using Whistler.Phone.Messenger.Chat;
using Whistler.Phone.Messenger.Contacts;
using Whistler.SDK;
using Whistler.UpdateR;
using PhoneModel = Whistler.Domain.Phone.Phone;

namespace Whistler.Phone
{
    public class PhoneLoader : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(PhoneLoader));
        public static event Func<Player, Character, Task> PhoneReadyAsync;
        public static Func<Character, Task> PhoneDisconnect;

        private PhoneLoader()
        {
            Main.OnPlayerReadyAsync += CreatePhoneIfNotExists;
            PhoneDisconnect += DisconnectInChat.DisconnectChats;
            PhoneDisconnect += Messenger.Accounts.CharacterDisconnect.DisconnectCharacter;
            PhoneDisconnect += ContactsDisconnect.DisconnectCharacter;
        }

        private async Task CreatePhoneIfNotExists(PlayerGo player, Character character)
        {
            try
            {
                if (player == null || character == null) return;
                using (var context = DbManager.TemporaryContext)
                {
                    var phone = await context.Phones
                        .Include(p => p.SimCard)
                        .Include(p => p.Account)
                        .FirstOrDefaultAsync(p => p.CharacterUuid == character.UUID);

                    if (phone == null)
                    {
                        var simcard = await CreateSimcard(context);

                        phone = new PhoneModel
                        {
                            CharacterUuid = character.UUID,
                            InstalledAppsIds = new List<AppId>(),
                            SimCardId = simcard.Id
                        };

                        context.Phones.Add(phone);
                        await context.SaveChangesAsync();
                        DbManager.GlobalContext.Phones.Attach(phone);
                    }

                    character.PhoneTemporary.Phone = phone;
                    character.PhoneTemporary.Account = phone.Account;
                    character.PhoneTemporary.Simcard = phone.SimCard;

                    PhoneReadyAsync?.Invoke(player, character);
                    character.PhoneTemporary?.GetPhoneBankAccount()?.Subscribe(player);
                }

                var number = character.PhoneTemporary.Simcard?.Number.ToString() ?? "-";
                var name = $"{character.FirstName} {character.LastName}";

                player.TriggerCefEvent("smartphone/setPersonalConfiguration",
                    JsonConvert.SerializeObject(new {Number = number, Name = name}));
            }
            catch (Exception e)
            {
                WhistlerTask.Run(() =>
                {
                    WhistlerTask.Run(() => _logger.WriteError($"CreatePhoneIfNotExists: {e.ToString()}"));
                });
            }
        }

        private static Random Random = new Random();
        private async Task<SimCard> CreateSimcard(ServerContext context)
        {
            var numberMain = 1818000000;
            var numberTotal = 1818000000;

            do
            {
                numberTotal = numberMain + Random.Next(0, 999999);
            }
            while ((await context.SimCards.FirstOrDefaultAsync(s => s.Number == numberTotal)) != null);

            var simcard = new SimCard
            {
                Number = numberTotal,
                BankNumber = BankManager.CreateAccount(TypeBankAccount.Phone, 2000).ID
            };

            context.SimCards.Add(simcard);
            await context.SaveChangesAsync();

            return simcard;
        }

        public static bool ChangeNumber(Player player, int newNumber)
        {
            try
            {
                if (!player.IsLogged())
                    return false;

                var character = player.GetCharacter();
                if (character.PhoneTemporary.Simcard == null)
                    return false;
                if (newNumber < 1000000000 || newNumber >= 2000000000)
                    return false;
                using (var context = DbManager.TemporaryContext)
                {
                    if (context.SimCards.FirstOrDefault(s => s.Number == newNumber) != null)
                    {
                        return false;
                    }
                    var simCard = context.SimCards
                        .FirstOrDefault(sim => sim.Id == (character.PhoneTemporary.Simcard.Id));
                    if (simCard == null)
                        return false;
                    simCard.Number = newNumber;
                    context.SaveChanges();
                    character.PhoneTemporary.Simcard.Number = newNumber;

                    var number = character.PhoneTemporary.Simcard.Number.ToString() ?? "-";
                    var name = $"{character.FirstName} {character.LastName}";

                    player.TriggerCefEvent("smartphone/setPersonalConfiguration",
                        JsonConvert.SerializeObject(new { Number = number, Name = name }));
                    return true;
                }
            }
            catch (Exception e)
            {
                WhistlerTask.Run(() => _logger.WriteError($"ChangeNumber: {e.ToString()}"));
                return false;
            }
        }

        public static int CreateBankNumber(int simId)
        {
            try
            {
                using (var context = DbManager.TemporaryContext)
                {
                    var simCard = context.SimCards
                        .FirstOrDefault(sim => sim.Id == simId);
                    if (simCard == null)
                        return 0;
                    simCard.BankNumber = BankManager.CreateAccount(TypeBankAccount.Phone, 2000).ID;
                    context.SaveChanges();
                    return simCard.BankNumber;
                }
            }
            catch (Exception e)
            {
                WhistlerTask.Run(() => _logger.WriteError($"ChangeNumber: {e.ToString()}"));
                return 0;
            }
        }
    }
}
