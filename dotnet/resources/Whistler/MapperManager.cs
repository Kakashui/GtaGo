using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Whistler
{
    public class MapperManager
    {
        private static readonly IReadOnlyCollection<Profile> profiles;
        private static readonly MapperConfiguration mapperConfiguration;

        public static void Init() { }

        static MapperManager()
        {
            profiles = GetInstancesImplementsType<Profile>().ToList();
            mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfiles(profiles);
            });
        } 

        public static Mapper Get()
        {
            return new Mapper(mapperConfiguration);
        }

        private static IEnumerable<T> GetInstancesImplementsType<T>()
        {
            AppDomain app = AppDomain.CurrentDomain;
            Assembly[] ass = app.GetAssemblies();
            Type[] types;
            Type targetType = typeof(T);

            foreach (Assembly a in ass)
            {
                types = a.GetTypes();
                foreach (Type t in types)
                {
                    if (t.IsInterface) continue;
                    if (t.IsAbstract) continue;

                    var typesArray = new Type[0];
                    if (t.IsSubclassOf(targetType) && t.GetConstructor(typesArray) != null)
                    {
                        yield return (T)Activator.CreateInstance(t);
                    }
                }
            }
        }
    }
}
