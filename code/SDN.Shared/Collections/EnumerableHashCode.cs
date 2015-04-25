using System.Collections;
using System.Linq;

namespace SDN.Shared.Collections
{
    /// <summary>
    /// Provides methods for calculating hashcodes of collections.
    /// </summary>
    public static class EnumerableHashCode
    {
        /// <summary>
        /// Calculates hashcode of the specified list. Hashcode is based on hashcodes of elements. 
        /// Result does not depend from collection type.
        /// </summary>
        /// <param name="list">Target list.</param>
        /// <param name="ordered">If true then result value depends from element order, otherwise false.</param>
        /// <returns>Hashcode of the specified list.</returns>
        public static int GetHashCode(IList list, bool ordered)
        {
            Check.ObjectIsNotNull(list, "list");

            return ordered ? GetHashCode(list) : GetHashCode(list as IEnumerable);
        }

        /// <summary>
        /// Calculates hashcode of the specified list. Hashcode is based on hashcodes of elements and their order. 
        /// Result does not depend on collection type.
        /// </summary>
        /// <param name="list">Target list.</param>
        /// <returns>Hashcode of the specified list.</returns>
        public static int GetHashCode(IList list)
        {
            Check.ObjectIsNotNull(list, "list");

            int result = 0;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null)
                    result ^= (list[i].GetHashCode() ^ i);
            }

            result ^= list.Count.GetHashCode();

            return result;
        }

        /// <summary>
        /// Calculates hashcode of the specified dictionary. Hashcode is based on hashcodes of keys and associated values. 
        /// Result does not depend on collection type.
        /// </summary>
        /// <param name="dictionary">Target dictionary.</param>
        /// <returns>Hashcode of the specified dictionary.</returns>
        public static int GetHashCode(IDictionary dictionary)
        {
            Check.ObjectIsNotNull(dictionary, "dictionary");

            int result = dictionary.Keys.Cast<object>().Where(key => key != null).Aggregate(0, (current, key) => current ^ key.GetHashCode());

            return dictionary.Values.Cast<object>().Where(value => value != null).Aggregate(result, (current, value) => current ^ value.GetHashCode());
        }

        /// <summary>
        /// Calculates hashcode of the specified enumerable. Hashcode is based on hashcodes of elements. 
        /// </summary>
        /// <param name="enumerable">Target enumerable.</param>
        /// <returns>Hashcode of the specified enumerable.</returns>
        public static int GetHashCode(IEnumerable enumerable)
        {
            Check.ObjectIsNotNull(enumerable, "enumerable");

            int result = 0;

            int count = 0;
            foreach (object currentItem in enumerable)
            {
                if (currentItem != null)
                {
                    int currenthashCode = currentItem.GetHashCode();
                    result ^= currenthashCode;
                }
                count++;
            }
            result ^= count.GetHashCode();

            return result;
        }
    }
}
