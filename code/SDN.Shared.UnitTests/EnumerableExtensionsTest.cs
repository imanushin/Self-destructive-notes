using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using SDN.Shared.Collections;

namespace Deltacorvi.NetCommon.Tests.Collections
{
    [TestFixture]
    public sealed class EnumerableExtensionsTest
    {
        [Test]
        public void ToReadOnlyList()
        {
            var input = new[] { 1, 2, 3 };

            var output = input.ToReadOnlyList();

            CollectionAssert.AreEqual(output, input);
        }

        [Test]
        public void ForEach_Action()
        {
            var input = new[] { "1", null };

            var output = new List<string>();

            input.ForEach(output.Add);

            CollectionAssert.AreEquivalent(input, output);
        }

        [Test]
        public void UnionWith()
        {
            var original = Enumerable.Range(0, 4);
            const int newElement = 4;
            var otherElements = Enumerable.Range(5, 9);

            var result = original.UnionWith(newElement, otherElements.ToArray()).ToReadOnlyList();

            Assert.AreEqual(Enumerable.Range(0, 14).ToReadOnlyList(), result);

        }
    }
}
