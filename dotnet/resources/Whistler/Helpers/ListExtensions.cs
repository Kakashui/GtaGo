using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Helpers.Interfaces;

namespace Whistler.Helpers
{
    public static class ListExtensions
    {
        private static Random _rnd = new Random();
        public static void AddIfNotExists<T>(this List<T> list, T element)
        {
            if (!list.Contains(element))
            {
                list.Add(element);
            }
        }
        public static T GetRandomElement<T>(this IEnumerable<T> source)
        {
            if (source.Count() == 0)
                return default;
            return source.ElementAt(_rnd.Next(0, source.Count()));
        }
        public static List<T> GetElementsWithRandomProbability<T>(this List<T> source, double probability)
        {
            Random rnd = new Random();
            return source.Where(item => rnd.NextDouble() <= probability).ToList();
        }

        public static T GetRandomElementWithProbability<T>(this IList<T> source, Func<T, int> probability)
        {
            if (source.Count == 0)
                return default(T);
            int allSum = source.Sum(item => probability(item));
            int sum = 0;
            int index = -1;
            int random = _rnd.Next(0, allSum);
            do
            {
                sum += probability(source[++index]);
            }
            while (sum <= random && index < source.Count);
            return source[index++];
        }

        public static List<T> Sorted<T>(this List<T> source, bool sortByAsc = true, int firstPlace = 1)
            where T: ISortedWithPlace
        {
            if (sortByAsc)
            {
                CompListWithPlaceByAsc<T> sort = new CompListWithPlaceByAsc<T>();
                source.Sort(sort);
            }
            else
            {
                CompListWithPlaceByDesc<T> sort = new CompListWithPlaceByDesc<T>();
                source.Sort(sort);
            }
            foreach (var item in source)
            {
                item.Place = firstPlace++;
            }
            return source;
        }
        class CompListWithPlaceByAsc<T> : IComparer<T>
            where T : ISortedWithPlace
        {
            public int Compare(T x, T y)
            {
                return x.SortedValue - y.SortedValue;
            }
        }
        class CompListWithPlaceByDesc<T> : IComparer<T>
            where T : ISortedWithPlace
        {
            public int Compare(T x, T y)
            {
                return -x.SortedValue + y.SortedValue;
            }
        }
    }
}
