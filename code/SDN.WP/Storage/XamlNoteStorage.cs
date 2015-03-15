using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDN.WP.Storage
{
    public class XamlNoteStorage
    {
        public ReadOnlyObservableCollection<NoteData> ActualNotes
        {
            get
            {
                return NoteStorage.ActualNotes;
            }
        }
    }
}
