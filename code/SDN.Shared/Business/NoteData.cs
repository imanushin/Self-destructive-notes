using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using SDN.Shared.Collections;

namespace SDN.Shared.Business
{
    [DataContract(IsReference = true)]
    public sealed class NoteData : BaseReadOnlyObject
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

        [DataMember]
        public Guid Identity
        {
            get;
            private set;
        }

        [DataMember]
        public DateTime RemoveAtUtc
        {
            get;
            private set;
        }

        [DataMember]
        public ImmutableList<NoteSnapshot> Snapshots
        {
            get;
            private set;
        }

        [DataMember]
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

        public static NoteData CreateNew(string defaultTitle)
        {
            var emptySnapshot = new NoteSnapshot(string.Empty, defaultTitle, new ImmutableList<Guid>(new List<Guid>()));

            return new NoteData(
                    Guid.NewGuid(),
                    DateTime.UtcNow.AddDays(1),
                    new ImmutableList<NoteSnapshot>(new[] { emptySnapshot }),
                    new ImmutableDictionary<Guid, byte[]>(new Dictionary<Guid, byte[]>()));
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Identity;
            yield return RemoveAtUtc;
            yield return Snapshots;
            yield return Images;
        }
    }
}
