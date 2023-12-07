using System;
using System.Collections.Generic;
using System.Text;
using Whistler.SDK;
using Newtonsoft.Json;

namespace Whistler.NewJobs.Models
{
    public class JobWorker
    {
        private bool _changed = false;
        public int UUID { get; set; }
        public Dictionary<string, int> Expiriance { get; set; }
        public Job CurrentJob { get; set; }
        public int TotalInPayday { get; set; } = 0;

        public int GetExp(Job job)
        {
            if (!Expiriance.ContainsKey(job.Name)) 
                return 0;

            return Expiriance[job.Name];
        }
        public int AddExp(Job job, int exp = 1)
        {
            if (!Expiriance.ContainsKey(job.Name))
            {
                Expiriance.Add(job.Name, exp);
            }
            else
                Expiriance[job.Name] += exp;
            _changed = true;
            return Expiriance[job.Name];
        }

        public void ResetLimit()
        {
            TotalInPayday = 0;
        }

        public void Update()
        {
            if (!_changed) return;
            MySQL.Query("UPDATE `workers` SET `exp`=@prop0 WHERE `uuid`=@prop1", JsonConvert.SerializeObject(Expiriance), UUID);
            _changed = false;
        }
    }
}
