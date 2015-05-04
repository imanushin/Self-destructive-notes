using System;
using System.Collections.Generic;
using System.Linq;
using SDN.Shared.Collections;
using SDN.WP.Resources;
using System.Runtime.Serialization;
using System.IO;

namespace SDN.WP.Storage
{
    public sealed class NoteData
    {
        public NoteData(Guid identity, DateTime removeAtUtc, ImmutableList<NoteSnapshot> snapshots, ImmutableDictionary<Guid, byte[]> images)
        {
            Identity = identity;
            RemoveAtUtc = removeAtUtc;
            Snapshots = snapshots;
            Images = images;
        }

        public NoteData(NoteData previous, DateTime newRemovingTime, NoteSnapshot newData, IDictionary<Guid, byte[]> newImages)
        {
            Identity = previous.Identity;
            RemoveAtUtc = newRemovingTime;
            Snapshots = new ImmutableList<NoteSnapshot>(previous.Snapshots.Concat(new[] { newData }).ToArray());

            var images = previous.Images.ToDictionary(kv => kv.Key, kv => kv.Value);

            foreach (var newImage in newImages)
            {
                images[newImage.Key] = newImage.Value;
            }

            Images = new ImmutableDictionary<Guid, byte[]>(images);
        }

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

        public ImmutableList<NoteSnapshot> Snapshots
        {
            get;
            private set;
        }

        public ImmutableDictionary<Guid, byte[]> Images
        {
            get;
            private set;
        }

        public string Serilize()
        {
            var serializer = new DataContractSerializer(typeof(NoteData));

            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, this);

                return Convert.ToBase64String(stream.ToArray());
            }
        }

        public static NoteData Deserialize(string arg)
        {
            var serializer = new DataContractSerializer(typeof(NoteData));

            using (var stream = new MemoryStream(Convert.FromBase64String(arg)))
            {
                return (NoteData)serializer.ReadObject(stream);
            }
        }

        public static NoteData CreateNew()
        {
            var emptySnapshot = new NoteSnapshot(string.Empty, AppResources.DefaultTitle, new ImmutableList<Guid>(new List<Guid>()));

            return new NoteData(
                    Guid.NewGuid(),
                    DateTime.UtcNow.AddDays(1),
                    new ImmutableList<NoteSnapshot>(new[] { emptySnapshot }),
                    new ImmutableDictionary<Guid, byte[]>(new Dictionary<Guid, byte[]>()));
        }
    }
}
