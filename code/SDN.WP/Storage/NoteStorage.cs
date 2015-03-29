using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Buffer = Windows.Storage.Streams.Buffer;

namespace SDN.WP.Storage
{
    public static class NoteStorage
    {
        private static readonly object syncRoot = new object();

        private const string dataFolderName = "notes";

        private async static Task<StorageFolder> GetOrCreateNotesFolder()
        {
            var root = ApplicationData.Current.LocalFolder;

            var result = await root.CreateFolderAsync(dataFolderName, CreationCollisionOption.OpenIfExists);

            return result;
        }

        public static ReadOnlyObservableCollection<NoteData> ActualNotes
        {
            get;
            private set;
        }

        public static void RemoveNote(Guid noteIdentity)
        {
            lock (syncRoot)
            {
                using (var isolatedFile = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    var fileName = GetNoteFileName(noteIdentity);

                    if (!isolatedFile.FileExists(fileName))
                    {
                        return;
                    }

                    isolatedFile.DeleteFile(fileName);
                }
            }
        }

        public static void AddOrUpdateNote(NoteData note)
        {
            lock (syncRoot)
            {
                using (var isolatedFile = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!isolatedFile.DirectoryExists(dataFolderName))
                    {
                        isolatedFile.CreateDirectory(dataFolderName);
                    }

                    var fileName = GetNoteFileName(note);

                    var serializedNote = note.Serilize();

                    using (var stream = isolatedFile.OpenFile(fileName, FileMode.Create))
                    {
                        stream.Write(serializedNote, 0, serializedNote.Length);
                    }
                }
            }
        }

        private static string GetNoteFileName(NoteData note)
        {
            return GetNoteFileName(note.Identity);
        }

        private static string GetNoteFileName(Guid noteId)
        {
            return string.Format("{0}/{1}.note", dataFolderName, noteId);
        }


    }
}
