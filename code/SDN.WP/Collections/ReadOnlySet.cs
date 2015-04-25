using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using Deltacorvi.PortableCommon.Equality;
using SDN.WP;

namespace Deltacorvi.PortableCommon.Collections
{
    /// <summary>
    /// Readonly wrapper for sets.
    /// </summary>
    /// <typeparam name="T">The type of elements in the set.</typeparam>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    [DataContract(IsReference = true)]
    [DebuggerDisplay("Count = {Count}")]
    public sealed class ReadOnlySet<T> : BaseReadOnlyObject, ICollection<T>, IReadOnlyCollection<T>
    {
        private const string notSupportedMessage = "Collection is read-only and can not be modified.";

        #region Empty

        private static readonly ReadOnlySet<T> empty = new ReadOnlySet<T>(new T[0]);

        /// <summary>
        /// Пустая коллекция. Для разных <typeparamref name="T"/> выдаются разные объекты, для одинаковых <typeparamref name="T"/> всегда выдается один и тот же объект 
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static ReadOnlySet<T> Empty
        {
            get
            {
                return empty;
            }
        }

        #endregion

        [DataMember]
        private readonly List<T> originalData;

        private Dictionary<T, object> innerSet;

        private Dictionary<T, object> Data
        {
            get
            {
                innerSet = innerSet ?? (innerSet = new Dictionary<T, object>());

                return innerSet;
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the set.
        /// </summary>
        public int Count
        {
            get
            {
                return originalData.Count;
            }
        }

        /// <summary>
        /// Creates an instance of ReadOnlySet class.
        /// </summary>
        /// <param name="collection">Base set that should be wrapped.</param>
        public ReadOnlySet(IEnumerable<T> collection)
        {
            Check.ObjectIsNotNull(collection, "collection");

            innerSet = new Dictionary<T, object>();

            foreach (T newItem in collection)
            {
                innerSet.TryAdd(newItem, null);
            }

            originalData = innerSet.Keys.ToList();
        }

        /// <summary>
        /// Determines whether the System.Collections.Generic.ICollection contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the set.</param>
        /// <returns>True if item is found in the System.Collections.Generic.ICollection; otherwise, false.</returns>
        public bool Contains(T item)
        {
            return Data.ContainsKey(item);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A System.Collections.Generic.IEnumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return originalData.GetEnumerator();
        }

        /// <summary>
        /// Copies the elements of the set to an System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from set. 
        /// The System.Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            originalData.CopyTo(array, arrayIndex);
        }

        #region Not supported

        [Obsolete("Not supported", true)]
        void ICollection<T>.Add(T item)
        {
            Exceptions.Throw(ErrorMessage.NotSupported, notSupportedMessage);
        }

        [Obsolete("Not supported", true)]
        void ICollection<T>.Clear()
        {
            Exceptions.Throw(ErrorMessage.NotSupported, notSupportedMessage);
        }

        [Obsolete("Not supported", true)]
        bool ICollection<T>.Remove(T item)
        {
            Exceptions.Throw(ErrorMessage.NotSupported, notSupportedMessage);
            return false;
        }

        #endregion

        #region Explicit interface implementation

        IEnumerator IEnumerable.GetEnumerator()
        {
            return originalData.GetEnumerator();
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Equality

        /// <summary>
        /// Бросает исключение, не должна вызываться
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            Exceptions.Throw(ErrorMessage.NotSupported);
            return null;
        }

        /// <summary>
        /// Сравнивает себя и другой объект
        /// </summary>
        protected override bool CheckEquality(BaseReadOnlyObject target)
        {
            return EnumerableComparer.Compare(this, (ReadOnlySet<T>)target);
        }

        /// <summary>
        /// Вычисляет хеш-код без кеширования
        /// </summary>
        protected override int CalculateHashCode()
        {
            return EnumerableHashCode.GetHashCode(this);
        }

        #endregion

        /// <summary>
        /// Для метода ToString. Объект сам не занимается кешированием
        /// </summary>
        protected override string GetString()
        {
            if (Count == 0)
                return string.Format("ReadOnlySet<{0}>, 0 elements", typeof(T).Name);

            List<T> items = this.ToList();

            items.Sort((left, right) => string.CompareOrdinal(left.ToString(), right.ToString()));

            string result = string.Format("ReadOnlySet<{0}>, {1} elements:\r\n  {2}", typeof(T).Name, Count, string.Join("\r\n  ", items.Select(item => item.ToString()).ToArray()));

            return result.Replace("\r\n", "\r\n  ");
        }
    }
}
