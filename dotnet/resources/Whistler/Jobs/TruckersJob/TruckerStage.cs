using System.Collections.Generic;
using GTANetworkAPI;

namespace Whistler.Jobs.TruckersJob
{
    internal class TruckerStage
    {
        /// <summary>
        /// Количество перевозок для нового уровня
        /// </summary>
        public int RequiredTransportations { get; }

        /// <summary>
        /// Зарплата за разгрузку
        /// </summary>
        public int PaymentPerUnload { get; }

        public List<(Vector3, float)> LoadPoints { get; }
        public List<Vector3> UnloadPoints { get; }
        
        public int RentCost { get; }
        
        public string TrailerHash { get; }

        public int? MCoinsReward { get; }

        public TruckerStage(int requiredTransportations, int rentCost, int paymentPerUnload, List<(Vector3, float)> loadPoints, List<Vector3> unloadPoints, int? mCoinsReward = null, string? trailerHash = null)
        {
            PaymentPerUnload = paymentPerUnload;
            RentCost = rentCost;
            LoadPoints = loadPoints;
            UnloadPoints = unloadPoints;
            MCoinsReward = mCoinsReward;
            RequiredTransportations = requiredTransportations;
            TrailerHash = trailerHash;
        }
    }
}