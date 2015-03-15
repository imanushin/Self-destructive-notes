using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDN.WP.Storage
{
    internal static class NoteStorage
    {
        public static ReadOnlyCollection<NoteData> ActualNotes
        {
            get;
            private set;
        }

        public static void RemoveNote(Guid noteIdentity)
        {
            throw new NotImplementedException(); 
        }

        public static void AddOrUpdateNote(NoteData note)
        {
            throw new NotImplementedException(); 
        }
    }
}
