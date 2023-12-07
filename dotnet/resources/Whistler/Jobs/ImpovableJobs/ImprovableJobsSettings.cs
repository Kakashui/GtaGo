using GTANetworkAPI;
using System.Collections.Generic;

namespace Whistler.Jobs.ImpovableJobs
{
    internal static class ImprovableJobsSettings
    {
        private static Dictionary<ProductsLoaderJobStages, int> _productLoaderActionsToNewLevelCount = 
            new Dictionary<ProductsLoaderJobStages, int>();

        public static IReadOnlyDictionary<ProductsLoaderJobStages, int> ProductLoaderActionsToNewLevelCount =>
            _productLoaderActionsToNewLevelCount;

        static ImprovableJobsSettings()
        {
            _productLoaderActionsToNewLevelCount.Add(ProductsLoaderJobStages.DockLoader, 30);
        }
    }
}