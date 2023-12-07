using GTANetworkAPI;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Whistler.UpdateR.Internal
{
    internal abstract class SubscribersStorageBase
    {
        public abstract void RemoveFromAll(Player subscriber);
    }

    internal class SubscribersStorage<TKey> : SubscribersStorageBase
    {
        private readonly ConcurrentDictionary<TKey, IReadOnlyCollection<Player>> _subscribers = new ConcurrentDictionary<TKey, IReadOnlyCollection<Player>>();

        public void AddSubscriber(TKey key, Player subscriber)
        {
            bool updateSuccessfull = false;

            do
            {
                var currentSubscribers = _subscribers.GetOrAdd(key, t => new List<Player>());

                var updatedSubscribers = new List<Player>(currentSubscribers);

                if (updatedSubscribers.Contains(subscriber))
                {
                    return;
                }

                updatedSubscribers.Add(subscriber);

                updateSuccessfull = _subscribers.TryUpdate(key, updatedSubscribers, currentSubscribers);
            }
            while (!updateSuccessfull);
        }

        public void RemoveSubscriber(TKey key, Player subscriber)
        {
            bool updateSuccessfull = false;

            do
            {
                var currentSubscribers = _subscribers.GetOrAdd(key, t => new List<Player>());

                var updatedSubscribers = new List<Player>(currentSubscribers);
                updatedSubscribers.Remove(subscriber);

                updateSuccessfull = _subscribers.TryUpdate(key, updatedSubscribers, currentSubscribers);
            }
            while (!updateSuccessfull);
        }

        public override void RemoveFromAll(Player subscriber)
        {
            foreach (var key in _subscribers.Keys)
            {
                bool isUpdateSuccessfull = false;

                do
                {
                    var isKeyFound = _subscribers.TryGetValue(key, out var currentSubscribers);
                    if (!isKeyFound)
                    {
                        break;
                    }

                    var updatedSubscribers = new List<Player>(currentSubscribers);
                    updatedSubscribers.Remove(subscriber);

                    isUpdateSuccessfull = _subscribers.TryUpdate(key, updatedSubscribers, currentSubscribers);
                }
                while (!isUpdateSuccessfull);
            }
        }

        public IEnumerable<Player> GetAllSubscribers(TKey key)
        {
            if (!_subscribers.TryGetValue(key, out var currentSubscribers))
            {
                return new List<Player>();
            }
            else
            {
                return currentSubscribers;
            }
        }
    }
}
