using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using SDN.Shared.Collections;

namespace SDN.Shared
{
    /// <summary>
    /// Base immutable object. Contains operators and methods for comparison and other basic operations.
    /// </summary>
    [DataContract(IsReference = true)]
    public abstract class BaseReadOnlyObject : IComparable<BaseReadOnlyObject>, IComparable, IEquatable<BaseReadOnlyObject>
    {
        private int? hashCode;

        private string description;

        private ReadOnlyList<object> innerObjects;

        /// <summary>
        /// Returns true if objects are equal.
        /// </summary>
        public static bool operator ==(BaseReadOnlyObject left, BaseReadOnlyObject right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Returns true if objects are not equal.
        /// </summary>
        public static bool operator !=(BaseReadOnlyObject left, BaseReadOnlyObject right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// Левый больше правого
        /// </summary>
        public static bool operator >(BaseReadOnlyObject left, BaseReadOnlyObject right)
        {
            return Compare(left, right) > 0;
        }

        /// <summary>
        /// Левый меньше правого
        /// </summary>
        public static bool operator <(BaseReadOnlyObject left, BaseReadOnlyObject right)
        {
            return Compare(left, right) < 0;
        }

        /// <summary>
        /// Левый не меньше правого
        /// </summary>
        public static bool operator >=(BaseReadOnlyObject left, BaseReadOnlyObject right)
        {
            return Compare(left, right) >= 0;
        }

        /// <summary>
        /// Левый не больше правого
        /// </summary>
        public static bool operator <=(BaseReadOnlyObject left, BaseReadOnlyObject right)
        {
            return Compare(left, right) <= 0;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public sealed override string ToString()
        {
            return description ?? (description = GetString() ?? base.ToString());
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified System.Object is equal to the current System.Object; otherwise, false.</returns>
        public sealed override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return CheckEquality((BaseReadOnlyObject)obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        // ReSharper disable NonReadonlyFieldInGetHashCode
        public sealed override int GetHashCode()
        {
            if (!hashCode.HasValue)
                hashCode = GetType().GetHashCode() ^ CalculateHashCode();
            return hashCode.Value;
        }

        // ReSharper restore NonReadonlyFieldInGetHashCode
        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns>true if the specified System.Object is equal to the current System.Object; otherwise, false.</returns>
        public bool Equals(BaseReadOnlyObject other)
        {
            return Equals(other as object);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current 
        /// instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning 
        /// Less than zero This instance precedes obj in the sort order. Zero This instance occurs in the same position
        /// in the sort order as obj. Greater than zero This instance follows obj in the sort order.
        /// </returns>
        public int CompareTo(BaseReadOnlyObject other)
        {
            return Compare(this, other);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current 
        /// instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning 
        /// Less than zero This instance precedes obj in the sort order. Zero This instance occurs in the same position
        /// in the sort order as obj. Greater than zero This instance follows obj in the sort order.
        /// </returns>
        public int CompareTo(object obj)
        {
            return Compare(this, obj);
        }

        /// <summary>
        /// Returns all object that should be involved in hash code calculation and equalirty checks.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        protected abstract IEnumerable<object> GetInnerObjects();

        /// <summary>
        /// Вычисляет значение хеш-кода без кеширования.
        /// Использует функцию <see cref="GetInnerObjects"/>
        /// </summary>
        protected virtual int CalculateHashCode()
        {
            if (innerObjects == null)
                innerObjects =  GetInnerObjects().ToReadOnlyList();

            return innerObjects.Aggregate(0, (current, innerObject) => current ^ GetHashCodeOfMember(innerObject));
        }

        private static int GetHashCodeOfMember(object innerObject)
        {
            return innerObject == null ? 0 : innerObject.GetHashCode();
        }

        /// <summary>
        /// Сравнивает себя в объектом того же типа.
        /// Использует функцию <see cref="GetInnerObjects"/>
        /// </summary>
        protected virtual bool CheckEquality(BaseReadOnlyObject target)
        {
            if (innerObjects == null)
                innerObjects = GetInnerObjects().ToReadOnlyList();

            var other = target;

            if (other.innerObjects == null)
                other.innerObjects = other.GetInnerObjects().ToReadOnlyList();

            if (innerObjects.Count != other.innerObjects.Count)
                return false;

            for (int i = 0; i < innerObjects.Count; i++)
            {
                object first = innerObjects[i];
                object second = other.innerObjects[i];

                if (ReferenceEquals(first, second))
                    continue;

                if (ReferenceEquals(null, first) || ReferenceEquals(null, second))
                    return false;

                if (!innerObjects[i].Equals(other.innerObjects[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current 
        /// instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="target">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning 
        /// Less than zero This instance precedes obj in the sort order. Zero This instance occurs in the same position
        /// in the sort order as obj. Greater than zero This instance follows obj in the sort order.
        /// </returns>
        protected virtual int CustomCompare(object target)
        {
            return DefaultCompare(target);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        protected virtual string GetString()
        {
            return null;
        }

        private static int Compare(BaseReadOnlyObject left, object right)
        {
            if (ReferenceEquals(right, left))
                return 0;

            if (ReferenceEquals(right, null))
                return 1;

            if (ReferenceEquals(left, null))
                return -1;

            return (left.GetType() != right.GetType()) ? left.DefaultCompare(right) : left.CustomCompare(right);
        }

        private int DefaultCompare(object target)
        {
            return string.CompareOrdinal(ToString(), target.ToString());
        }
    }
}
