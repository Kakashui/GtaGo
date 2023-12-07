using GTANetworkAPI;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Helpers;
using Whistler.Entities;

namespace Whistler.Jobs.AbstractEntity
{
    abstract class AbstractWorker
    {
        protected static int _blockNextActionMinutes = 1;
        public ExtPlayer Player { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }

        public AbstractWorker(ExtPlayer client)
        {
            try
            {
                this.Player = client;
                this.Name = client.Name;
                this.Id = client.Value;
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.Worker(ExtPlayer client): {ex.ToString()}");
            }
        }

        public void SendMessage(string message, int time)
        {
            try
            {
                if (Player == null) return;
                Notify.Send(Player, NotifyType.Info, NotifyPosition.BottomCenter, message, time);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.SendMessage(): {ex.ToString()}");
            }
        }

        public void DeleteWaypointAndCheckPoint()
        {
            try
            {
                Player.DeleteClientMarker(15);
                SafeTrigger.ClientEvent(Player, "deleteWorkBlip");
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Policeman.DeleteWaypointAndCheckPoint(): {ex.ToString()}");
            }
        }

        public virtual void CreateWaypoint(Vector3 point)
        {
            try
            {
                Player.CreateWaypoint(point);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Thief.CreateWaypoint(): {ex.ToString()}");
            }
        }

        public  void CreateCheckpoint(Vector3 point)
        {
            try
            {
                Player.CreateClientCheckpoint(15, 1, point, 1, 0, new Color(255, 0, 0));
                SafeTrigger.ClientEvent(Player, "createWorkBlip", point);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Thief.CreateCheckpoint(): {ex.ToString()}");
            }
        }

        public virtual bool CheckAccessNextAction()
        {
            if (Player.HasData("lastWorkAction"))
            {
                if (DateTime.Now.Subtract(Player.GetData<DateTime>("lastWorkAction")).TotalMinutes < _blockNextActionMinutes)
                {
                    Notify.SendError(Player, "Frac_63");
                    return false;
                }
            }
            return true;
        }
    }
}
