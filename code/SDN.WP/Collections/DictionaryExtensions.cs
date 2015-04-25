using System.Collections.Generic;
using JetBrains.Annotations;

namespace SDN.WP.Collections
{
    /// <summary>
    /// Provides additional methods for default dictionaries.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Returns a read-only wrapper for the current dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="currentDictionary">The current dictionary.</param>
        /// <returns>A ReadOnlyDictionary that acts as a read-only wrapper around the current dictionary.</returns>
        public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this IDictionary<TKey, TValue> currentDictionary)
        {
            var result = currentDictionary as ReadOnlyDictionary<TKey, TValue>;

            return result ?? new ReadOnlyDictionary<TKey, TValue>(currentDictionary);
        }

        /// <summary>
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="currentDictionary">The current dictionary.</param>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        /// <returns>Returns true if value was added to the dictionary. Returns false if the key is null or an element with 
        /// the same key already exists in the dictionary.</returns>
        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> currentDictionary, TKey key, TValue value)
        {
            bool result = false;

            if ((currentDictionary != null) && (key != null) && !currentDictionary.ContainsKey(key))
            {
                currentDictionary.Add(key, value);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Removes the specified key and value from the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="currentDictionary">The current dictionary.</param>
        /// <param name="key">The key of the element to remove.</param>
        /// <param name="removedValue">When this method returns, contains the value associated with the specified key, if the key is found; 
        /// otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns>Returns true if the element is successfully found and removed.</returns>
        public static bool TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> currentDictionary, TKey key, out TValue removedValue)
        {
            bool result = false;

            if ((currentDictionary != null) && (key != null) && currentDictionary.ContainsKey(key))
            {
                removedValue = currentDictionary[key];
                currentDictionary.Remove(key);
                result = true;
            }
            else
                removedValue = default(TValue);

            return result;
        }

        /// <summary>
        /// Removes the specified key and value from the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="currentDictionary">The current dictionary.</param>
        /// <param name="key">The key of the element to remove.</param>        
        /// <returns>Returns true if the element is successfully found and removed.</returns>
        public static bool TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> currentDictionary, TKey key)
        {
            bool result = false;

            if ((currentDictionary != null) && (key != null) && currentDictionary.ContainsKey(key))
            {
                currentDictionary.Remove(key);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Adds keys and values from the the specified dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="currentDictionary">The current dictionary.</param>
        /// <param name="dictionary">The dictionary whose values should be added to the current dictionary.</param>
        /// <param name="replace">If set to true then values associated with key that is present in both dictionaries 
        /// will be replaced with new ones. Otherwise only new items will be added.</param>
        public static void MergeWith<TKey, TValue>(this IDictionary<TKey, TValue> currentDictionary, IDictionary<TKey, TValue> dictionary, bool replace)
        {
            Check.ObjectIsNotNull(currentDictionary, "currentDictionary");
            Check.ObjectIsNotNull(dictionary, "dictionary");

            foreach (KeyValuePair<TKey, TValue> currentItem in dictionary)
            {
                if (replace)
                    currentDictionary[currentItem.Key] = currentItem.Value;
                else
                    currentDictionary.TryAdd(currentItem.Key, currentItem.Value);
            }
        }

        /// <summary>
        /// Выдает значение по ключу. Если его нет, то оно создается, используя оператор new()
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="currentDictionary">Current dictionary.</param>
        /// <param name="key">The key of the element to get.</param>
        /// <returns>Existing or newly created value.</returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> currentDictionary, TKey key)
            where TValue : new()
        {
            Check.ObjectIsNotNull(currentDictionary, "currentDictionary");
            Check.ObjectIsNotNull(key, "key");

            TValue result;

            bool exists = currentDictionary.TryGetValue(key, out result);

            if (exists)
                return result;

            result = new TValue();

            currentDictionary.Add(key, result);

            return result;
        }

        /// <summary>
        /// Выдает значение по ключу. Если его нет, то возвращается default значение.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="currentDictionary">Current dictionary.</param>
        /// <param name="key">The key of the element to get.</param>
        /// <returns>Existing or default value.</returns>
        [CanBeNull]
        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> currentDictionary, TKey key)
        {
            Check.ObjectIsNotNull(currentDictionary, "currentDictionary");
            Check.ObjectIsNotNull(key, "key");

            TValue result;

            bool exists = currentDictionary.TryGetValue(key, out result);

            return exists ? result : default(TValue);
        }
    }
}
