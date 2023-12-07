using GTANetworkAPI;
using Whistler.Core;
using Whistler.Helpers;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using Whistler.MoneySystem;
using Whistler.Inventory.Enums;
using Whistler.Entities;

namespace Whistler.Fractions.SupplyManagers
{
    public class MaterialsSupply : Script
    {
        public const int MoneyPerHandContainer = 1000;
        public const int CountContainers = 20;

        private static readonly Random Random = new Random();
        private static DateTime SupplyCanBeRequestedAt = DateTime.Now.AddMinutes(-1);
        private static int _nextBoxReload = 50;

        private static List<SupplySpot> SupplySpots = new List<SupplySpot>()
        {
            new SupplySpot(new List<ContainerSlot>
            {
                new ContainerSlot(new Vector3(3074.957, -4757.908, 5.08251), new Vector3(0, 0, 17)),
                new ContainerSlot(new Vector3(3079.957, -4756.308, 5.08251), new Vector3(0, 0, 17))
            }),
        };

        private class SupplySpot
        {
            private List<ContainerSlot> _containerSlots;

            public SupplySpot(List<ContainerSlot> containerSlots)
            {
                _containerSlots = containerSlots;
            }

            public void SpawnContainers()
            {
                foreach (var slot in _containerSlots)
                {
                    slot.DestroyContainer();
                    slot.CreateContainer();
                }
            }
            public void DestroyContainers()
            {
                foreach (var slot in _containerSlots)
                {
                    slot.DestroyContainer();
                }
            }
        }

        private class ContainerSlot
        {
            public Vector3 Position { get; }
            public Vector3 Rotation { get; }

            private Container _container;

            public ContainerSlot(Vector3 position, Vector3 rotation)
            {
                Position = position;
                Rotation = rotation;
            }

            public void CreateContainer()
            {
                if (_container != null)
                {
                    return;
                }

                _container = new Container(Position, Rotation);
            }

            public void DestroyContainer()
            {
                if (_container == null)
                {
                    return;
                }

                _container.Destroy();
                _container = null;
            }
        };

        private class Container
        {
            private int _containersLeft = MaterialsSupply.CountContainers;

            private BoxesSpawner _boxesSpawner;
            private GTANetworkAPI.Object _object;
            private InteractShape _interactShape;
            private Blip _blip;

            public Container(Vector3 position, Vector3 rotation)
            {
                _object = NAPI.Object.CreateObject(NAPI.Util.GetHashKey("prop_container_03_ld"), position, rotation);

                var boxesPosition = position + new Vector3(0, 0, 0.2);

                _interactShape = InteractShape.Create(boxesPosition, 2, 2)
                    .AddInteraction(GetMaterials);

                _blip = NAPI.Blip.CreateBlip(478, position, 0.5f, 76, "Контейнер для оружия", shortRange: true);

                _boxesSpawner = new BoxesSpawner(boxesPosition, rotation, MaterialsSupply.CountContainers);
            }

            public void Destroy()
            {
                if (_object != null)
                    _object.Delete();

                if (_interactShape != null)
                    _interactShape.Destroy();

                if (_blip != null)
                    _blip.Delete();

                if (_boxesSpawner != null)
                    _boxesSpawner.Destroy();
            }

