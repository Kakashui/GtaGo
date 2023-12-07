using Newtonsoft.Json;
using System;

namespace Whistler.Businesses.Manager.DTOs
{
    public class BizProfitDTO
    {
        /// <summary>
        /// Расходы за время отсчёта (месяц)
        /// </summary>
        public int Expenses { get; private set; } = 0;
        /// <summary>
        /// Доходы за время отсчёта (месяц)
        /// </summary>
        public int Income { get; private set; } = 0;
        /// <summary>
        /// Расходы за всё время владения бизнесом
        /// </summary>
        public int TotalExpenses { get; private set; } = 0;
        /// <summary>
        /// Доходы за всё время владения бизнесом
        /// </summary>
        public int TotalIncome { get; private set; } = 0;
        /// <summary>
        /// Дата, с начала которой ведётся отчёт
        /// </summary>
        public DateTime RecordSince { get; private set; }
        /// <summary>
        /// Дата, до которой ведётся отчёт
        /// </summary>
        public DateTime RecordUntil { get; private set; }

        /// <summary>
        /// Прибыль за время отчёта (месяц)
        /// </summary>
        [JsonIgnore]
        public int Profit 
        { 
            get 
            {
                return Income - Expenses;
            } 
        }

        /// <summary>
        /// Общая прибыль за всё время
        /// </summary>
        [JsonIgnore]
        public int TotalProfit
        {
            get
            {
                return TotalIncome - TotalExpenses;
            }
        }

        [JsonConstructor]
        public BizProfitDTO(int expenses, int income, int totalExpenses, int totalIncome, DateTime recordSince, DateTime recordUntil)
        {
            Expenses = expenses;
            Income = income;
            TotalExpenses = totalExpenses;
            TotalIncome = totalIncome;
            RecordSince = recordSince;
            RecordUntil = recordUntil;
        }

        public BizProfitDTO(DateTime date)
        {
            SetupRecordDate(date);
        }

        /// <summary>
        /// Изменить дату отчёта, при этом отчёт (кроме общего) будет сброшен.
        /// </summary>
        /// <param name="date"></param>
        public void SetupRecordDate(DateTime date, bool withReset = false)
        {
            RecordSince = new DateTime(date.Year, date.Month, 1); // Получаем первый день месяца
            RecordUntil = RecordSince.AddMonths(1).AddSeconds(-1); // Получаем последний день того же месяца
            if (withReset) Reset();
        }

        /// <summary>
        /// Записать новый доход
        /// </summary>
        /// <param name="amount">Сумма денег</param>
        public void NewIncome(int amount)
        {
            Income += amount;
            TotalIncome += amount;
        }

        /// <summary>
        /// Записать новый расход
        /// </summary>
        /// <param name="amount">Сумма денег</param>
        public void NewExpenses(int amount)
        {
            Expenses += amount;
            TotalExpenses += amount;
        }

        /// <summary>
        /// Сбросить расходы и доходы за время отчёта (месяц)
        /// </summary>
        public void Reset()
        {
            Expenses = 0;
            Income = 0;
        }

        /// <summary>
        /// Сбросить расходы и доходы за всё время и времё отчёта
        /// </summary>
        public void FullReset()
        {
            Reset();
            TotalExpenses = 0;
            TotalIncome = 0;
        }
    }
}
