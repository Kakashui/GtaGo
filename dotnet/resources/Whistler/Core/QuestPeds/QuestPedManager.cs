using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.Fractions;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Core.QuestPeds
{
    internal class QuestPedManager : Script 
    {
        public static List<QuestPed> QuestPeds { get; } = new List<QuestPed>();
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(QuestPedManager));

        public QuestPedManager()
        {
            var questPed = new QuestPed(PedHash.FbiSuit01, new Vector3(1853, 2598, 45.67), "Karim_Denz", "");
            questPed.PlayerInteracted += (player, ped) =>
            {
                var descPage =
                    new DialogPage("Чтобы посмотреть весь список  работ, нажмите кнопку M. Там вы найдете большой список доступных вакансий. Выберите ту работу, которая вам по душе", ped.Name, ped.Role)
                        .AddCloseAnswer();
                var workDescriptionPage = new DialogPage("Добрый день! Я сотрудник мэрии, от лица всего государства хочу поздравить вас с выходом на свободу! Надеюсь, нахождение в тюрьме вам послужило на пользу, и вы исправили свое мировозрение. Наш штат любит своих граждан, поэтому хочу предложить вам работу",
                        ped.Name, questPed.Role)
                    .AddAnswer("Конечно, я готов начать жизнь с чистого листа!", descPage)
                    .AddCloseAnswer("Спасибо, мне это неинтересно");
                workDescriptionPage.OpenForPlayer(player);
            };
            
            var medicPed = new QuestPed(PedHash.Doctor01SMM, new Vector3(304.5436, -588.2626, 43.25), "Jock Cranley", "Главный врач", interactionRange: 2, heading: 68);
            medicPed.PlayerInteracted += (player, ped) =>
            {
                var descPage = new DialogPage($"Я осмотрю тебя, найду причину твоей болезни и устраню ее. Это будет стоить {Ems.HealByBotPrice}$", ped.Name, ped.Role)
                    .AddAnswer("Осмотрите меня, я себя плохо чувствую", Ems.HealPlayerByPed)
                    .AddCloseAnswer("Понятно. Спасибо, я пока здоров");
                var introPage =
                    new DialogPage("Приветствую! Я главный врач в этом штате. Если у тебя есть проблемы со здоровьем, то смело обращайся ко мне! Что-то беспокоит в данный момент", ped.Name, ped.Role)
                        .AddAnswer("Доктор, я болен! Мне очень нужна ваша помощь", Ems.HealPlayerByPed)
                        .AddAnswer("А как вы собираетесь меня лечить?", descPage)
                        .AddCloseAnswer("Спасибо, я пока здоров");

                introPage.OpenForPlayer(player);
            };

            var vetPed = new QuestPed(PedHash.Paramedic01SMM, new Vector3(307.1508, -590.4382, 43.129753), "Deve Luxe", "Ветеринар", interactionRange: 2, heading: 140);
            vetPed.PlayerInteracted += (player, ped) =>
            {
                var introPage =
                    new DialogPage("Приветствую! Я заведующий ветеринарной части больницы. Готов помочь тебе с твоим питомцем, у тебя есть вопросы?", ped.Name, ped.Role);

                Pets.Models.PetData petData = Pets.Controller.GetPet(player);
                if (petData != null)
                {
                    introPage.AddAnswer($"Доктор, моему питомцу срочно нужно лечение ({Ems.HealPetByBotPrice}$)", Pets.Controller.Pet_Revive);
                    string priceText = petData.FreeRename ? "Бесплатно" : $"{Ems.RenamePetByBotPrice}$";
                    introPage.AddAnswer($"Вы не могли бы помочь мне с выбором имени питомца? ({priceText})", Pets.Controller.Pet_Rename);
                    introPage.AddAnswer($"Я бы хотел купить игрушку для моего питомца ({Ems.ToyPetByBotPrice}$)", Pets.Controller.Pet_BuyToy);
                }
                introPage.AddCloseAnswer("Спасибо, я пока не нуждаюсь в услугах");

                introPage.OpenForPlayer(player);
            };
        }
        
        [ServerEvent(Event.PlayerConnected)]
        public static void OnPlayerConnected(ExtPlayer player)
        {
            try
            {
                SafeTrigger.ClientEvent(player,"questPeds:load", JsonConvert.SerializeObject(QuestPeds));
            }
            catch (Exception e) { _logger.WriteError("QuestPeds: " + e.ToString()); }
        }
        
        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnected(ExtPlayer player, DisconnectionType type, string reason)
        {
            try
            {
                if (DialogPage.OpenedPages.ContainsKey(player)) DialogPage.OpenedPages.Remove(player);
            }
            catch (Exception e) { _logger.WriteError("QuestPeds: " + e.ToString()); }
        }
        
        [RemoteEvent("dialogWindow:playerSelectedAnswer")]
        public static void OnPlayerSelectedAnswer(ExtPlayer player, int answerId)
        {
            try
            {
                if (DialogPage.OpenedPages.ContainsKey(player)) 
                    DialogPage.OpenedPages[player].OnPlayerSelectedAnswer(player, answerId);                
            }
            catch (Exception e) { _logger.WriteError("QuestPeds: " + e.ToString()); }
        }

        [RemoteEvent("dialogWindow:playerClosedDialog")]
        public static void OnPlayerClosedDialog(ExtPlayer player)
        {
            try
            {
                if (DialogPage.OpenedPages.ContainsKey(player))
                    DialogPage.OpenedPages[player].OnPlayerClosedDialog(player);
            }
            catch (Exception e) { _logger.WriteError("QuestPeds: " + e.ToString()); }
        }
    }
}