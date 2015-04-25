using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using SDN.WP.Storage;

namespace SDN.WP.UnitTests
{
    [TestClass]
    public sealed class NoteDataTest
    {
        [TestMethod]
        public void SerializeDeserializeTest()
        {
            var noteData = NoteData.CreateNew();

            var serialized = noteData.Serilize();

            var deserialize = NoteData.Deserialize(serialized);

            var nextSerialized = deserialize.Serilize();

            Assert.AreEqual(serialized, nextSerialized);

            Assert.AreEqual(noteData.Identity, deserialize.Identity);
            Assert.AreEqual(noteData.RemoveAtUtc, deserialize.RemoveAtUtc);
            Assert.AreEqual(noteData.Snapshots.Count, deserialize.Snapshots.Count);
            Assert.AreEqual(noteData.Images.Count, deserialize.Images.Count);
        }
    }
}
