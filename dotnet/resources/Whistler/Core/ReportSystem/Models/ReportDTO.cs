using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Whistler.Core.ReportSystem.Models
{
    class ReportDTO
    {
        public int ID { get; set; }
        public string Author { get; set; }
        public int AuthorApiID { get; set; }
        public string AdminName { get; set; }
        public int StateAuthor { get; set; }
        public double OpenedDate { get; set; }
        public bool Closed { get; set; }
        public List<ReportAnswerDTO> Answers { get; set; }
        public ReportDTO(Report report)
        {
            ID = report.ID;
            Author = report.Author.Replace('_', ' ');
            AuthorApiID = report.AuthorUUID;
            AdminName = report.AdminName?.Replace('_', ' ');
            StateAuthor = (int)report.StateAuthor;
            OpenedDate = (report.OpenedDate - ReportConfigs.StartDate).TotalSeconds;
            Closed = report.Closed;
            Answers = report.Answers.Select(item => item.GetReportAnswerDTO()).ToList();
        }
    }
}
