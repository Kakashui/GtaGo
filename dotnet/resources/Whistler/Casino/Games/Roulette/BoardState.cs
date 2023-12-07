using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace ServerGo.Casino.Games.Roulette
{
    /// <summary>
    /// Represents roulette board states
    /// </summary>
    internal enum BoardState
    {
        [Display(Name = "casino_waitingForGamblers")]
        WaitingForGamblers,
        [Display(Name = "casino_waitingForBets")]
        WaitingForBets,
        [Display(Name = "casino_calculatingWinner")]
        CalculatingWinner,
        [Display(Name = "casino_showingResult")]
        ShowingResult
    }
    
    public static class Extensions
    {
        /// <summary>
        ///     A generic extension method that aids in reflecting 
        ///     and retrieving any attribute that is applied to an `Enum`.
        /// </summary>
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) 
            where TAttribute : Attribute
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<TAttribute>();
        }
    }

    internal class BoardStateDto
    {
        public int Seconds { get; set; }
        public string Name { get; set; }
    }
}
