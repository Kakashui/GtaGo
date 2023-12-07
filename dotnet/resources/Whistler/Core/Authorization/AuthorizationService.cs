using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core.nAccount;
using Whistler.Customization;
using Whistler.DoorsSystem;
using Whistler.Entities;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.SDK;
using Whistler.Services;

namespace Whistler.Core.Authorization
{
    public class AuthorizationService : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(AuthorizationService));
        private const int _emailSendingDelayInMinutes = 1;
        
        [ServerEvent(Event.PlayerConnected)]
        public static void OnPlayerConnected(ExtPlayer player)
        {
            Ban ban = Ban.Get1(player);
            //if (BlackList.Exists(player)) return;
            if (ban != null)
            {
                if (ban.isHard && ban.CheckDate())
                {
                    SafeTrigger.ClientEvent(player, "kick", $"Вы забанены до {ban.Until.ToString()}. Причина: {ban.Reason} ({ban.ByAdmin})");
                    return;
                }
            }
        }

        [RemoteEvent("Auth:PlayerReady")]
        public void OnPlayerReady(ExtPlayer player, string login, string password)
        {
            //Console.WriteLine("PlayerReady");//пойду похаваю
            try
            {                
                if (Main.SocialClubsID.Contains(player.SocialClubId) || Main.SocialClubs.Contains(player.SocialClubName))
                    HandleIfAccountExist(player, login, password);
                else 
                    HandleIfAccountNotExist(player);                
            }
            catch(Exception ex) {Console.WriteLine("error auth " + ex);}
        }
        
        [ServerEvent(Event.PlayerDisconnected)]
        public static void OnPlayerDisconnected(ExtPlayer player, DisconnectionType type, string reason)
        {
            try
            {
                if (_passwordRecoveryRequests.ContainsKey(player)) _passwordRecoveryRequests.Remove(player);
            }
            catch(Exception e) { _logger.WriteError($"OnPlayerDisconnecte:\n{e}");}
        }

        private void HandleIfAccountExist(ExtPlayer player, string login, string password)
        {            
            if (login != "" && password != "")
            {
                LoginEvent result = LoginIn(player, login, password);
                if (result == LoginEvent.Authorized)
                    player.Account.LoadSlots(player);
                else if (result == LoginEvent.Already) return;
                else
                {
                    var response = MySQL.QueryRead("SELECT `login`, `password` FROM `accounts` WHERE `socialclubid` > 0 and `socialclubid` = @prop0 or `socialclubid` = 0 and `socialclub` = @prop1", player.SocialClubId, player.SocialClubName);
                    var l = response.Rows.Count > 0 ? response.Rows[0]["login"].ToString() : "not found";
                    SafeTrigger.ClientEvent(player,"auth:startAuth", l);
                }
            }
            else
            {
                var response = MySQL.QueryRead("SELECT `login`, `password` FROM `accounts` WHERE `socialclubid` > 0 and `socialclubid` = @prop0 or `socialclubid` = 0 and `socialclub` = @prop1", player.SocialClubId, player.SocialClubName);
                var l = response.Rows.Count > 0 ? response.Rows[0]["login"].ToString() : "not found";
                SafeTrigger.ClientEvent(player,"auth:startAuth", l);
            }
        }
        
        private static void HandleIfAccountNotExist(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player,"auth:startReg");
        }

        [RemoteEvent("auth:char:delete")]
        public void ClientEvent_deleteCharacter(ExtPlayer player, int index)
        {
            try
            {
                DialogUI.Open(player, $"Вы действительно хотите удалить персонажа безвозвратно?", new List<DialogUI.ButtonSetting>
                {
                    new DialogUI.ButtonSetting
                    {
                        Name = "Подтвердить",
                        Icon = null,
                        Action = (p) =>
                        {
                            p.Account?.DeleteCharacter(player, index);
                        }
                    },
                    new DialogUI.ButtonSetting
                    {
                        Name = "Отменить",
                        Icon = null,
                        Action = { }
                    }
                });
            }
            catch (Exception e) { _logger.WriteError($"ClientEvent_deleteCharacter: {e}"); }
        }

        //[Command("testreg")]
        //public void TestRegistr(ExtPlayer player)
        //{
        //    try
        //    {
        //        int i = 0;
        //        var myExtPlayer = player;
        //        Timers.Start(1000, () => {
        //            RegisterEvent result = Register(player, $"SLADEWILSON{i}", $"SLADEWILSON{i}", $"SLADEWILSON{i}", $"SLADEWILSON{i}@mail.com", out ExtPlayer ExtPlayer);
        //            var character = new Character.Character($"SLADEWILSON{i}", $"SLADEWILSON{i}", ExtPlayer.Account.Id, myExtPlayer.Character.Customization.Id, new Customization.Models.ClothesDTO());
        //            ExtPlayer.CreateCharacter(player, character);
        //            Main.InvokePlayerReady(player);
        //            i++;
        //            _logger.WriteInfo($"TestRegistr: {result.ToString()}");
        //        });
        //    }
        //    catch (Exception e) { _logger.WriteError($"ClientEvent_deleteCharacter: {e}"); }
        //}

        [RemoteEvent("signup")]//перебилди клиентку
        public void ClientEvent_signup(ExtPlayer player, params object[] args )
        {
            Console.WriteLine($"signup: {JsonConvert.SerializeObject(args)}");
            string email = (string)args[0]; 
            string login = (string)args[1]; 
            string pass = (string)args[2]; 
            //string promocode = (string)args[3];
            try
            {
                login = login.ToLower();               
                RegisterEvent result = Register(player, player.SocialClubId, login, pass, email, null);
                switch (result)
                {
                    case RegisterEvent.Registered:
                        player.Account.LoadSlots(player);
                        break;
                    case RegisterEvent.SocialReg:
                        Notify.SendAuthNotify(player, 1, "Ошибка", "На этот SocialClub уже зарегистрирован игровой аккаунт!");
                        //Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Main_167", 3000);
                        break;
                    case RegisterEvent.UserReg:
                        Notify.SendAuthNotify(player, 1, "Ошибка", "Данное имя пользователя уже занято!");
                        //Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Main_168", 3000);
                        break;
                    case RegisterEvent.EmailReg:
                        Notify.SendAuthNotify(player, 1, "Ошибка", "Данный email уже занят!");
                        //Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Main_169", 3000);
                        break;
                    case RegisterEvent.DataError:
                        Notify.SendAuthNotify(player, 1, "Ошибка", "Введите корректные данные");
                        //Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Main_170", 3000);
                        break;
                    case RegisterEvent.Error:
                        Notify.SendAuthNotify(player, 1, "Ошибка", "Неизвестная ошибка, обратитесь на форум");
                        //Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Money_1", 3000);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e) { _logger.WriteError("signup: " + e.ToString()); }
        }

        [RemoteEvent("signin")]
        public void ClientEvent_signin(ExtPlayer player, string login, string pass)
        {
            SignIn(player, login, pass);
        }

        private static Dictionary<ExtPlayer, DateTime> _passwordRecoveryRequests = new Dictionary<ExtPlayer,DateTime>();

        [RemoteEvent("auth:passRecovered")]
        public void OnPasswordRecovered(ExtPlayer player, string email)
        {
            try
            {
                var response = MySQL.QueryRead("SELECT * FROM accounts WHERE email = @prop0 AND (`socialclubid` > 0 and `socialclubid` = @prop1 or `socialclubid` = 0 and `socialclub` = @prop2)", email, player.SocialClubId, player.SocialClubName);
                if (response == null || response.Rows.Count == 0)
                {
                    //Notify.SendError(player, "auth:noaccaunt");
                    Notify.SendAuthNotify(player, 1, "Ошибка", "Аккаунт не найден");
                    return;
                }

                if (_passwordRecoveryRequests.TryGetValue(player, out var dataTime))
                {
                    if (DateTime.Now.Subtract(dataTime).TotalMinutes < _emailSendingDelayInMinutes)
                    {
                        //Notify.SendError(player, "auth:email:already");
                        Notify.SendAuthNotify(player, 1, "Ошибка", "Письмо уже было отправлено, попробуйте еще раз через минуту");
                        return;
                    }
                    _passwordRecoveryRequests[player] = DateTime.Now;
                }
                else
                {
                    _passwordRecoveryRequests.Add(player, DateTime.Now);
                }
                
                var randomGeneratedPassword = AuthUtils.GenerateRandomPassword(8, 0);
                System.Threading.Tasks.Task.Run((async() =>
                {
                    string msg;
                    int status = 1;
                    if (await EmailService.SendNewPasswordTo(email, randomGeneratedPassword))
                    {
                        msg = "Новый пароль был выслан на Вашу электронную почту";
                        status = 2;
                        MySQL.Query("UPDATE `accounts` SET password = @prop0 WHERE email = @prop1", Account.GetSha256(randomGeneratedPassword), email);
                    }                        
                    else msg = "Проверьте правильность введенных данных.";

                    NAPI.Task.Run(()=> {
                        Notify.SendAuthNotify(player, status, status == 1 ? "Ошибка" : "Успешно", msg);
                        //Notify.SendSuccess(player, msg);
                    });
                }));
            }
            catch (Exception e) { _logger.WriteError("passRecovery: " + e.ToString()); }
        }

        public void SignIn(ExtPlayer player, string login, string pass)
        {
            try
            {
                if (Main.Emails.ContainsKey(login))
                    login = Main.Emails[login];
                else
                    login = login.ToLower();
               
                LoginEvent result = LoginIn(player, login, pass);
                if (result == LoginEvent.Authorized)
                {
                    player.Account.LoadSlots(player);
                }
                else if (result == LoginEvent.Already) return;
                else if (result == LoginEvent.Refused)
                    Notify.SendAuthNotify(player, 1, "Ошибка", "Неправильный пароль от аккаунта");
                    //Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_75", 3000);
                if (result == LoginEvent.SclubError)
                    Notify.SendAuthNotify(player, 1, "Ошибка", "SocialClub, с которого Вы подключены, не совпадает с тем, который привязан к аккаунту");
                    //Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Main_165", 3000);
            }
            catch (Exception e) { _logger.WriteError("signin: " + e.ToString()); }
        }
        
        // не юзается
        // [RemoteEvent("auth:characters:request")]
        // public void OnPlayerRequestedLoadCharacter(ExtPlayer player)
        // {
        //     try
        //     {
        //         var user = player.Account;
        //         if (user.Characters[user.LastCharacter] > 0)
        //             LoadCharacterIfExist(player, user);
        //         else
        //            SafeTrigger.ClientEvent(player,"auth:startCreateCharacter");
        //     }
        //     catch (Exception e) { _logger.WriteError("auth:character:load: " + e.ToString()); }
        // }

        // Нажал выбрать персонажа
        [RemoteEvent("auth:char:select")]
        public void OnPlayerRequestedLoadCharacter(ExtPlayer player, int index)
        {
            try
            {
                Account user = player.Account;
                if (user == null) return;
                if (!user.SelectCharacter(player, index)) return;
                if (user.Characters[user.LastCharacter] > 0)
                    LoadCharacterIfExist(player, user, index);
                else
                    CustomizationService.SendToCreator(player, user.LastCharacter);
            }
            catch (Exception e) { _logger.WriteError("auth:char:select: " + e.ToString()); }
        }

        [RemoteEvent("auth:char:create")]
        public void OnPlayerRequestedCreateCharacter(ExtPlayer player, int index)
        {
            try
            {
                var user = player.Account;
                if (user == null)
                    return;
                if (!user.SelectCharacter(player, index)) return;
                if(index == -1){
                    CustomizationService.SendToCreator(player, -1);
                }
            }
            catch (Exception e) { _logger.WriteError("auth:char:create: " + e.ToString()); }
        }

        [RemoteEvent("auth:char:spawn")]
        public void SpawnPlayerOnPoint(ExtPlayer player, int index)
        {
            try
            {
                if (!player.IsLogged()) return;

                player.Spawn(index);
            }
            catch (Exception e) { _logger.WriteError("auth:char:spawn: " + e.ToString()); }
        }

        private void LoadCharacterIfExist(ExtPlayer player, Account user, int index)
        {
            try
            {
                var ban = Ban.Get2(user.Characters[user.LastCharacter]);
                var banned = ban != null && ban.CheckDate();
                if (banned)
                {
                    Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Аккаунт заблокирован", 4000);
                    return;
                }
                if (player.LoadCharacterData(user.Characters[user.LastCharacter]))
                {
                    player.Character.LoadSpawnPoints(player, index);
                    GameLog.Connected(player.Name, player.Character.UUID, player.SocialClubId, player.Session.HWID, player.Value, player.Address);
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError($"LoadCharacterIfExist:\n{ex}");
            }
        }
        public LoginEvent LoginIn(ExtPlayer player, string login_, string pass_)
        {
            try
            {
                login_ = login_.ToLower();
                pass_ = Account.GetSha256(pass_);
                DataTable result = MySQL.QueryRead("SELECT * FROM `accounts` WHERE `login` = @prop0 AND password = @prop1", login_, pass_);
                if (result == null || result.Rows.Count == 0) return LoginEvent.Refused;

                DataRow row = result.Rows[0];
                return player.LoadAccount(row);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"LoginIn {ex}");
                return LoginEvent.Error;
            }
        }
        public RegisterEvent Register(ExtPlayer client, ulong socialClubId, string login_, string pass_, string email_, string promocode_)
        {
            try
            {
                if (Main.SocialClubsID.Contains(socialClubId)) 
                {
                    _logger.WriteWarning($"{login_} tried to register with {socialClubId}, but account with that social club already exists.");
                    return RegisterEvent.SocialReg;
                }
                if (login_.Contains(" ") || login_.Length < 1 || pass_.Length < 1 || !email_.Contains("@") || email_.Length < 1) return RegisterEvent.DataError;
                if (Main.Usernames.Contains(login_)) return RegisterEvent.UserReg;

                email_ = email_.ToLower();
                if (Main.Emails.ContainsKey(email_)) return RegisterEvent.EmailReg;

                lock (Main.Emails)
                {
                    if (Main.Emails.ContainsKey(email_))
                        Main.Emails[email_] = login_;
                    else
                        Main.Emails.Add(email_, login_);
                }
                Main.SocialClubsID.Add(socialClubId);
                Main.Usernames.Add(login_);

                Account account = new Account(email_, login_, pass_, socialClubId, client.Serial, client.Address);
                client.LoadAccount(account);
                PromoCodesService.RegisterUserPromo(client, promocode_);
                client.TriggerCefEvent("optionsMenu/setCanUsePromo", string.IsNullOrEmpty(promocode_));
                return RegisterEvent.Registered;
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Register{ex}");
                return RegisterEvent.Error;
            }
        }
    }
}