            private void GetMaterials(ExtPlayer player)
            {
                if (!Manager.CanUseCommand(player, "takesupplybox")) return;
                if (player.HasData("gunsupplynextbox") && DateTime.Now < player.GetData<DateTime>("gunsupplynextbox"))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.Bottom, "matsupply_9".Translate(), 3000);
                    return;
                }
                if (_containersLeft == 0)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.Bottom, "matsupply_1".Translate(), 3000);
                    return;
                }
                
                if (player.IsPlayerHaveContainer())
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.Bottom, "matsupply_2".Translate(), 3000);
                    return;
                }

                var boxType = Random.Next(0, 2) == 0 ? ItemNames.WeaponBox : ItemNames.AmmoBox;

                Inventory.Models.ItemBox box = SupplyConfig.GetRandomTypeBox(boxType);
                if (box == null) return;

                _containersLeft--;
                _boxesSpawner.ChangeContainers(_containersLeft);

                Wallet.MoneyAdd(player.Character, MoneyPerHandContainer, "Награда за разгрузку контейнера");

                if (_containersLeft == 0)
                {
                    _blip.Color = 55;
                }

                player.GiveContainerToPlayer(box, box.Config.AttachId);
                SafeTrigger.SetData(player, "gunsupplynextbox", DateTime.Now.AddSeconds(_nextBoxReload));
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, boxType == ItemNames.WeaponBox ? "Вы взяли ящик с оружием, отнесите его в транспортное средство." : "Вы взяли ящик с боеприпасами, отнесите его в транспортное средство.", 3000);
            }

            private class BoxesSpawner
            {
                private static List<(float distOffset, float zOffset)> PositionOffsets = new List<(float distOffset, float zOffset)>
                {
                    (distOffset: 2.4f, zOffset: 0),
                    (distOffset: 1.6f, zOffset: 0),
                    (distOffset: 0.8f, zOffset: 0),
                    (distOffset: -0f, zOffset: 0),
                    (distOffset: -0.8f, zOffset: 0),
                    (distOffset: 2.4f, zOffset: 0.45F),
                    (distOffset: 1.6f, zOffset: 0.45F),
                    (distOffset: 0.8f, zOffset: 0.45F),
                    (distOffset: 0f, zOffset: 0.45F),
                    (distOffset: -0.8f, zOffset: 0.45F),
                    (distOffset: 2f, zOffset: 0.9F),
                    (distOffset: 1.2f, zOffset: 0.9F),
                    (distOffset: 0.4f, zOffset: 0.9F),
                    (distOffset: -0.4f, zOffset: 0.9F),
                };

                private int _materialsPerBox;
                private Stack<GTANetworkAPI.Object> _boxes = new Stack<GTANetworkAPI.Object>();

                public BoxesSpawner(Vector3 position, Vector3 containerRotation, int countBox)
                {
                    _materialsPerBox = countBox / PositionOffsets.Count;

                    var boxRotation = containerRotation;

                    for (int i = 0; i < PositionOffsets.Count; i++)
                    {
                        var offsets = PositionOffsets[i];
                        var boxPosition = position.ApplyDirectionWithRange(boxRotation, offsets.distOffset) + new Vector3(0, 0, offsets.zOffset);

                        _boxes.Push(NAPI.Object.CreateObject(NAPI.Util.GetHashKey("prop_box_ammo03a"), boxPosition, boxRotation));
                    }
                }

                public void ChangeContainers(int currentBoxes)
                {
                    var boxesShouldLeft = Math.Ceiling((double)currentBoxes / _materialsPerBox);

                    if (_boxes.Count > boxesShouldLeft)
                    {
                        var box = _boxes.Pop();
                        box.Delete();
                    }
                }

                public void Destroy()
                {
                    while (_boxes.Count > 0)
                        _boxes.Pop().Delete();
                }
            }
        };

        [RemoteEvent("materialsSupply:dropContainer")]
        public void DropContainerHandler(ExtPlayer player)
        {
            if (!player.IsLogged())
                return;
            player.TakeContainerFromPlayer();
        }

        [Command("requestsupply")]
        public void RequestSupplyHandler(ExtPlayer player)
        {
            if (!Manager.CanUseCommand(player, "requestsupply")) return;

            if (DateTime.Now < SupplyCanBeRequestedAt)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.Bottom, "matsupply_3".Translate( SupplyCanBeRequestedAt.ToString("H:mm")), 3000);
                return;
            }

            foreach (var supply in SupplySpots)
            {
                supply.DestroyContainers();
            }
            var supplySpot = SupplySpots[Random.Next(0, SupplySpots.Count)];
            supplySpot.SpawnContainers();

            var fractionToNotify = new List<int> { 1, 2, 3, 4, 5, 10, 11, 12, 13 };
            foreach (var fracId in fractionToNotify)
                Chat.SendFractionMessage(fracId, "matsupply_8", false);

            Notify.Send(player, NotifyType.Success, NotifyPosition.Bottom, "matsupply_4".Translate(), 3000);
            SupplyCanBeRequestedAt = SupplyCanBeRequestedAt.AddHours(2);
        }

        [Command("requestsupplyadm")]
        public void RequestSupplyHandlerAdmin(ExtPlayer player)
        {
            if (!Group.CanUseAdminCommand(player, "requestsupplyadm")) return;

            foreach (var supply in SupplySpots)
            {
                supply.DestroyContainers();
            }
            var supplySpot = SupplySpots[Random.Next(0, SupplySpots.Count)];
            supplySpot.SpawnContainers();

            var fractionToNotify = new List<int> { 1, 2, 3, 4, 5, 10, 11, 12, 13 };
            foreach (var fracId in fractionToNotify)
                Chat.SendFractionMessage(fracId, "matsupply_8", false);

            Notify.Send(player, NotifyType.Success, NotifyPosition.Bottom, "matsupply_4".Translate(), 3000);
            SupplyCanBeRequestedAt = SupplyCanBeRequestedAt.AddHours(2);
        }

        [Command("deletesupply")]
        public void DeleteSupplyHandler(ExtPlayer player)
        {
            if (!Group.CanUseAdminCommand(player, "deletesupply")) return;

            foreach (var supply in SupplySpots)
            {
                supply.DestroyContainers();
            }
        }
    }
}
