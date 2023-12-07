using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Fractions.GOV.Models
{
    class Complaint
    {
        public int ApplicantUUID { get; set; }
        public int FractionId { get; set; }
        public int EmployeeUUID { get; set; }
        public string Text { get; set; }
        public Complaint(int applicantUUID, int fractionId, int employeeUUID, string text)
        {
            ApplicantUUID = applicantUUID;
            FractionId = fractionId;
            EmployeeUUID = employeeUUID;
            Text = text;
        }
    }
}
