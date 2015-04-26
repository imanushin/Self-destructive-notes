using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Deltacorvi.NetCommon;
using NUnit.Framework;
using SDN.Shared.Collections;

namespace Deltacorvi.NetCommon.Tests.Collections
{
// ReSharper disable InvokeAsExtensionMethod
    [TestFixture]
    public sealed class DictionaryExtensionsTest
    {
        private const int key1 = 5;
        private const int key2 = 12;
        private const string value1 = "234";
        private const string value2 = "543";

        [Test]
        public void AsReadOnly()
        {
            var dictionary = new Dictionary<int, string>();

            var result = DictionaryExtensions.ToReadOnlyDictionary(dictionary);

            Assert.IsNotNull(result);

            var theSame = result.ToReadOnlyDictionary();

            Assert.AreSame(result,theSame);
        }

        [Test]
        public void TryAdd()
        {
            var dictionary = new Dictionary<int, string>();

            bool result = DictionaryExtensions.TryAdd(dictionary, key1, value1);
            Assert.IsTrue(result);

            result = DictionaryExtensions.TryAdd(dictionary, key1, value1);
            Assert.IsFalse(result);
        }

        [Test]
        public void TryRemove()
        {
            var dictionary = new Dictionary<int, string>();

            bool result = DictionaryExtensions.TryRemove(dictionary, key1);
           Assert.IsFalse(result);

            dictionary.Add(key1, value1);

            result = DictionaryExtensions.TryRemove(dictionary, key1);
           Assert.IsTrue(result);

            result = DictionaryExtensions.TryRemove(dictionary, key1);
           Assert.IsFalse(result);
        }

        [Test]
        public void TryRemove_Simple()
        {
            var dictionary = new Dictionary<int, string>();

            bool result = DictionaryExtensions.TryRemove(dictionary, key1);
           Assert.IsFalse(result);

            dictionary.Add(key1, value1);

            result = DictionaryExtensions.TryRemove(dictionary, key1);
           Assert.IsTrue(result);

            result = DictionaryExtensions.TryRemove(dictionary, key1);
           Assert.IsFalse(result);
        }

        [Test]
        public void TryRemove_Out()
        {
            var dictionary = new Dictionary<int, string>();

            bool result = DictionaryExtensions.TryRemove(dictionary, key1);
           Assert.IsFalse(result);

            dictionary.Add(key1, value1);

            string outValue;

            result = DictionaryExtensions.TryRemove(dictionary, key1, out outValue);
           Assert.IsTrue(result);
           Assert.AreEqual(value1, outValue);

            result = DictionaryExtensions.TryRemove(dictionary, key1, out outValue);
           Assert.IsFalse(result);
           Assert.IsNull(outValue);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MergeWith_FirstIsNull()
        {
            DictionaryExtensions.MergeWith(null, new Dictionary<int, string>(), false);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MergeWith_SecondIsNull()
        {
            DictionaryExtensions.MergeWith(new Dictionary<int, string>(), null, false);
        }

        [Test]
        public void MergeWith_OnlyAdd()
        {
            var first = new Dictionary<int, string>();
            var second = new Dictionary<int, string>();

            first.Add(key1, value1);
            second.Add(key1, value2);
            second.Add(key2, value2);

            /*int changedCount =*/ DictionaryExtensions.MergeWith(first, second, false);

           //Assert.AreEqual(1, changedCount);
           Assert.AreEqual(2, first.Count);
           Assert.AreEqual(value1, first[key1]);
        }

        [Test]
        public void MergeWith_AddOrReplace()
        {
            var first = new Dictionary<int, string>();
            var second = new Dictionary<int, string>();

            first.Add(key1, value1);
            second.Add(key1, value2);
            second.Add(key2, value2);

            /*int changedCount =*/ DictionaryExtensions.MergeWith(first, second, true);

           //Assert.AreEqual(2, changedCount);
           Assert.AreEqual(2, first.Count);
           Assert.AreEqual(value2, first[key1]);
        }

        [Test]
        public void GetOrAdd()
        {
            var target = new Dictionary<string, int>();

            string key = "key";

            int added = target.GetOrAdd(key);

            Assert.AreEqual(0, added);

            target[key] = 1;

            int getted = target.GetOrAdd(key);

            Assert.AreEqual(1, getted);
        }

        [Test]
        public void GetOrNull()
        {
            var target = new Dictionary<string, string>();

            string key = "key";
            string value = "value";

            string firstResult = target.TryGetValue(key);

            Assert.IsNull(firstResult);

            target[key] = value;

            string secondResult = target.TryGetValue(key);

            Assert.AreEqual(value, secondResult);
        }
    }
    // ReSharper restore InvokeAsExtensionMethod

}
