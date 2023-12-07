using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.MiniGames.MakeWeapon;
using Whistler.NewJobs.Enums;
using Whistler.NewJobs.Models;
using Whistler.SDK;

namespace Whistler.NewJobs.Models
{
    public class Job
    {       
        public string Name { get; set; }
        public List<JobLevel> Levels { get; set; }
        public Predicate<ExtPlayer> Condition { get; set; }
        public int Limit { get; set; } = 50000;
        private List<JobLevel> _sorted;
        public Job()
        {
            JobService.RegisterNewJob(this);
        }

        public void Invite(ExtPlayer player)
        {
            if(CanGetOnJob(player) == TryGetJobResults.Success)
            {
                var worker = JobService.GetWorker(player);
                worker.CurrentJob = this;
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "mg:mw:job:invite", 3000);
            }
        }

        public void Leave(ExtPlayer player)
        {
            var worker = JobService.GetWorker(player);
            worker.CurrentJob = null; 
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "mg:mw:job:leave", 3000);
        }

        public TryGetJobResults CanGetOnJob(ExtPlayer player)
        {
            var character = player.Character;
            if (character.WorkID > 0) return TryGetJobResults.OnOtherJob;
            if (!Condition(player)) return TryGetJobResults.BadCondition;
            var worker = JobService.GetWorkerByUUID(character.UUID);
            if (worker.CurrentJob == null)
            {
                //if (IsLimit(player, worker))
                //    return TryGetJobResults.Limit;
                return TryGetJobResults.Success;
            }
            else
            {
                if (worker.CurrentJob == this) return TryGetJobResults.Already;
                else return TryGetJobResults.OnOtherJob;
            }
        }

        public bool IsOnJub(ExtPlayer player)
        {
            var character = player.Character;
            if (character.WorkID > 0) return false;
            var worker = JobService.GetWorkerByUUID(character.UUID);
            return worker.CurrentJob == this;
        }

        public int GetLvlLvl(ExtPlayer player)
        {
            var worker = JobService.GetWorkerByUUID(player.Character.UUID);
            var exp = worker.GetExp(this);
            if(_sorted == null)
                _sorted = Levels.OrderByDescending(l => l.NeededExpiriance).ToList();

            foreach (var item in _sorted)
            {
                if (exp >= item.NeededExpiriance) return item.Id;
            }
            return 0;
        }

        public void AddExpiriance(ExtPlayer player, int exp)
        {
            var worker = JobService.GetWorkerByUUID(player.Character.UUID);
            worker?.AddExp(this, exp);
        }

        public void ParseLevels(string path, string file)
        {
            if (Directory.Exists(path))
            {
                using var w = new StreamWriter(path+file);
                var config = Levels.OrderBy(l=>l.NeededExpiriance).Select(l => new { level = l.Id, exp = l.NeededExpiriance }).ToList();
                w.Write($"export default {JsonConvert.SerializeObject(config)}");
            }
        }

        public bool IsLimit(ExtPlayer player, JobWorker worker)
        {
            return worker.TotalInPayday > NewDonateShop.DonateService.UseJobCoef(player, Limit, player.Character.IsPrimeActive());
        }
        public bool IsLimit(ExtPlayer player)
        {
            var worker = JobService.GetWorkerByUUID(player.Character.UUID);
            return worker.TotalInPayday > NewDonateShop.DonateService.UseJobCoef(player, Limit, player.Character.IsPrimeActive());
        }
    }
}
