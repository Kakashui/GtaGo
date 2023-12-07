using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Whistler.MP.Arena.Locations;

namespace Whistler.MP.Arena.Racing
{
    public class GameEventsList
    {
        public IReadOnlyList<RacingMap> Events => _mapQueue.ToList();
        private Queue<RacingMap> _mapQueue = new Queue<RacingMap>();

        public RacingMap NextEvent()
        {
            var mapInOrder = _mapQueue.Peek();
            AddEventToList(mapInOrder.RacingName);

            return mapInOrder;
        }

        public void InsertFromQueue()
        {
            _mapQueue.Dequeue();
        }
        
        public void LoadMaps()
        {
            PreloadEventToList(RacingName.SuperCarRace);
            PreloadEventToList(RacingName.F1);
            PreloadEventToList(RacingName.MotoRace);
            PreloadEventToList(RacingName.VeloRace);
            PreloadEventToList(RacingName.OffroadRace);
        }

        private void PreloadEventToList(RacingName name)
        {
            _mapQueue.Enqueue(GameEventsFactory.CreateRacing(name, 
                DateTime.Now.AddMilliseconds(_mapQueue.Count * RacingSettings.CreationDelayTime).AddMilliseconds(RacingSettings.RegistrationTime)));
        }
        
        public void AddEventToList(RacingName name)
        {
            _mapQueue.Enqueue(GameEventsFactory.CreateRacing(name,
                _mapQueue.Peek().StartTime.AddMilliseconds(_mapQueue.Count * RacingSettings.CreationDelayTime).AddMilliseconds(RacingSettings.RegistrationTime)));
        }
    }
}