using NUnit.Framework;
using SDN.Shared.Business;

namespace SDN.Shared.UnitTests
{
    [TestFixture]
    public sealed class NoteDataTest
    {
        [Test]
        public void SerializeDeserializeTest()
        {
            var noteData = NoteData.CreateNew("123");

            var serialized = noteData.Serilize();

            var deserialize = NoteData.Deserialize(serialized);

            var nextSerialized = deserialize.Serilize();

            Assert.AreEqual(serialized, nextSerialized);

            Assert.AreEqual(noteData.Identity, deserialize.Identity);
            Assert.AreEqual(noteData.RemoveAtUtc, deserialize.RemoveAtUtc);
            Assert.AreEqual(noteData.Snapshots, deserialize.Snapshots);
            Assert.AreEqual(noteData.Images, deserialize.Images);
        }
    }
}
