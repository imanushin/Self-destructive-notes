using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SDN.WP;

namespace Deltacorvi.PortableCommon.Collections
{
    /// <summary>
    /// Provides additional methods for default enumerables.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Converts the current enumerable to read-only list.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
        /// <param name="source">The current enumerable.</param>
        /// <returns>ReadOnlyList that contains the same elements as the current enumerable.</returns>
        public static ReadOnlyList<T> ToReadOnlyList<T>(this IEnumerable<T> source)
        {
            Check.ObjectIsNotNull(source, "source");

            return new ReadOnlyList<T>(source);
        }

        /// <summary>
        /// Фильтрует коллекцию: пропускает все null'ы.
        /// </summary>
        public static IEnumerable<T> SkipDefault<T>(this IEnumerable<T> source)
        {
            Check.ObjectIsNotNull(source, "source");

            return source.Where(item => !Equals(item, default(T)));
        }

        /// <summary>
        /// Performs the specified action for all items in the collection.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
        /// <param name="source">The current enumerable.</param>
        /// <param name="action">Action that should be performed.</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            Check.ObjectIsNotNull(source, "source");

            foreach (T item in source)
                action(item);
        }

        /// <summary>
        /// Создает ReadOnlySet для коллекции, которая не является им.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
        /// <param name="source">The current enumerable.</param>
        /// <returns>A ReadOnlySet instance that contains same elements as the specified enumerable.</returns>
        public static ReadOnlySet<T> ToReadOnlySet<T>(this IEnumerable<T> source)
        {
            Check.ObjectIsNotNull(source, "source");

            return (source as ReadOnlySet<T>) ?? new ReadOnlySet<T>(source);
        }


        /// <summary>
        /// Выдает элементы, которые не содержатся в <paramref name="other"/>
        /// </summary>
        /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
        /// <param name="source">The current enumerable.</param>
        /// <param name="other">Enumerable whose elements must be excluded.</param>
        public static IEnumerable<T> Without<T>(this IEnumerable<T> source, IEnumerable<T> other)
        {
            Check.ObjectIsNotNull(source, "source");
            Check.ObjectIsNotNull(other, "other");

            ReadOnlySet<T> otherSet = other.ToReadOnlySet();

            return source.Where(item => !otherSet.Contains(item));
        }


        /// <summary>
        /// Расширенный вариант метода Union. Позволяет добавлять в результирующую коллекцию элементы по одному.
        /// Не меняет базовую коллекцию.
        /// Гарантируется порядок следования элементов: сначала последовательно все из <paramref name="source"/>,
        /// потом <paramref name="newElement"/>, потом последовательно все из <paramref name="newElements"/>
        /// </summary>
        public static IEnumerable<TInput> UnionWith<TInput>(this IEnumerable<TInput> source, TInput newElement, params TInput[] newElements)
        {
            Check.ObjectIsNotNull(source, "source");
            Check.ObjectIsNotNull(newElement, "newElement");
            Check.ObjectIsNotNull(newElements, "newElements");

            foreach (TInput item in source)
            {
                yield return item;
            }

            yield return newElement;

            foreach (TInput item in newElements)
            {
                yield return item;
            }
        }

        /// <summary>
        /// Converts current collection to readonly dictionary.
        /// </summary>
        /// <typeparam name="TSource">The type of the keys in the source collection.</typeparam>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="source">The current collection.</param>
        /// <param name="keySelector">Key converter.</param>
        /// <param name="valueSelector">Value converter.</param>
        /// <returns>A ReadOnlyDictionary that acts as a read-only wrapper around the current dictionary.</returns>
        public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
        {
            Check.ObjectIsNotNull(source, "source");
            Check.ObjectIsNotNull(keySelector, "keySelector");
            Check.ObjectIsNotNull(valueSelector, "valueSelector");

            var result = new Dictionary<TKey, TValue>();

            foreach (TSource currentItem in source)
            {
                TKey key = keySelector(currentItem);
                TValue value = valueSelector(currentItem);

                if (key == null)
                    throw new InvalidOperationException(string.Format("Key cannot be null (Source: {0}).", Convert.ToString(currentItem, CultureInfo.InvariantCulture)));

                if (result.ContainsKey(key))
                    throw new InvalidOperationException(string.Format("An item with the same key ({0}) has already been added (Source: {1}).",
                        Convert.ToString(key, CultureInfo.InvariantCulture), Convert.ToString(currentItem, CultureInfo.InvariantCulture)));

                result.Add(key, value);
            }

            return result.ToReadOnlyDictionary();
        }
    }
}
