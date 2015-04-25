using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SDN.Shared.Collections
{
    /// <summary>
    /// Provides methods for comparing collections. 
    /// </summary>
    public static class EnumerableComparer
    {
        /// <summary>
        /// Compares two lists. Lists whose elements are equal and are in the same order are considered equal even 
        /// if collections have different types.
        /// </summary>
        /// <param name="firstList">First list.</param>
        /// <param name="secondList">Second list.</param>
        /// <returns>True if lists are considered equal.</returns>
        public static bool Compare<T>(IList<T> firstList, IList<T> secondList)
        {
            bool result = ReferenceEquals(firstList, secondList);

            if (!result && (firstList != null) && (secondList != null))
            {
                result = true;

                if (firstList.Count != secondList.Count)
                {
                    result = false;
                }
                else
                {
                    if (firstList.Select((item, index) => Equals(item, secondList[index])).Any(currentCheckResult => !currentCheckResult))
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Compares two dictionaries. Dictionaries whose elements are equal are considered equal even 
        /// if collections have different types.
        /// </summary>
        /// <param name="firstDictionary">First dictionary.</param>
        /// <param name="secondDictionary">Second dictionary.</param>
        /// <returns>True if dictionaries are considered equal.</returns>
        public static bool Compare(IDictionary firstDictionary, IDictionary secondDictionary)
        {
            if (ReferenceEquals(firstDictionary, secondDictionary))
                return true;

            if ((firstDictionary == null) || (secondDictionary == null))
                return false;

            if (EnumerableHashCode.GetHashCode(firstDictionary) != EnumerableHashCode.GetHashCode(secondDictionary))
            {
                return false;
            }

            if (firstDictionary.Count != secondDictionary.Count)
            {
                return false;
            }

            foreach (object firstKey in firstDictionary.Keys)
            {
                if (!secondDictionary.Contains(firstKey))
                    return false;

                object firstValue = firstDictionary[firstKey];
                object secondValue = secondDictionary[firstKey];

                if (!Equals(firstValue, secondValue))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Compares two untyped enumerables. Enumerables whose elements are equal are considered equal even 
        /// if collections have different types. This method also can compare dictionaries and lists but it is much slower.
        /// </summary>
        /// <param name="firstEnumerable">First enumerable.</param>
        /// <param name="secondEnumerable">Second enumerable.</param>
        /// <returns>True if enumerables are considered equal.</returns>
        public static bool Compare<T>(IEnumerable<T> firstEnumerable, IEnumerable<T> secondEnumerable)
        {
            if (ReferenceEquals(firstEnumerable, secondEnumerable))
                return true;

            if ((firstEnumerable == null) || (secondEnumerable == null))
                return false;

            Dictionary<T, int> firstCounts = GetCounts(firstEnumerable);
            Dictionary<T, int> secondCounts = GetCounts(secondEnumerable);

            return Compare((IDictionary)firstCounts, secondCounts);
        }

        private static Dictionary<T, int> GetCounts<T>(IEnumerable<T> data)
        {
            var result = new Dictionary<T, int>();

            foreach (T item in data)
            {
                if (result.ContainsKey(item))
                    result[item]++;
                else
                    result[item] = 1;
            }

            return result;
        }
    }
}