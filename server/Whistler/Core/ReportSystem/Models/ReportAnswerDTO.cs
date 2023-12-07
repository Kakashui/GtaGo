using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Core.ReportSystem.Models
{
    class ReportAnswerDTO
    {
        public int id { get; set; }
        public string sender { get; set; }
        public string text { get; set; }
        public int reportId { get; set; }
        public ReportAnswerDTO(ReportAnswer answer)
        {
            id = answer.ID;
            sender = answer.Sender.Replace('_', ' ');
            text = answer.Message;
            reportId = answer.ReportId;
        }
    }
}
