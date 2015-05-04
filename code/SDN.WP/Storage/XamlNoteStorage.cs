using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SDN.Shared.Business;

namespace SDN.WP.Storage
{
    public class XamlNoteStorage
    {
        public ReadOnlyObservableCollection<NoteData> ActualNotes
        {
            get
            {
                return App.NoteStorage.ActualNotes;
            }
        }
    }
}
