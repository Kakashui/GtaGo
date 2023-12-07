using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Jobs.Farm.Models
{
    class PlantStages
    {
        /// <summary>
        /// Время в процентном соотношении
        /// </summary>
        public double Time { get; set; }
        public string ModelName { get; set; }
        public PlantStages(double time, string modelName)
        {
            Time = time;
            ModelName = modelName;
        }
    }
}
