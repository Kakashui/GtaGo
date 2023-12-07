using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Phone.Taxi.Dtos
{
    internal class TaxiJobStatsDto
    {
        public int TotalSumForDay { get; set; }

        public int TotalTripsForDay { get; set; }

        public int TotalSumForMonth { get; set; }

        public int TotalTripsForMonth { get; set; }

        public int TotalSum { get; set; }

        public int TotalTrips { get; set; }
    }
}
