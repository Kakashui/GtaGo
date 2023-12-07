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

        public List<(Vector3, float?)> LoadPoints { get; }
        public List<Vector3> UnloadPoints { get; }
        
        public int RentCost { get; }
        
        public uint? TrailerHash { get; }

        public int? GoCoinsReward { get; }

        public TruckerStage(int requiredTransportations, int rentCost, int paymentPerUnload, List<(Vector3, float?)> loadPoints, List<Vector3> unloadPoints, int? goCoinsReward = null, uint? trailerHash = null)
        {
            PaymentPerUnload = paymentPerUnload;
            RentCost = rentCost;
            LoadPoints = loadPoints;
            UnloadPoints = unloadPoints;
            GoCoinsReward = goCoinsReward;
            RequiredTransportations = requiredTransportations;
            TrailerHash = trailerHash;
        }
    }
}