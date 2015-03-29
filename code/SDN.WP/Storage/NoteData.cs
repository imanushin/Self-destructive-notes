﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SDN.WP.Storage
{
    public sealed class NoteData
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

        public byte[] Serilize()
        {
            var serializer = new DataContractSerializer(typeof (NoteData));

            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, this);

                return stream.ToArray();
            }
        }
    }
}
