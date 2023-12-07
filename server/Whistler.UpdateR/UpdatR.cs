using GTANetworkAPI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Whistler.UpdateR.Internal;

namespace Whistler.UpdateR
{
    public class UpdatR
    {
        private static readonly ConcurrentDictionary<Type, SubscribersStorageBase> _subscribers = new ConcurrentDictionary<Type, SubscribersStorageBase>();
        private static ReadOnlyDictionary<Type, List<Type>> _requestHandlersByUpdateType;
        private static bool _isInit;

        public static void Init(params Assembly[] assemblies)
        {
            if (_isInit)
            {
                throw new InvalidOperationException("UpdateR is already initialized.");
            }

            _isInit = true;

            _requestHandlersByUpdateType = new ReadOnlyDictionary<Type, List<Type>>(AssemblyScanner.GetUpdateRTypes(assemblies.AsEnumerable()));
        }

        /// <summary>
        /// Подписывает объект на обновления <paramref name="updateTarget"/>.
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="subscriber"></param>
        /// <param name="updateTarget">
        /// Объект с обязательно переопределенными потоко-безопасным
        /// образом методами <see cref="object.GetHashCode"/> и <see cref="object.Equals"/>.</param>
        public static void Subscribe<TTarget>(Player subscriber, TTarget updateTarget)
        {
            if (updateTarget == null)
            {
                throw new ArgumentNullException(nameof(updateTarget));
            }

            var targetType = updateTarget.GetType();

            CheckOnOverridedMethod(targetType, "GetHashCode");
            CheckOnOverridedMethod(targetType, "Equals");

            var storage = (SubscribersStorage<TTarget>)GetOrCreateStorage(targetType);

            storage.AddSubscriber(updateTarget, subscriber);
        }

        /// <summary>
        /// Отписывает <paramref name="subscriber"/> от обновлений конкретного объекта.
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="subscriber"></param>
        /// <param name="updateTarget"></param>
        public static void Unsubscribe<TTarget>(Player subscriber, TTarget updateTarget)
        {
            if (updateTarget == null)
            {
                throw new ArgumentNullException(nameof(updateTarget));
            }

            var targetType = updateTarget.GetType();

            var storage = (SubscribersStorage<TTarget>)GetOrCreateStorage(targetType);

            storage.RemoveSubscriber(updateTarget, subscriber);
        }

        /// <summary>
        /// Отписывает <paramref name="subscriber"/> от всех объектов, на которые он подписан.
        /// </summary>
        public static void UnsubscribeFromAll(Player subscriber)
        {
            foreach (var key in _subscribers.Keys)
            {
                var isSuccesfull = _subscribers.TryGetValue(key, out var storage);
                if (!isSuccesfull)
                {
                    continue;
                }

                storage.RemoveFromAll(subscriber);
            }
        }

        /// <summary>
        /// Обрабатывает обновление
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="target"></param>
        /// <param name="update"></param>
        public static async Task SendUpdate<TTarget, TUpdate>(TTarget target, TUpdate update)
            where TUpdate : IUpdate<TTarget>
        {
            var updateType = update.GetType();
            var targetType = target.GetType();

            var storage = (SubscribersStorage<TTarget>)GetOrCreateStorage(targetType);

            var subscribers = storage.GetAllSubscribers(target);

            var handlers = _requestHandlersByUpdateType[updateType];
            foreach (var handlerType in handlers)
            {
                var typeInfo = handlerType.GetTypeInfo();
                var handler = (IUpdateHandler<TUpdate, TTarget>)Activator.CreateInstance(typeInfo);
                foreach (var subscriber in subscribers)
                {
                    await handler.Handle(subscriber, update);
                }
            }
        }

        private static void CheckOnOverridedMethod(Type type, string methodName)
        {
            var method = type.GetMethod(methodName);
            if (method.DeclaringType == typeof(object))
            {
                throw new InvalidOperationException($"{methodName} method is not overrided for {type.FullName}. You have to override {methodName} method.");
            }
        }

        private static SubscribersStorageBase GetOrCreateStorage(Type targetType)
        {
            return _subscribers.GetOrAdd(targetType,
                t => (SubscribersStorageBase)Activator.CreateInstance(typeof(SubscribersStorage<>).MakeGenericType(targetType)));
        }
    }
}