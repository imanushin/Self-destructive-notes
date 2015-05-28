using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using JetBrains.Annotations;
using SDN.Shared.Collections;

namespace SDN.Shared.Business
{
    [DataContract(IsReference = true)]
    public sealed class NoteDataSerialized
    {
        public NoteDataSerialized()
        {
        }

        [DataMember]
        public Guid Identity;

        [DataMember]
        public DateTime RemoveAtUtc;

        [DataMember]
        public NoteSnapshotSerialized[] Snapshots;

        [DataMember]
        public Dictionary<Guid, string> Images;

        public NoteData Convert()
        {
            var images = Images.ToDictionary(kv => kv.Key, kv => Base64Helper.FromBase64(kv.Value)).ToReadOnlyDictionary();

            return new NoteData(Identity, RemoveAtUtc, Snapshots.Select(s => s.Convert()).ToImmutableList(), images);
        }

        public static NoteDataSerialized Convert(NoteData input)
        {
            return new NoteDataSerialized()
            {
                Identity = input.Identity,
                RemoveAtUtc = input.RemoveAtUtc,
                Snapshots = input.Snapshots.Select(NoteSnapshotSerialized.Convert).ToArray(),
                Images = input.Images.ToDictionary(kv => kv.Key, kv => Base64Helper.ToBase64(kv.Value))
            };
        }
    }
}
