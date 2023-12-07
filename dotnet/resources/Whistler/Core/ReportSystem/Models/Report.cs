using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Whistler.Core.Character;
using Whistler.SDK;

namespace Whistler.Core.ReportSystem.Models
{
    class Report
    {
        /// <summary>
        /// report id
        /// </summary>
        public int ID { get; }
        /// <summary>
        /// uuid автора репорта
        /// </summary>
        public int AuthorUUID { get; }
        /// <summary>
        /// имя автора репорта
        /// </summary>
        public string Author { get; }
        /// <summary>
        /// гташный id автора репорта
        /// </summary>
        public int AuthorApiID { get; }
        /// <summary>
        /// Имя админа, который закрыл репорт
        /// </summary>
        public string AdminName { get; set; }
        public int AdminUUID { get; set; }
        public ReporterStatus StateAuthor { get; set; }
        public DateTime OpenedDate { get; set; }
        public DateTime ClosedDate { get; set; }
        public bool Closed { get; set; }
        public int Rating { get; set; }
        public int StateGet = 0;

        public List<ReportAnswer> Answers { get; set; }

        public Report(Character.Character character, int playerID, nAccount.Account account)
        {
            AuthorUUID = character.UUID;
            Author = character.FullName;
            AuthorApiID = playerID;
            OpenedDate = DateTime.Now;
            ClosedDate = DateTime.MinValue;
            if (character.Media > 0 || character.MediaHelper > 0)
                StateAuthor = ReporterStatus.Media;
            else if (character.IsPrimeActive())
                StateAuthor = ReporterStatus.VIP;
            else if (character.AmountOfRatings / (character.NumberOfRatings * 1.0F) >= ReportConfigs.MinRatingForBest && character.NumberOfRatings >= ReportConfigs.MinNumberRatingForBest)
                StateAuthor = ReporterStatus.GoodReputation;
            else
                StateAuthor = ReporterStatus.Normal;
            AdminName = null;
            AdminUUID = -1;
            Rating = -1;
            Answers = new List<ReportAnswer>();
            DataTable query =  MySQL.QueryRead("INSERT INTO `reports` (`authoruuid`, `opendate`) VALUES (@prop0, @prop1); SELECT @@identity;", AuthorUUID, MySQL.ConvertTime(OpenedDate));
            ID = Convert.ToInt32(query.Rows[0][0]);
        }

        public ReportAnswer AddMessage(Character.Character character, string message, bool saveToDB = true)
        {
            ReportAnswer answer = new ReportAnswer(ID, character, message, saveToDB);
            Answers.Add(answer);
            return answer;
        }

        public void Send(string eventName, bool sendAccess, params object[] args)
        {
            if (!sendAccess)
                SafeTrigger.ClientEventToPlayers(ReportManager.Admins.ToArray(), eventName, args);
            else
            {
                List<object> param = new List<object>() { false };
                param.AddRange(args);
                object[] listParam = param.ToArray();
                foreach (var admin in ReportManager.Admins)
                {
                    bool viewAllReports = Group.CanUseAdminCommand(admin, "viewallreports", false);
                    listParam[0] = viewAllReports;
                    SafeTrigger.ClientEvent(admin, eventName, listParam);
                }
            }
        }

        public ReportDTO GetReportDTO()
        {
            return new ReportDTO(this);
        }

        public string GetSerializeReportDTO()
        {
            return JsonConvert.SerializeObject(new ReportDTO(this));
        }
    }
}
