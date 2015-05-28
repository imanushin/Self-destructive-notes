using System;
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
    public sealed class NoteSnapshotSerialized
    {
        [DataMember]
        public string Text;

        [DataMember]
        public string Title;

        [DataMember]
        public Guid[] Images;

        public NoteSnapshot Convert()
        {
            return new NoteSnapshot(Text, Title, Images.ToImmutableList());
        }

        public static NoteSnapshotSerialized Convert(NoteSnapshot input)
        {
            return new NoteSnapshotSerialized()
            {
                Images = input.Images.ToArray(),
                Title = input.Title,
                Text = input.Text
            };
        }
    }
}
