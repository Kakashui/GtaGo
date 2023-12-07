using Whistler.Common;
using Whistler.Entities;
using Whistler.GUI;
using Whistler.Houses;
using Whistler.SDK;
using Whistler.VehicleSystem;

namespace Whistler.Core.Models.Rewards
{
    internal class VehicleReward : RewardBase
    {
        public VehicleReward(string model) : base(model, 1)
        {

        }

        public override bool GiveReward(ExtPlayer player)
        {
            VehicleSystem.Models.VehiclesData.PersonalBaseVehicle vehData = VehicleManager.Create(player.Character.UUID, Name, new GTANetworkAPI.Color(255, 255, 255), new GTANetworkAPI.Color(255, 255, 255), 100, 0, typeOwner: OwnerType.Personal);
            GarageManager.SendVehicleIntoGarage(vehData);
            MainMenu.SendProperty(player);
            GameLog.Admin("system", $"referal_vehicle({Name})", player.Name);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы получили транспортное средство {Name} за реферальную систему.", 5000);
            return true;
        }
    }
}
