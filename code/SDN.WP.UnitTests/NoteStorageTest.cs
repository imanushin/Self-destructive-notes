using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using SDN.Shared;
using SDN.Shared.Business;
using SDN.WP.Storage;
using SDN.WP.UnitTests.Helpers;

namespace SDN.WP.UnitTests
{
    [TestClass]
    public sealed class NoteStorageTest
    {
        private IUiCheck uiCheck;
        private UiThreadMock uiThreadMock;

        [TestInitialize, STAThread]
        public void StartUp()
        {
            uiCheck = new TestUiCheck();
            uiThreadMock=new UiThreadMock();
            
            var storageFolder = ApplicationData.Current.LocalFolder;

            var foldersToRemove = storageFolder
                .GetFoldersAsync()
                .GetResults()
                .Where(f => string.Equals("notes", f.Name, StringComparison.OrdinalIgnoreCase))
                .ToArray();

            foldersToRemove.ToList().ForEach(f => f.DeleteAsync().GetResults());
        }

        [TestMethod]
        [Timeout(1000)]
        public void AddNote()
        {
            // Given
            var storage = CreateInstance();
            var anotherStorage = CreateInstance();
            var note = NoteData.CreateNew("123");

            // When
            var addOrUpdateTask = storage.AddOrUpdateNoteAsync(note);
            var updateAnotherTask = addOrUpdateTask.ContinueWith(t => anotherStorage.UpdateNotes());

            uiThreadMock.ExecuteAll();

            updateAnotherTask.Wait();

            // Then
            Assert.AreEqual(1, storage.ActualNotes.Count);
            Assert.AreEqual(1, anotherStorage.ActualNotes.Count);
            Assert.AreEqual(note, anotherStorage.ActualNotes.First());
        }

        private NoteStorage CreateInstance()
        {
            return new NoteStorage(uiThreadMock.Enqueue, uiCheck);
        }
    }
}