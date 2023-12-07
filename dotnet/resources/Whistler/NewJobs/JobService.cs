using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Whistler.NewJobs.Models;
using Whistler.SDK;
using Newtonsoft.Json;
using GTANetworkAPI;
using Whistler.Helpers;
using Whistler.NewJobs.Enums;
using System.Collections.Concurrent;
using Whistler.Core.Character;
using Whistler.Entities;

namespace Whistler.NewJobs
{
    public static class JobService
    {        
        private static ConcurrentDictionary<int, JobWorker> _workers = new ConcurrentDictionary<int, JobWorker>();
        private static List<Job> _jobs = new List<Job>();

        public static void ResetLimits()
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                foreach (var worker in _workers)
                {
                    worker.Value.ResetLimit();
                }
            });            
        }

        public static JobWorker GetWorker(this ExtPlayer player)
        {
            var character = player.Character;
            return GetWorkerByUUID(character.UUID);
        }

        public static JobWorker GetWorkerByUUID(int uuid)
        {
            return _workers.GetOrAdd(uuid, LoadFromDB(uuid));
        }

        private static JobWorker LoadFromDB(int uuid)
        {
            var resp = MySQL.QueryRead("SELECT `exp` FROM `workers` WHERE `uuid`=@prop0", uuid);
            if (resp == null || resp.Rows.Count == 0) return CreateNewJobWorker(uuid);
            else return LoadJobWorker(uuid, resp.Rows[0]["exp"].ToString());
        }

        private static JobWorker CreateNewJobWorker(int uuid)
        {
            var worker = new JobWorker { UUID = uuid, Expiriance = new Dictionary<string, int>() };
            _workers.TryAdd(worker.UUID, worker);
            MySQL.Query("INSERT INTO `workers`(`exp`, `uuid`) VALUES(@prop0, @prop1)", JsonConvert.SerializeObject(worker.Expiriance), worker.UUID);
            return worker;
        }

        private static JobWorker LoadJobWorker(int uuid, string exp)
        {
            var worker = new JobWorker { UUID = uuid, Expiriance = JsonConvert.DeserializeObject<Dictionary<string, int>>(exp) };
            _workers.TryAdd(worker.UUID, worker);
            return worker;
        }

        public static void UpdateWorkers()
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                foreach (var worker in _workers)
                {
                    worker.Value.Update();
                }
            });
        }

        public static void RegisterNewJob(Job job)
        {
            if (_jobs.Contains(job))
                throw new Exception($"Job with name {job.Name} already exists");
            _jobs.Add(job);
        }
    }
}
