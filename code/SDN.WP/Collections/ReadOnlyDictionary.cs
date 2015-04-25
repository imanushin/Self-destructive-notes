using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Threading;
using Deltacorvi.PortableCommon.Equality;
using SDN.WP;

namespace Deltacorvi.PortableCommon.Collections
{
    /// <summary>
    /// Provides the base class for a generic read-only collection.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    [DataContract(IsReference = true)]
    public sealed class ReadOnlyDictionary<TKey, TValue> : BaseReadOnlyObject, IDictionary<TKey, TValue>, IDictionary
    {
        private const string notSupportedMessage = "Collection is read-only and can not be modified.";

        #region Empty

        private static readonly ReadOnlyDictionary<TKey, TValue> empty = new ReadOnlyDictionary<TKey, TValue>(new Dictionary<TKey, TValue>());

        /// <summary>
        /// Empty collection.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static ReadOnlyDictionary<TKey, TValue> Empty
        {
            get
            {
                return empty;
            }
        }

        #endregion

        [DataMember]
        private readonly Dictionary<TKey, TValue> data;

        private object synchronization;

        /// <summary>
        /// Initializes a new instance of the ReadOnlyDictionary class that is a read-only wrapper around the specified dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary to wrap.</param>
        public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
        {
            Check.ObjectIsNotNull(dictionary, "dictionary");

            data = new Dictionary<TKey, TValue>(dictionary);

            var collection = (ICollection)data;
            synchronization = collection.SyncRoot;
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The element with the specified key.</returns>
        public TValue this[TKey key]
        {
            get
            {
                return data[key];
            }
            set
            {
                Exceptions.Throw(ErrorMessage.NotSupported, notSupportedMessage);
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the dictionary.
        /// </summary>
        public int Count
        {
            get
            {
                return data.Count;
            }
        }

        /// <summary>
        /// Gets a collection containing the keys of the dictionary.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get
            {
                return data.Keys;
            }
        }

        /// <summary>
        /// Gets an collection containing the values in the dictionary.
        /// </summary>
        public ICollection<TValue> Values
        {
            get
            {
                return data.Values;
            }
        }

        /// <summary>
        /// Determines whether the dictionary contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the dictionary.</param>
        /// <returns>Returns true if the dictionary contains an element with the key.</returns>
        public bool ContainsKey(TKey key)
        {
            return data.ContainsKey(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; 
        /// otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns>Returns true if the dictionary contains an element with the specified key.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return data.TryGetValue(key, out value);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        #region Explicit interface implementation

        bool IDictionary.IsFixedSize
        {
            get
            {
                return true;
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get
            {
                return true;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)data).CopyTo(array, index);
        }

        object IDictionary.this[object key]
        {
            get
            {
                return ((IDictionary)data)[key];
            }
            set
            {
                Exceptions.Throw(ErrorMessage.NotSupported, notSupportedMessage);
            }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                return data.Keys;
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                return data.Values;
            }
        }

        bool IDictionary.IsReadOnly
        {
            get
            {
                return true;
            }
        }

        bool IDictionary.Contains(object key)
        {
            bool result = false;

            if (key is TKey)
                result = data.ContainsKey((TKey)key);

            return result;
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IDictionary)data).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)data).Contains(keyValuePair);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)data).CopyTo(array, arrayIndex);
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return true;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                if (synchronization == null)
                    Interlocked.CompareExchange<object>(ref synchronization, new object(), null);
                return synchronization;
            }
        }

        #endregion

        #region Not supported

        [Obsolete("Not supported", true)]
        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            Exceptions.Throw(ErrorMessage.NotSupported, notSupportedMessage);
            return false;
        }

        [Obsolete("Not supported", true)]
        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            Exceptions.Throw(ErrorMessage.NotSupported, notSupportedMessage);
        }

        [Obsolete("Not supported", true)]
        void IDictionary.Clear()
        {
            Exceptions.Throw(ErrorMessage.NotSupported, notSupportedMessage);
        }

        [Obsolete("Not supported", true)]
        void IDictionary.Add(object key, object value)
        {
            Exceptions.Throw(ErrorMessage.NotSupported, notSupportedMessage);
        }

        [Obsolete("Not supported", true)]
        void IDictionary.Remove(object key)
        {
            Exceptions.Throw(ErrorMessage.NotSupported, notSupportedMessage);
        }

        [Obsolete("Not supported", true)]
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
        {
            Exceptions.Throw(ErrorMessage.NotSupported, notSupportedMessage);
            return false;
        }

        [Obsolete("Not supported", true)]
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
        {
            Exceptions.Throw(ErrorMessage.NotSupported, notSupportedMessage);
        }

        [Obsolete("Not supported", true)]
        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            Exceptions.Throw(ErrorMessage.NotSupported, notSupportedMessage);
        }

        #endregion

        #region Equality

        /// <summary>
        /// Бросает исключение, не должна вызываться
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            Exceptions.Throw(ErrorMessage.NotSupported, notSupportedMessage);
            return null;
        }

        /// <summary>
        /// Сравнивает себя и другой объект
        /// </summary>
        protected override bool CheckEquality(BaseReadOnlyObject target)
        {
            return EnumerableComparer.Compare((IDictionary)this, (ReadOnlyDictionary<TKey, TValue>)target);
        }

        /// <summary>
        /// Вычисляет хеш-код без кеширования
        /// </summary>
        protected override int CalculateHashCode()
        {
            return EnumerableHashCode.GetHashCode(this);
        }

        #endregion
    }
}
