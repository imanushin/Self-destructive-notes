using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDN.WP.Storage
{
    internal sealed class NoteSnapshot
    {
        public string Text
        {
            get;
            private set;
        }

        public string Title
        {
            get;
            private set;
        }

        public ReadOnlyCollection<Guid> Images
        {
            get;
            private set;
        }
    }
}
