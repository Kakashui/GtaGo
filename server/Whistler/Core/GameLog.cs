using MySql.Data.MySqlClient;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using GTANetworkAPI;
using System.Collections.Concurrent;
using Whistler.Inventory.Enums;
using Whistler.MoneySystem;
using Whistler.Domain.Phone.Bank;
using Whistler.Helpers;

namespace Whistler.Core
{
    public class GameLog
    {

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(GameLog));
        private static ConcurrentQueue<RecordLog> queue = new ConcurrentQueue<RecordLog>();
        private static Dictionary<int, DateTime> OnlineQueue = new Dictionary<int, DateTime>();        

        public static string DB = Main.ServerConfig.MySQL.DataBase + "logs";

        private static string insert = "insert into " + DB + ".{0} ({1}) values ({2})";
        
        public static int GetItemLastId()
        {
            var request = MySQL.QueryRead($"SELECT ifnull(max(id), 0) as cnt from {DB}.itemslog");
            if (request.Rows == null || request.Rows.Count <= 0)
                return 0;
            else
                return Convert.ToInt32(request.Rows[0]["cnt"]);
        }
        public static void Votes(uint electionId, string login, string voteFor)
        {
            queue.Enqueue(new RecordLog(                
                string.Format(insert, "votelog", "`election`,`login`,`votefor`,`time`", $"@prop0, @prop1, @prop2, @prop3"),
                electionId, login, voteFor, MySQL.ConvertTime(DateTime.Now)
                ));
        }
        public static void Box(int boxId, int uuid, string item)
        {
            queue.Enqueue(new RecordLog(
                string.Format(insert, "boxlog", "`time`,`boxId`,`uuid`,`item`", $"@prop0, @prop1, @prop2, @prop3"),
                MySQL.ConvertTime(DateTime.Now), boxId, uuid, item
                ));
        }
        public static void Admin(string admin, string action, string client)
        {
            queue.Enqueue(new RecordLog(
                string.Format(insert, "adminlog", "`time`,`admin`,`action`,`player`", $"@prop0, @prop1, @prop2, @prop3"),
                MySQL.ConvertTime(DateTime.Now), admin, action, client
                ));
        }
        
        public static void Casino(string operation, int money, string playerName, DateTime date)
        {
            queue.Enqueue(new RecordLog(
                string.Format(insert, "casinologs", "`operation`,`money`,`playername`, `datetime`", $"@prop0, @prop1, @prop2, @prop3"),
                operation, money, playerName, MySQL.ConvertTime(DateTime.Now)
                ));
        }
        public static void Chat(int uuid, int typeChat, string message)
        {
            queue.Enqueue(new RecordLog(
                string.Format(insert, "chatlogs", "`uuid`, `typechat`, `message`, `datetime`", $"@prop0, @prop1, @prop2, @prop3"),
                uuid, typeChat, message, MySQL.ConvertTime(DateTime.Now)
                ));
        }        
     
        //public static void Money(string from, string to, long amount, string comment)
        //{
        //    queue.Enqueue(new RecordLog(
        //        string.Format(insert, "moneylog", "`time`,`from`,`to`,`amount`,`comment`", $"@prop0, @prop1, @prop2, @prop3, @prop4"),
        //        MySQL.ConvertTime(DateTime.Now), from, to, amount, comment
        //        ));
        //}
        public static void MoneyNew(TypeMoneyAcc fromType, int from, TypeMoneyAcc toType, int to, long amount, int tax, string comment)
        {
            queue.Enqueue(new RecordLog(
                string.Format(insert, "newmoneylog", "`time`, `fromtype`, `from`, `totype`, `to`, `amount`, `tax`, `comment`", $"@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6, @prop7"),
                MySQL.ConvertTime(DateTime.Now), (int)fromType, from, (int)toType, to, amount, tax, comment
                ));
        }
        //public static void FractionMoney(int id, string operation, int amount, string desc)
        //{
        //    queue.Enqueue(new RecordLog(
        //        string.Format(insert, "fractionmoney", "`fractionId`,`operation`,`sum`,`date`,`description`", $"@prop0, @prop1, @prop2, @prop3, @prop4"),
        //        id, operation, amount, MySQL.ConvertTime(DateTime.Now), desc
        //        ));     
        //}

