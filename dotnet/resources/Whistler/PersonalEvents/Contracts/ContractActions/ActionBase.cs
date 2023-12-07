using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.PersonalEvents.Contracts.Models;

namespace Whistler.PersonalEvents.Contracts.ContractActions
{
    abstract class ActionBase
    {
        abstract public void PlayerInteract(ExtPlayer player, ContractModel contractModel, GTANetworkAPI.Vector3 point);
    }
}
