using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.Entities;
using Whistler.PersonalEvents.Contracts.ContractActions;

namespace Whistler.PersonalEvents.Contracts.Models
{
    class ContractCoords
    {
        public string Name { get; set; }
        public List<Vector3> Positions { get; set; }
        public ActionBase ActionModel { get; set; }
        public bool IsOneInteract; 
        public ContractCoords(string name, Vector3 position, ActionBase actionModel)
        {
            Name = name;
            Positions = new List<Vector3> { position };
            ActionModel = actionModel;
            IsOneInteract = false;
        }
        public ContractCoords(string name, List<Vector3> positions, ActionBase actionModel)
        {
            Name = name;
            Positions = positions;
            ActionModel = actionModel;
            IsOneInteract = true;
        }
        public void CreateInteractions(Func<ColShape, ExtPlayer, bool> enterPredicate, ContractModel contractModel)
        {
            foreach (var position in Positions)
            {
                var shape = InteractShape.Create(position, 2, 2, 0)
                    .AddInteraction(p => ActionModel.PlayerInteract(p, contractModel, IsOneInteract ? position : null))
                    .AddEnterPredicate(enterPredicate);
            }
        }

    }
}
