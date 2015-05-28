using NUnit.Framework;
using SDN.Shared.Business;

namespace SDN.Shared.UnitTests
{
    [TestFixture]
    public sealed class InnerSerializerTest
    {
        [Test]
        public void SerializeDeserializeTest()
        {
            var noteData = NoteData.CreateNew("123");

            var serialized = InnerSerializer.Serialize(noteData);

            var deserialized = InnerSerializer.Deserialize(serialized);

            Assert.AreEqual(noteData.Identity, deserialized.Identity);
            Assert.AreEqual(noteData.RemoveAtUtc, deserialized.RemoveAtUtc);
            Assert.AreEqual(noteData.Snapshots, deserialized.Snapshots);
            Assert.AreEqual(noteData.Images, deserialized.Images);
            Assert.AreEqual(noteData, deserialized);
        }
    }
}
