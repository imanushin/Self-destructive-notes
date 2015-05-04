using SDN.Shared.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDN.WP.Storage
{
    public sealed class NoteSnapshot
    {
        public NoteSnapshot(string text, string title, ImmutableList<Guid> images)
        {
            Text = text;
            Title = title;
            Images = images;
        }

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

        public ImmutableList<Guid> Images
        {
            get;
            private set;
        }
    }
}
