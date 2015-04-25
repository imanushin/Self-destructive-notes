using System.Collections.Generic;

namespace SDN.Shared.Collections
{
    /// <summary>
    /// Provides additional methods for the System.Collections.Generic.ICollection interface.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds multiple values to the collection.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The current collection.</param>
        /// <param name="values">Values to add.</param>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> values)
        {
            Check.ObjectIsNotNull(collection, "collection");
            Check.ObjectIsNotNull(values, "values");

            values.ForEach(collection.Add);
        }
    }
}
