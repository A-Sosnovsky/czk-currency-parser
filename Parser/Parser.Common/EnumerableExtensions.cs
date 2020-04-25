using System;
using System.Collections.Generic;
using System.Linq;

namespace Parser.Common
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
        
        public static decimal Median(this IEnumerable<decimal> input)
        {
            var array = input?.ToArray();

            if (array.IsNullOrEmpty())
            {
                throw new ArithmeticException("Can't calculate median with empty or null sequence");
            }
            
            Array.Sort(array);

            var n = array.Length;

            var isOdd = n % 2 != 0;
            if (isOdd)
            {
                return array[(n + 1) / 2 - 1];
            }
            else
            {
                return (array[n / 2 - 1] + array[n / 2]) / 2;
            }
        }
    }
}