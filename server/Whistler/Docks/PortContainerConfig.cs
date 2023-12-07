using System.Collections.Generic;

namespace Whistler.Docks
{
    internal class PortContainerConfig
    {
        public static IReadOnlyDictionary<ContainerSize, string> ContainerObjectModels => _containerObjectModels;
        private static Dictionary<ContainerSize, string> _containerObjectModels = new Dictionary<ContainerSize, string>
        {
            [ContainerSize.Small] = "prop_ld_container",
            [ContainerSize.Medium] = "prop_container_04a",
            [ContainerSize.Big] = "prop_container_02a"
        };
        
        /// <summary>
        /// Сколько ящиков вмещает контейнер
        /// </summary>
        public static IReadOnlyDictionary<ContainerSize, int> ContainerCapacity => _containerCapacity;
        private static Dictionary<ContainerSize, int> _containerCapacity = new Dictionary<ContainerSize, int>
        {
            [ContainerSize.Small] = 0,
            [ContainerSize.Medium] = 100000,
            [ContainerSize.Big] = 440000,
        };
    }
}