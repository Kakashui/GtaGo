using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Whistler.ServerQuery
{
    class Commands
    {
        public delegate string QueryAction(params string[] args);

        private Dictionary<string, QueryAction> _listCommands = new Dictionary<string, QueryAction>();
        public Commands()
        {
            _listCommands.Add("globalmsg", GlobalMessage);
            _listCommands.Add("killplr", KillPlayer);
            _listCommands.Add("kickplr", KickPlayer);
            _listCommands.Add("cacheplayer", ClearPlayerCache);
        }

        private string ClearPlayerCache(string[] args)
        {
            if (args.Length < 1) return "input socialclubname";
            if (int.TryParse(args[0], out var uuid))
            {
                return "charNoteFound";
            }else 
                return "format \\srv_charprop uuid prop value";
        }

        private string ClearInventoryCache(string[] args)
        {
            if (args.Length < 1) return "input socialclubname";
            if (int.TryParse(args[0], out var id))
            {
                return "charNoteFound";
            }
            else
                return "format \\srv_charprop uuid prop value";
        }

        private string ClearDonateInventoryCache(string[] args)
        {
            if (int.TryParse(args[0], out var id))
            {
                return "charNoteFound";
            }
            else
                return "format \\srv_charprop id";
        }

        public string Call(string command)
        {
            var args = command.Split(' ');
            if (_listCommands.ContainsKey(args[0]))
            {
                var nargs = new string[args.Length - 1];
                Array.Copy(args, 1, nargs, 0, args.Length - 1);
                return _listCommands[args[0]]?.Invoke(nargs);
            }
            else
                return "Command not found";
        }

        private string GlobalMessage(params string[] args)
        {
            var msg = String.Join(' ', args);
            Console.WriteLine(msg);
            NAPI.Task.Run(() =>
            {
                NAPI.ClientEvent.TriggerClientEventForAll("chat:api:action", ChatType.Global, msg, -1);
            });
            return "ok";
        }

        private string KillPlayer(params string[] args)
        {
            var name = args[0].Split("_");
            if (name.Length != 2) return "Bad name";

            var player = NAPI.Pools.GetAllPlayers().FirstOrDefault(p => p.Name == args[0]);
            if (player != null)
            {
                NAPI.Task.Run(() =>
                {
                    player.Kill();
                });
                return "ok";
            }
            else 
                return "Player not found";
        }

        private string KickPlayer(params string[] args)
        {
            var name = args[0].Split("_");
            if (name.Length != 2) return "Bad name";

            var player = NAPI.Pools.GetAllPlayers().FirstOrDefault(p => p.Name == args[0]);
            if (player != null)
            {
                NAPI.Task.Run(() =>
                {
                    player.Kick();
                });
                return "ok";
            }
            else
                return "Player not found";
        }
    }
}
