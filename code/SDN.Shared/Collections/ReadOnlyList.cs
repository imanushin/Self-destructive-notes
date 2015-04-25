using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;

namespace SDN.Shared.Collections
{
    /// <summary>
    /// Readonly wrapper for lists.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    [DataContract(IsReference = true)]
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public sealed class ReadOnlyList<T> : BaseReadOnlyObject, IList<T>, IList, IReadOnlyCollection<T>
    {
        private const string notSupportedMessage = "Collection is read-only and can not be modified.";

        #region Empty

        private static readonly ReadOnlyList<T> empty = new ReadOnlyList<T>(new T[0]);

        /// <summary>
        /// Empty collection.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static ReadOnlyList<T> Empty
        {
            get
            {
                return empty;
            }
        }

        #endregion

        [DataMember]
        private readonly List<T> data;

        private object synchronization;

        /// <summary>
        /// Gets the number of elements contained in the list.
        /// </summary>
        public int Count
        {
            get
            {
                return data.Count;
            }
        }

        /// <summary>
        /// Creates an instance of ReadOnlyList class.
        /// </summary>
        /// <param name="collection">Base set that should be wrapped.</param>
        public ReadOnlyList(IEnumerable<T> collection)
        {
            Check.ObjectIsNotNull(collection, "collection");

            data = new List<T>(collection);
        }

        /// <summary>
        /// Determines whether the collection contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the list.</param>
        /// <returns>True if item is found in the System.Collections.Generic.ICollection; otherwise, false.</returns>
        public bool Contains(T item)
        {
            return data.Contains(item);
        }

        /// <summary>
        /// Determines the index of a specific item in the list.
        /// </summary>
        /// <param name="item">The object to locate in the list.</param>
        /// <returns>The index of item if found in the list; otherwise, -1.</returns>
        public int IndexOf(T item)
        {
            return data.IndexOf(item);
        }

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index.</returns>
        public T this[int index]
        {
            get
            {
                return data[index];
            }
            set
            {
                Exceptions.Throw(ErrorMessage.NotSupported, notSupportedMessage);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A System.Collections.Generic.IEnumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        /// <summary>
        /// Copies the elements of the set to an System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from set. 
        /// The System.Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            data.CopyTo(array, arrayIndex);
        }

        #region Not supported

        [Obsolete("Not supported", true)]
        int IList.Add(object value)
        {
            Exceptions.Throw(ErrorMessage.NotSupported, notSupportedMessage);
            return 0;
        }

        [Obsolete("Not supported", true)]
        void IList.Clear()
        {
            Exceptions.Throw(ErrorMessage.NotSupported, notSupportedMessage);
        }

        [Obsolete("Not supported", true)]
        void IList.Insert(int index, object value)
        {
            Exceptions.Throw(ErrorMessage.NotSupported, notSupportedMessage);
        }

        [Obsolete("Not supported", true)]
        void IList.Remove(object value)
        {
            Exceptions.Throw(ErrorMessage.NotSupported, notSupportedMessage);
        }

        [Obsolete("Not supported", true)]
        void IList.RemoveAt(int index)
        {
            Exceptions.Throw(ErrorMessage.NotSupported, notSupportedMessage);
        }

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

        [Obsolete("Not supported", true)]
        void IList<T>.Insert(int index, T item)
        {
            Exceptions.Throw(ErrorMessage.NotSupported, notSupportedMessage);
        }

        [Obsolete("Not supported", true)]
        void IList<T>.RemoveAt(int index)
        {
            Exceptions.Throw(ErrorMessage.NotSupported, notSupportedMessage);
        }

        #endregion

        #region Explicit interface implementation

        IEnumerator IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }

        bool ICollection<T>.IsReadOnly
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

        bool IList.Contains(object value)
        {
            return ((IList)data).Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return ((IList)data).IndexOf(value);
        }

        bool IList.IsFixedSize
        {
            get
            {
                return true;
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return true;
            }
        }

        object IList.this[int index]
        {
            get
            {
                return data[index];
            }
            set
            {
                Exceptions.Throw(ErrorMessage.NotSupported, notSupportedMessage);
            }
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
            return EnumerableComparer.Compare(this, (ReadOnlyList<T>)target);
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
        /// Выдает строку, которая может быть использована для вывода данных в лог или в целях отладки
        /// </summary>
        /// <returns></returns>
        protected override string GetString()
        {
            return string.Format("ReadOnlyList<{0}>: {1}", typeof(T).Name, string.Join(", ", this.Select(item => item.ToString()).ToArray()));
        }
    }
}
