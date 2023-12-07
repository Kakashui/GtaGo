using System;
using System.Collections.Generic;
using System.Text;
using Whistler.NewDonateShop;

namespace Whistler.VehicleSystem
{
    class VehicleConstants
    {
        /// <summary>
        /// Номер водительского сиденья
        /// </summary>
        public const int DriverSeat = 0;
        /// <summary>
        /// Номер водительского сиденья на клиенте сломанный
        /// </summary>
        public const int DriverSeatClientSideBroken = -1;
        /// <summary>
        /// Пассажир номер 1
        /// </summary>
        public const int Passenger1 = 1;
        /// <summary>
        /// Интервал в метрах, через который происходит обновление хп двигателя
        /// </summary>
        public const int DistanceUpdateEngineHealth = 1000;

        /// <summary>
        /// Количество снимаемых хп при разрушении двигателя
        /// </summary>
        public const int EngineBrokenHealth = 10;

        /// <summary>
        /// Коэффициент поломки двигателя от пройденного расстояния при желтом датчике масла
        /// </summary>
        public const float CoefYellowEngineBroke = 0.2F;

        /// <summary>
        /// Коэффициент поломки двигателя от пройденного расстояния при красном датчике масла
        /// </summary>
        public const float CoefRedEngineBroke = 2F;

        /// <summary>
        /// Количество снимаемых хп двигателя при пробеге DistanceUpdateEngineHealth м
        /// </summary>
        public const float MileageEngineBroke = 0.05F;

        /// <summary>
        /// Пройденное расстояние до замены масла (желтая лампа загорается) (м)
        /// </summary>
        public const int MileageOilChange = 1000000;

        /// <summary>
        /// Пройденное расстояние до красной лампы (м)
        /// </summary>
        public const int MileageOilRedSignal = 1100000;

        /// <summary>
        /// Пройденное расстояние до поломки тормозных колодок (м)
        /// </summary>
        public const int MileageBrakeBroken = 2000000;

        /// <summary>
        /// Пройденное расстояние до обслуживания трансмиссии (м)
        /// </summary>
        public const int MileageTransmissionService = 2000000;

        /// <summary>
        /// Пройденное расстояние до поломки трансмиссии (м)
        /// </summary>
        public const int MileageTransmissionBroken = 5000000;

        /// <summary>
        /// 1111 для проверки дверей
        /// </summary>
        public const int CheckBrokenDoor = 15;

        public const float SaleVehicleCoeff = 0.2F;

        /// <summary>
        /// Скорость поломки двигателя на каждые DistanceUpdateEngineHealth метров
        /// </summary>
        /// <param name="distanceDontOilChange"></param>
        /// <returns></returns>
        public static float GetMileageEngineBroke(double distanceDontOilChange)
        {
            float coef = distanceDontOilChange < MileageOilChange ? 1 : (distanceDontOilChange >= MileageOilRedSignal ? CoefRedEngineBroke : CoefYellowEngineBroke);
            return coef * MileageEngineBroke;
        }

        public static bool CheckUpdateEngineState(double miliage, float currMiliage)
        {
            return (int)(miliage - currMiliage) / DistanceUpdateEngineHealth != (int)miliage / DistanceUpdateEngineHealth;
        }

        public static float GetTransmissCoef(double distanceDontOilChange)
        {
            float coef = 1 - (float)(distanceDontOilChange - MileageTransmissionService) / (MileageTransmissionBroken - MileageTransmissionService);
            if (coef <= 0.5)
                return 0.5F;
            else if (coef >= 1)
                return 1;
            else
                return (float)Math.Round(coef, 1);
        }

        public static int GetCorrectSalePrice(int price, bool prime)
        {
            return Convert.ToInt32(price * (prime ? DonateService.PrimeAccount.SellVehicleMultipler : SaleVehicleCoeff));
        }
    }
}
