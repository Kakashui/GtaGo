using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Whistler.UpdateR.Internal
{
    internal static class AssemblyScanner
    {
        public static Dictionary<Type, List<Type>> GetUpdateRTypes(IEnumerable<Assembly> assembliesToScan)
        {
            assembliesToScan = assembliesToScan.Distinct().ToArray();

            var updateImplTypes = new List<Type>();
            var updateHandlersTypes = new List<Type>();

            var updateInterface = typeof(IUpdate<>);
            var updateHandlerInterface = typeof(IUpdateHandler<,>);

            var handlersTypesByUpdateType = new Dictionary<Type, List<Type>>();

            foreach (var type in assembliesToScan.SelectMany(a => a.DefinedTypes))
            {
                if (!type.IsConcrete()) continue;

                var typename = type.GetTypeInfo().FullName;

                if (type.GetTypeInfo().ImplementedInterfaces.Where(t => t.GetTypeInfo().IsGenericType).Any(t => t.GetGenericTypeDefinition() == updateInterface))
                {
                    updateImplTypes.Fill(type);
                }

                if (type.GetTypeInfo().ImplementedInterfaces.Where(t => t.GetTypeInfo().IsGenericType).Any(t => t.GetGenericTypeDefinition() == updateHandlerInterface))
                {
                    updateHandlersTypes.Add(type);
                }
            }

            foreach (var updateType in updateImplTypes)
            {
                var updateHandlers = updateHandlersTypes.Where(t => t.IsHandlerForUpdate(updateType)).ToList();
                handlersTypesByUpdateType.Add(updateType, updateHandlers);
            }

            return handlersTypesByUpdateType;
        }

        private static void Fill<T>(this List<T> list, T item)
        {
            if (list.Contains(item)) return;
            list.Add(item);
        }

        private static bool IsHandlerForUpdate(this Type typeToCheck, Type updateType)
        {
            var typeInfo = typeToCheck.GetTypeInfo();

            var handlersInterfaces = typeToCheck.GetTypeInfo().GetInterfaces()
                .Where(t => t.GetTypeInfo().IsGenericType)
                .Where(t => t.GetTypeInfo().GetGenericTypeDefinition() == typeof(IUpdateHandler<,>));

            return handlersInterfaces.Any(t => t.GetTypeInfo().GenericTypeArguments[0] == updateType);
        }

        private static bool IsConcrete(this Type type)
        {
            return !type.GetTypeInfo().IsAbstract && !type.GetTypeInfo().IsInterface;
        }
    }
}
