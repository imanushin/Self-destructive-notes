using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NUnit.Framework;
using SDN.Shared.Collections;

namespace SDN.Shared.UnitTests
{
    [TestFixture]
    public sealed class ImmutableDictionaryTest
    {
        private ImmutableDictionary<int, string> target;
        private IDictionary targetDictionary;
        private IDictionary<int, string> targetGenericDictionary;
        private Dictionary<int, string> expected;

        private const int key = 1;
        private const string value = "123";

        [SetUp]
        public void Initialize()
        {
            expected = new Dictionary<int, string>();

            expected.Add(key, value);

            target = new ImmutableDictionary<int, string>(expected);
            targetDictionary = target;
            targetGenericDictionary = target;
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorWrongArguments()
        {
            new ImmutableDictionary<int, string>(null);
        }

        [Test]
        public void This()
        {
           Assert.AreEqual(expected[key], target[key]);

            expected[key] = "321";

            Assert.AreEqual(value, target[key]);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void This_NotSupported()
        {
            target[key] = value;
        }

        [Test]
        public void IsSynchronized()
        {
           Assert.IsTrue(((ICollection)target).IsSynchronized);
        }

        [Test]
        public void SyncRoot()
        {
            Assert.IsNotNull(((ICollection)target).SyncRoot);
        }

        [Test]
        public void Count()
        {
           Assert.AreEqual(expected.Count, target.Count);
        }

        [Test]
        public void IsFixedSize()
        {
            Assert.IsTrue(((IDictionary)target).IsFixedSize);
        }

        [Test]
        public void IsReadOnly()
        {
            Assert.IsTrue(((ICollection<KeyValuePair<int, string>>)target).IsReadOnly);
        }

        #region Keys and Values

        [Test]
        public void Keys_CommonChecks()
        {
            ICollection<int> keys = target.Keys;

           Assert.IsNotNull(keys);
           Assert.IsTrue(keys.IsReadOnly);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void Keys_AddFail()
        {
            ICollection<int> keys = target.Keys;

            keys.Add(5);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void Keys_RemoveFail()
        {
            ICollection<int> keys = target.Keys;

            keys.Remove(key);
        }

        [Test]
        public void Values_CommonChecks()
        {
            ICollection<string> values = target.Values;

           Assert.IsNotNull(values);
           Assert.IsTrue(values.IsReadOnly);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void Values_AddFail()
        {
            ICollection<string> values = target.Values;

            values.Add("5");
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void Values_RemoveFail()
        {
            ICollection<string> values = target.Values;

            values.Remove(value);
        }

        #endregion

        [Test]
        public void ContainsKey()
        {
           Assert.AreEqual(expected.ContainsKey(key), target.ContainsKey(key));
           Assert.AreEqual(expected.ContainsKey(key + 1), target.ContainsKey(key + 1));
        }

        [Test]
        public void TryGetValue()
        {
            string expectedOutValue;
            string actualOutValue;

           Assert.AreEqual(expected.TryGetValue(key, out expectedOutValue), target.TryGetValue(key, out actualOutValue));
           Assert.AreEqual(expectedOutValue, actualOutValue);

           Assert.AreEqual(expected.TryGetValue(key + 1, out expectedOutValue), target.TryGetValue(key + 1, out actualOutValue));
           Assert.AreEqual(expectedOutValue, actualOutValue);
        }

        [Test]
        public void GetEnumerator()
        {
           Assert.IsNotNull(target.GetEnumerator());
        }

        [Test]
        public void CopyTo()
        {
            var objects = new object[expected.Count + 1];

            targetDictionary.CopyTo(objects, 1);

           Assert.IsNotNull(objects[1]);
           Assert.IsNull(objects[0]);
        }

        [Test]
        public void IDictionary_Keys()
        {
           Assert.IsNotNull(targetDictionary.Keys);
        }

        [Test]
        public void IDictionary_Values()
        {
           Assert.IsNotNull(targetDictionary.Values);
        }

        [Test]
        public void IDictionary_GetEnumerator()
        {
           Assert.IsNotNull(targetDictionary.GetEnumerator());
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Add()
        {
            targetGenericDictionary.Add(3, "654");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Remove()
        {
            targetGenericDictionary.Remove(3);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Clear()
        {
            targetGenericDictionary.Clear();
        }

    }
}