        public static void Name(int uuid, string old, string newName)
        {
            queue.Enqueue(new RecordLog(
                string.Format(insert, "namelog", "`time`,`uuid`,`old`,`new`", $"@prop0, @prop1, @prop2, @prop3"),
                MySQL.ConvertTime(DateTime.Now), uuid, old, newName
                ));
        }
        
        public static void Kill(string target, string killer, uint clientHash, uint serverHash, int distance)
        {
            queue.Enqueue(new RecordLog(
                string.Format(insert, "killog", "`killer`,`target`,`clientweapon`,`serverweapon`,`distance`,`date`", $"@prop0, @prop1, @prop2, @prop3, @prop4, @prop5"),
                killer, target, clientHash, serverHash, distance, MySQL.ConvertTime(DateTime.Now)
                ));
        }
        public static void Ban(int admin, int client, DateTime until, string reason, bool isHard)
        {
            queue.Enqueue(new RecordLog(
                string.Format(insert, "banlog", "`time`,`admin`,`player`,`until`,`reason`,`ishard`", $"@prop0, @prop1, @prop2, @prop3, @prop4, @prop5"),
                MySQL.ConvertTime(DateTime.Now), admin, client, MySQL.ConvertTime(until), reason, isHard
                ));
        }
        public static void Ticket(int player, int target, int sum, string reason, string pnick, string tnick)
        {
            queue.Enqueue(new RecordLog(
                string.Format(insert, "ticketlog", "`time`,`player`,`target`,`sum`,`reason`,`pnick`,`tnick`", $"@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6"),
                MySQL.ConvertTime(DateTime.Now), player, target, sum, reason, pnick, tnick
                ));
        }
        public static void Arrest(int player, int target, string reason, int stars, string pnick, string tnick)
        {
            queue.Enqueue(new RecordLog(
                string.Format(insert, "arrestlog", "`time`,`player`,`target`,`reason`,`stars`,`pnick`,`tnick`", $"@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6"),
                MySQL.ConvertTime(DateTime.Now), player, target, reason, stars, pnick, tnick
                ));
        }
        public static void Connected(string name, int uuid, ulong sClub, string hwid, int id, string ip)
        {
            if (OnlineQueue.ContainsKey(uuid)) return;
            DateTime now = DateTime.Now;
            queue.Enqueue(new RecordLog(
                string.Format(insert, "connlog", "`in`,`out`,`uuid`,`sclub`,`hwid`,`ip`", $"@prop0, @prop1, @prop2, @prop3, @prop4, @prop5"),
                MySQL.ConvertTime(now), null, uuid, sClub, hwid, ip
                ));
            queue.Enqueue(new RecordLog(
                string.Format(insert, "idlog", "`in`,`out`,`uuid`,`id`,`name`", $"@prop0, @prop1, @prop2, @prop3, @prop4"),
                MySQL.ConvertTime(now), null, uuid, id, name
                ));
            OnlineQueue.Add(uuid, now);
        }
        public static void Disconnected(int uuid)
        {
            if (!OnlineQueue.ContainsKey(uuid)) return;
            DateTime conn = OnlineQueue[uuid];
            if (conn == null) return;
            OnlineQueue.Remove(uuid);
            queue.Enqueue(new RecordLog(
                $"update {DB}.connlog set `out`=@prop0 WHERE `in`=@prop1 and `uuid`=@prop2",
                MySQL.ConvertTime(DateTime.Now), MySQL.ConvertTime(conn), uuid
                ));
            queue.Enqueue(new RecordLog(
                $"update {DB}.idlog set `out`=@prop0 WHERE `in`=@prop1 and `uuid`=@prop2",
                MySQL.ConvertTime(DateTime.Now), MySQL.ConvertTime(conn), uuid
                ));
        }
        public static void CharacterDelete(string name, int uuid, string account)
        {
            queue.Enqueue(new RecordLog(
                string.Format(insert, "deletelog", "`time`,`uuid`,`name`,`account`", $"@prop0, @prop1, @prop2, @prop3"),
                MySQL.ConvertTime(DateTime.Now), uuid, name, account
                ));
        }
        //public static void EventLogAdd(string AdmName, string EventName, ushort MembersLimit, string Started)
        //{
        //    queue.Enqueue(string.Format(
        //        insert, "eventslog", "`AdminStarted`,`EventName`,`MembersLimit`,`Started`", $"'{AdmName}','{EventName}','{MembersLimit}','{Started}'"));
        //}
        //public static void EventLogUpdate(string AdmName, int MembCount, string WinName, uint Reward, string Time, uint RewardLimit, ushort MemLimit, string EvName)
        //{
        //    queue.Enqueue($"update {DB}.eventslog set `AdminClosed`='{AdmName}',`Members`={MembCount},`Winner`='{WinName},`Reward`={Reward},`Ended`='{Time}',`RewardLimit`={RewardLimit} WHERE `Winner`='Undefined' AND `MembersLimit`={MemLimit} AND `EventName`='{EvName}'");
        //}
        //public static void CasinoPlacedBet(string name, int uuid, ushort red, ushort zero, ushort black)
        //{
        //    queue.Enqueue(string.Format(
        //        insert, "casinobetlog", "`time`,`name`,`uuid`,`red`,`zero`,`black`",
        //        $"'{DateTime.Now.ToString("s")}','{name}',{uuid},'{red}','{zero}','{black}'"));
        //}
        //public static void CasinoEnd(string name, int uuid, byte casino, byte disctype)
        //{
        //    queue.Enqueue(string.Format(
        //        insert, "casinoendlog", "`time`,`name`,`uuid`,`state`,`type`",
        //        $"'{DateTime.Now.ToString("s")}','{name}',{uuid},{casino},{disctype}"));
        //}
        //public static void CasinoWinLose(string name, int uuid, int wonbet)
        //{
        //    queue.Enqueue(string.Format(
        //        insert, "casinowinloselog", "`time`,`name`,`uuid`,`wonbet`",
        //        $"'{DateTime.Now.ToString("s")}','{name}',{uuid},{wonbet}"));
        //}
        public static void DonateRouletteDrop(string name, int uuid, int dropRarity, int dropType, string dropValue, bool isFree)
        {
            queue.Enqueue(new RecordLog(
                string.Format(insert, "donateroulettelog", "`time`,`name`,`uuid`,`droprarity`,`droptype`,`dropvalue`,`isfree`", $"@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6"),
                MySQL.ConvertTime(DateTime.Now), name, uuid, dropRarity, dropType, dropValue, isFree
                ));
        }
        public static void DonateShop(int uuid, int price, string typeproduct, string product)
        {
            queue.Enqueue(new RecordLog(
                string.Format(insert, "donateshoplog", "`time`,`uuid`,`price`,`typeproduct`,`product`", $"@prop0, @prop1, @prop2, @prop3, @prop4"),
                MySQL.ConvertTime(DateTime.Now), uuid, price, typeproduct, product
                ));
        }
        public static void ItemsLog(int itemId, string from, string to, ItemNames type, int amount, string data, LogAction action, int uuid = -1)
        {
            if (from != to && itemId > 0)
                queue.Enqueue(new RecordLog(
                    string.Format(insert, "itemslog", "`time`, `id`, `from`,`to`,`type`,`amount`, `data`, `action`, `player`", $"@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6, @prop7, @prop8"),
                    MySQL.ConvertTime(DateTime.Now), itemId, from, to, (int)type, amount, data, (int)action, uuid
                    ));
        }
        #region logic
        private static Timer _timer;
        public static void Start()
        {
            _timer = new Timer(Worker, null, 0, 250);
            _logger.WriteDebug("Worker started");
        }
        private static void Worker(object obj)
        {
            try
            {
                if (queue.Count > 0 && queue.TryDequeue(out RecordLog log))    
                    log?.MySQLQuery();
            }
            catch (Exception e)
            {
                _logger.WriteError($"{e}\n");
            }
        }       
        #endregion
    }

    class RecordLog
    {
        public string Query { get; }
        public object[] Args { get; }
        public RecordLog(string query, params object[] args)
        {
            Query = query;
            Args = args;
        }

        public void MySQLQuery()
        {
            MySQL.Query(Query, Args);
        }
    }
}
