using GTANetworkAPI;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.NewJobs;
using Whistler.SDK;

namespace Whistler.MiniGames.MakeWeapon
{
    class MakeWeaponPoint
    {
        public Player CurrentWorker { get; set; } = null;
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }

        public MakeWeaponPoint(float posX, float posY, float posZ, float rotX, float rotY, float rotZ)
        {
            Position = new Vector3(posX, posY, posZ);
            Rotation = new Vector3(rotX, rotY, rotZ);
            InteractShape.Create(new Vector3(Position.X, Position.Y, Position.Z - 1), .5f, 2)
               .AddDefaultMarker()
               .AddInteraction(WorkBegine, "interact_41");
        }
        public MakeWeaponPoint(Vector3 pos, Vector3 rot)
        {
            Position = pos;
            Rotation = rot;
            InteractShape.Create(new Vector3(Position.X, Position.Y, Position.Z - 1), .5f, 2)
               .AddDefaultMarker()
               .AddInteraction(WorkBegine, "interact_41");
        }

        private void WorkBegine(ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            if (!MakeWeaponService.Job.IsOnJub(player))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "mg:mw:job:no", 3000);
                return;
            }          
            var character = player.Character;
            if(character.Money < MakeWeaponService.GameCost)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "mg:mw:money:enough", 3000);
                return;
            }
            CurrentWorker = player;
            SafeTrigger.ClientEvent(player,"mg:makeweapon:game:open", JobService.GetWorkerByUUID(character.UUID).Expiriance);           
        }

    }
}
