﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using JetBrains.Annotations;
using SDN.Shared.Collections;

namespace SDN.Shared.Business
{
    [DataContract]
    public sealed class NoteSnapshot : BaseReadOnlyObject
    {
        private static readonly int guidSize = Guid.NewGuid().ToByteArray().Length;

        public NoteSnapshot(string text, string title, ImmutableList<Guid> images)
        {
            Text = text;
            Title = title;
            Images = images;
        }

        [DataMember]
        public string Text
        {
            get;
            private set;
        }

        [DataMember]
        public string Title
        {
            get;
            private set;
        }

        [DataMember]
        public ImmutableList<Guid> Images
        {
            get;
            private set;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Text;
            yield return Title;
            yield return Images;
        }
    }
}
