using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using Windows.Phone.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.Storage.Streams;
using JetBrains.Annotations;
using Microsoft.Phone.Reactive;
using SDN.Shared;
using SDN.Shared.Business;
using Buffer = Windows.Storage.Streams.Buffer;

namespace SDN.WP.Storage
{
    public sealed class NoteStorage
    {
        private readonly Func<Action, Task> createOnUiThreadAction;
        private const string dataFolderName = "notes";

        private readonly ObservableCollection<NoteData> actualNotes = CreateCollection();

        private static ObservableCollection<NoteData> CreateCollection()
        {
            var result = new ObservableCollection<NoteData>();

            result.CollectionChanged += (sender, args) => UiCheck.IsUiThread();

            return result;
        }

        public NoteStorage([NotNull] Func<Action, Task> createOnUiThreadAction)
        {
            Check.ObjectIsNotNull(createOnUiThreadAction, "createOnUiThreadAction");

            this.createOnUiThreadAction = createOnUiThreadAction;
        }

        private async Task ReadAllNotesAsync()
        {
            UiCheck.IsBackgroundThread();

            var content = await Task.Run(() => GetAllFiles());

            var notes = new HashSet<NoteData>(content.Select(nd => InnerSerializer.Deserialize(nd)));

            var currentTimeUtc = DateTime.UtcNow;

            var notesToRemove = notes.Where(n => n.RemoveAtUtc < currentTimeUtc).ToList();

            await RemoveNotesAsync(notesToRemove.Select(n => n.Identity).ToArray());

            notesToRemove.ForEach(n => notes.Remove(n));

            await createOnUiThreadAction(() => SetNotes(notes));
        }

        private void SetNotes(HashSet<NoteData> notes)
        {
            UiCheck.IsUiThread();

            var notesToRemove = actualNotes.Except(notes).ToList();

            notesToRemove.ForEach(n => actualNotes.Remove(n));

            foreach (var note in notes)
            {
                var existingNoteIndex = actualNotes.IndexOf(note);

                if (existingNoteIndex >= 0)
                {
                    actualNotes[existingNoteIndex] = note;
                }
                else
                {
                    actualNotes.Add(note);
                }
            }
        }

        private async Task<byte[][]> GetAllFiles()
        {
            UiCheck.IsBackgroundThread();

            var folder = await GetStorageFolderAsync();

            var allFiles = await folder.GetFilesAsync();

            var notesFiles = allFiles.Where(f => f.Name.EndsWith(".note", StringComparison.OrdinalIgnoreCase)).ToArray();

            var getContentTasks = notesFiles.Select(GetFileContent).ToArray();

            Task.WaitAll(getContentTasks);

            UiCheck.IsBackgroundThread();

            return getContentTasks.Select(t => t.Result).ToArray();
        }

        private async Task<StorageFolder> GetStorageFolderAsync()
        {
            UiCheck.IsBackgroundThread();

            var storageFolder = ApplicationData.Current.LocalFolder;

            return await storageFolder.CreateFolderAsync(dataFolderName, CreationCollisionOption.OpenIfExists);
        }

        private async Task<byte[]> GetFileContent(StorageFile file)
        {
            UiCheck.IsBackgroundThread();

            using (var fileStream = await file.OpenStreamForReadAsync())
            {
                var length  = (int) fileStream.Length;
                var buffer = new byte[length];
                await fileStream.ReadAsync(buffer, 0, length);

                return buffer;
            }
        }

        public ReadOnlyObservableCollection<NoteData> ActualNotes
        {
            get
            {
                return new ReadOnlyObservableCollection<NoteData>(actualNotes);
            }
        }

        public async Task RemoveNotesAsync(params Guid[] noteIdentities)
        {
            UiCheck.IsBackgroundThread();

            var folder = await GetStorageFolderAsync();

            var allFiles = await folder.GetFilesAsync();

            var names = new HashSet<string>(noteIdentities.Select(GetNoteFileName));

            var filesToRemove = allFiles.Where(f => names.Contains(f.Name)).ToArray();

            await Task.WhenAll(filesToRemove.Select(f => f.DeleteAsync().AsTask()).ToArray());
        }

        public async Task AddOrUpdateNoteAsync(NoteData note)
        {
            var saveTask = Task.Run(() => SaveNoteAsync(note));
            var updateCollectionTask = createOnUiThreadAction(() => ReAddNote(note));

            await Task.WhenAll(saveTask, updateCollectionTask);
        }

        private void ReAddNote(NoteData note)
        {
            UiCheck.IsUiThread();

            var indexOfExisting = actualNotes.SkipWhile(n => n.Identity != note.Identity).Count() - 1;

            if (indexOfExisting + 1 >= actualNotes.Count)
            {
                actualNotes.Add(note);
            }
            else
            {
                UiCheck.True(actualNotes[indexOfExisting].Identity == note.Identity, "Trying to replace note {0} to note {1}", actualNotes[indexOfExisting].Identity, note.Identity);

                actualNotes[indexOfExisting] = note;
            }
        }

        private async Task SaveNoteAsync(NoteData note)
        {
            UiCheck.IsBackgroundThread();

            var folder = await GetStorageFolderAsync();

            var fileName = GetNoteFileName(note.Identity);

            var targetFile = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

            using (var writeStream = await targetFile.OpenStreamForWriteAsync())
            {
                var data = InnerSerializer.Serialize(note);

                await writeStream.WriteAsync(data, 0, data.Length);
            }
        }

        private static string GetNoteFileName(Guid noteId)
        {
            return string.Format("{0}.note", noteId);
        }

        public async Task UpdateNotes()
        {
            await ReadAllNotesAsync();
        }
    }
}
