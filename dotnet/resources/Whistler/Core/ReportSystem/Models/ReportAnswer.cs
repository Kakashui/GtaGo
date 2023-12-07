using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Whistler.SDK;

namespace Whistler.Core.ReportSystem.Models
{
    class ReportAnswer
    {
        public int ID { get; set; }
        public int ReportId { get; set; }

        [JsonIgnore]
        public int SenderUUID { get; set; }
        public string Sender { get; set; }
        public string Message { get; set; }
        public ReportAnswer(int reportId, Character.Character sender, string message, bool saveToDB = true)
        {
            ReportId = reportId;
            SenderUUID = sender.UUID;
            Sender = sender.FullName;
            Message = message;
            if (saveToDB)
            {
                DataTable query = MySQL.QueryRead("INSERT INTO `reportmessages` (`reportid`, `senderuuid`, `message`) VALUES (@prop0, @prop1, @prop2); SELECT @@identity;", ReportId, SenderUUID, Message);
                ID = Convert.ToInt32(query.Rows[0][0]);
            }
            else
                ID = -1;
        }
        public void Send()
        {
            SafeTrigger.ClientEventToPlayers(ReportManager.Admins.ToArray(), "addreport", ID, ReportId, Sender, Message);
        }

        public ReportAnswerDTO GetReportAnswerDTO()
        {
            return new ReportAnswerDTO(this);
        }

        public string GetSerializeReportAnswerDTO()
        {
            return JsonConvert.SerializeObject(new ReportAnswerDTO(this));
        }
    }
}
