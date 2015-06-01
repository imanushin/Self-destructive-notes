using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using SDN.Shared.Business;
using SDN.WP.Storage;

namespace SDN.WP.UnitTests
{
    [TestClass, Ignore]
    public sealed class NoteStorageTest
    {
        [TestInitialize, STAThread]
        public void StartUp()
        {
            var storageFolder = ApplicationData.Current.LocalFolder;

            var foldersToRemove = storageFolder
                .GetFoldersAsync()
                .GetResults()
                .Where(f => string.Equals("notes", f.Name, StringComparison.OrdinalIgnoreCase))
                .ToArray();

            foldersToRemove.ToList().ForEach(f => f.DeleteAsync().GetResults());
        }

        [TestMethod, STAThread]
        public void AddNote()
        {
            // Given
            var storage = CreateInstance();
            var anotherStorage = CreateInstance();
            var note = NoteData.CreateNew("123");

            // When
            storage.AddOrUpdateNoteAsync(note).Wait();
            anotherStorage.UpdateNotes().Wait();

            // Then
            Assert.AreEqual(1, storage.ActualNotes.Count);
            Assert.AreEqual(1, anotherStorage.ActualNotes.Count);
            Assert.AreEqual(note, anotherStorage.ActualNotes.First());
        }

        private NoteStorage CreateInstance()
        {
            return new NoteStorage(Task.Run);
        }
    }
}