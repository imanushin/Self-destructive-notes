using System;
using SDN.Shared.Collections;

namespace SDN.Shared.Business
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
