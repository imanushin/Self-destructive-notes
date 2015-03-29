using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Phone.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;
using Microsoft.Phone.Reactive;
using Buffer = Windows.Storage.Streams.Buffer;

namespace SDN.WP.Storage
{
    public static class NoteStorage
    {
        private static readonly object syncRoot = new object();

        private static readonly ObservableCollection<NoteData> actualNotes = new ObservableCollection<NoteData>();

        private const string dataFolderName = "notes";

        private async static Task<StorageFolder> GetOrCreateNotesFolder()
        {
            var root = ApplicationData.Current.LocalFolder;

            var result = await root.CreateFolderAsync(dataFolderName, CreationCollisionOption.OpenIfExists);

            return result;
        }

        private static void ReadAllNotes()
        {
            var content = GetAllFiles();

            var notes = new HashSet<NoteData>(content.Select(NoteData.Deserialize));

            var currentTimeUtc = DateTime.UtcNow;

            var notesToRemove = notes.Where(n => n.RemoveAtUtc < currentTimeUtc).ToList();

            Task.Run(() => notesToRemove.ForEach(n => RemoveNote(n.Identity)));
            notesToRemove.ForEach(n => notes.Remove(n));
            
            Deployment.Current.Dispatcher.BeginInvoke(() => SetNotes(notes));
        }

        private static void SetNotes(HashSet<NoteData> notes)
        {
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

        private static string[] GetAllFiles()
        {
            lock (syncRoot)
            {
                using (var isolatedFile = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!isolatedFile.DirectoryExists(dataFolderName))
                    {
                        isolatedFile.CreateDirectory(dataFolderName);
                    }

                    var fileNames = isolatedFile.GetFileNames(dataFolderName + "/*.note");

                    var files = new List<string>();

                    foreach (var fileName in fileNames)
                    {
                        using (var file = isolatedFile.OpenFile(fileName, FileMode.Open))
                        {
                            using (var reader = new StreamReader(file))
                            {
                                files.Add(reader.ReadToEnd());
                            }
                        }
                    }

                    return files.ToArray();
                }
            }
        }

        public static ReadOnlyObservableCollection<NoteData> ActualNotes
        {
            get
            {
                return new ReadOnlyObservableCollection<NoteData>(actualNotes);
            }
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
                        using (var writer = new StreamWriter(stream))
                        {
                            writer.Write(serializedNote);
                        }
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

        public static void UpdateNotes()
        {
            Task.Run(() => ReadAllNotes());
        }
    }
}
