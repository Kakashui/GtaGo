using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GTANetworkAPI;
using Whistler.Core.nAccount;
using Whistler.Customization;
using Whistler.DoorsSystem;
using Whistler.Entities;
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
        public static void OnPlayerConnected(Player player)
        {
            Ban ban = Ban.Get1(player);
            //if (BlackList.Exists(player)) return;
            if (ban != null)
            {
                if (ban.isHard && ban.CheckDate())
                {
                    player?.TriggerEvent("kick", "Main_163".Translate(ban.Until.ToString(), ban.Reason, ban.ByAdmin));
                    return;
                }
            }
        }

        [RemoteEvent("Auth:PlayerReady")]
        public void OnPlayerReady(PlayerGo player, string login, string password)
        {
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
        public static void OnPlayerDisconnected(Player player, DisconnectionType type, string reason)
        {
            try
            {
                if (_passwordRecoveryRequests.ContainsKey(player)) _passwordRecoveryRequests.Remove(player);
            }
            catch(Exception e) { _logger.WriteError($"OnPlayerDisconnecte:\n{e}");}
        }

        private void HandleIfAccountExist(PlayerGo player, string login, string password)
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
                    player.TriggerEvent("auth:startAuth", l);
                }
            }
            else
            {
                var response = MySQL.QueryRead("SELECT `login`, `password` FROM `accounts` WHERE `socialclubid` > 0 and `socialclubid` = @prop0 or `socialclubid` = 0 and `socialclub` = @prop1", player.SocialClubId, player.SocialClubName);
                var l = response.Rows.Count > 0 ? response.Rows[0]["login"].ToString() : "not found";
                player.TriggerEvent("auth:startAuth", l);
            }
        }
        
        private static void HandleIfAccountNotExist(PlayerGo player)
        {
            player.TriggerEvent("auth:startReg");
        }

        [RemoteEvent("auth:char:delete")]
        public void ClientEvent_deleteCharacter(Player player, int index)
        {
            try
            {
                player.GetAccount()?.DeleteCharacter(player, index);
            }
            catch (Exception e) { _logger.WriteError($"ClientEvent_deleteCharacter: {e}"); }
        }

        //[Command("testreg")]
        //public void TestRegistr(Player player)
        //{
        //    try
        //    {
        //        int i = 0;
        //        var myPlayerGo = player.GetPlayerGo();
        //        Timers.Start(1000, () => {
        //            RegisterEvent result = Register(player, $"SLADEWILSON{i}", $"SLADEWILSON{i}", $"SLADEWILSON{i}", $"SLADEWILSON{i}@mail.com", out PlayerGo playerGo);
        //            var character = new Character.Character($"SLADEWILSON{i}", $"SLADEWILSON{i}", playerGo.Account.Id, myPlayerGo.Character.Customization.Id, new Customization.Models.ClothesDTO());
        //            playerGo.CreateCharacter(player, character);
        //            Main.InvokePlayerReady(player);
        //            i++;
        //            _logger.WriteInfo($"TestRegistr: {result.ToString()}");
        //        });
        //    }
        //    catch (Exception e) { _logger.WriteError($"ClientEvent_deleteCharacter: {e}"); }
        //}

        [RemoteEvent("signup")]
        public void ClientEvent_signup(PlayerGo player, string email, string login, string pass)
        {
            try
            {
                login = login.ToLower();               
                RegisterEvent result = Register(player, player.SocialClubId, login, pass, email);
                switch (result)
                {
                    case RegisterEvent.Registered:
                        player.Account.LoadSlots(player);
                        NAPI.Util.ConsoleOutput($"{player.Name} loggiden");
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
                        Notify.SendAuthNotify(player, 1, "Ошибка", "Ошибка создания персонажа!");
                        //Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Money_1", 3000);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e) { _logger.WriteError("signup: " + e.ToString()); }
        }

        [RemoteEvent("signin")]
        public void ClientEvent_signin(PlayerGo player, string login, string pass)
        {
            SignIn(player, login, pass);
        }
        
        private static Dictionary<Player, DateTime> _passwordRecoveryRequests = new Dictionary<Player,DateTime>();

        [RemoteEvent("auth:passRecovered")]
        public void OnPasswordRecovered(Player player, string email)
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
                System.Threading.Tasks.Task.Run((Action)(() =>
                {
                    string msg;
                    int status = 1;
                    if (EmailService.SendNewPasswordTo(email, randomGeneratedPassword))
                    {
                        msg = "Письмо с новым паролем было отправлено на ваш почтовый ящик.";
                        status = 2;
                        MySQL.Query("UPDATE `accounts` SET password = @prop0 WHERE email = @prop1", Account.GetSha256(randomGeneratedPassword), email);
                    }                        
                    else
                        msg = "Произошла ошибка при отправлении письма с паролем. Попробуйте повторить операцию позже.";

                    WhistlerTask.Run(()=> {
                        Notify.SendAuthNotify(player, status, status == 1 ? "Ошибка" : "Успех", msg);
                        //Notify.SendSuccess(player, msg);
                    });
                }));
                Console.WriteLine("All ok");
            }
            catch (Exception e) { _logger.WriteError("passRecovery: " + e.ToString()); }
        }

        public void SignIn(PlayerGo player, string login, string pass)
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
        
        [RemoteEvent("auth:characters:request")]
        public void OnPlayerRequestedLoadCharacter(PlayerGo player)
        {
            try
            {
                var user = player.GetPlayerGo()?.Account;
                if (user.Characters[user.LastCharacter] > 0)
                    LoadCharacterIfExist(player, user);
                else
                   player.TriggerEvent("auth:startCreateCharacter");
            }
            catch (Exception e) { _logger.WriteError("auth:character:load: " + e.ToString()); }
        }

        [RemoteEvent("auth:char:select")]
        public void OnPlayerRequestedLoadCharacter(PlayerGo player, int index)
        {
            try
            {
                var user = player.GetPlayerGo()?.Account;
                if (user == null)
                    return;
                if (!user.SelectCharacter(player, index)) return;
                if (user.Characters[user.LastCharacter] > 0)
                    LoadCharacterIfExist(player, user);
                else
                    CustomizationService.SendToCreator(player, user.LastCharacter);
            }
            catch (Exception e) { _logger.WriteError("auth:char:select: " + e.ToString()); }
        }

        [RemoteEvent("auth:char:spawn")]
        public void SpawnPlayerOnPoint(PlayerGo player, int index)
        {
            try
            {
                if (!player.IsLogged()) return;
                var character = player.GetCharacter();
                player.Spawn(index);
            }
            catch (Exception e) { _logger.WriteError("auth:char:spawn: " + e.ToString()); }
        }

        private void LoadCharacterIfExist(PlayerGo player, Account user)
        {
            try
            {
                var ban = Ban.Get2(user.Characters[user.LastCharacter]);
                var banned = ban != null && ban.CheckDate();
                if (banned)
                {
                    Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Main_162", 4000);
                    return;
                }
                if (player.LoadCharacter(user.Characters[user.LastCharacter]))
                {
                    player.Character.LoadSpawnPoints(player);
                    GameLog.Connected(player.Name, player.Character.UUID, player.SocialClubId, player.GetData<string>("RealHWID"), player.Value, player.Address);
                    NAPI.Util.ConsoleOutput($"{player.Name} loggiden");
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError($"LoadCharacterIfExist:\n{ex}");
            }
        }
        public LoginEvent LoginIn(PlayerGo player, string login_, string pass_)
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
        public RegisterEvent Register(PlayerGo client, ulong socialClubId, string login_, string pass_, string email_)
        {
            try
            {
                if (Main.SocialClubsID.Contains(socialClubId)) return RegisterEvent.SocialReg;
                if (login_.Contains(" ") || login_.Length < 1 || pass_.Length < 1 || !email_.Contains("@") || email_.Length < 1) return RegisterEvent.DataError;
                if (Main.Usernames.Contains(login_)) return RegisterEvent.UserReg;

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