using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDN.WP.Storage
{
    internal sealed class NoteData
    {
        public Guid Identity
        {
            get;
            private set;
        }

        public DateTime RemoveAtUtc
        {
            get;
            private set;
        }

        public ReadOnlyCollection<NoteSnapshot> Snapshots
        {
            get;
            private set;
        }

        public ReadOnlyDictionary<Guid, byte[]> Images
        {
            get;
            private set;
        } 
    }
}
