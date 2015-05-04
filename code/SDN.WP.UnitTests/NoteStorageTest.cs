using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using SDN.WP.Storage;

namespace SDN.WP.UnitTests
{
    [TestClass]
    public sealed class NoteStorageTest
    {
        private NoteStorage CreateInstance()
        {
            return new NoteStorage();
        }
    }
}