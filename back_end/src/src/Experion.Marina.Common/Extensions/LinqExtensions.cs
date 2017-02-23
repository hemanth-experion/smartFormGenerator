using System;
using System.Collections.Generic;
using System.Linq;

namespace Experion.Marina.Common
{
    public static class LinqExtensions
    {
        /// <summary>
        /// Pivots the specified first key selector.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TFirstKey">The type of the first key.</typeparam>
        /// <typeparam name="TSecondKey">The type of the second key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="firstKeySelector">The first key selector.</param>
        /// <param name="source">The source.</param>
        /// <param name="secondKeySelector">The second key selector.</param>
        /// <param name="aggregate">The aggregate.</param>
        /// <returns></returns>
        public static Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>> Pivot<TSource, TFirstKey, TSecondKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TFirstKey> firstKeySelector, Func<TSource, TSecondKey> secondKeySelector, Func<IEnumerable<TSource>, TValue> aggregate)
        {
            var retVal = new Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>>();

            var l = source.ToLookup(firstKeySelector);
            foreach (var item in l)
            {
                var dict = new Dictionary<TSecondKey, TValue>();
                retVal.Add(item.Key, dict);
                var subdict = item.ToLookup(secondKeySelector);
                foreach (var subitem in subdict)
                {
                    dict.Add(subitem.Key, aggregate(subitem));
                }
            }

            return retVal;
        }

        /// <summary>
        /// Transposes the specified values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> values)
        {
            if (values == null)
            {
                return values;
            }
            if (values.First() == null)
            {
                return Transpose(values.Skip(1));
            }

            var x = values.First().First();
            var xs = values.First().Skip(1);
            var xss = values.Skip(1);
            return new[] { new[] { x }.Concat(xss.Select(ht => ht.First())) }
               .Concat(new[] { xs }
               .Concat(xss.Select(ht => ht.Skip(1)))
               .Transpose());
        }

        /// <summary>
        /// Wheres if.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="condition">if set to <c>true</c> [condition].</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        {
            if (condition)
            {
                return source.Where(predicate);
            }
            return source;
        }

        /// <summary>
        /// Wheres if.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="condition">if set to <c>true</c> [condition].</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, int, bool> predicate)
        {
            if (condition)
            {
                return source.Where(predicate);
            }
            return source;
        }

        /// <summary>
        /// Get the distinct entities from the source based on a property.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
