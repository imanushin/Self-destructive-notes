using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SDN.WP.Resources;

namespace SDN.WP.Storage
{
    public sealed class NoteData
    {
        public NoteData(Guid identity, DateTime removeAtUtc, ReadOnlyCollection<NoteSnapshot> snapshots, ReadOnlyDictionary<Guid, byte[]> images)
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
            Snapshots = new ReadOnlyCollection<NoteSnapshot>(previous.Snapshots.Concat(new[] { newData }).ToArray());

            var images = previous.Images.ToDictionary(kv => kv.Key, kv => kv.Value);

            foreach (var newImage in newImages)
            {
                images[newImage.Key] = newImage.Value;
            }

            Images = new ReadOnlyDictionary<Guid, byte[]>(images);
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
            var emptySnapshot = new NoteSnapshot(string.Empty, AppResources.DefaultTitle, new ReadOnlyCollection<Guid>(new List<Guid>()));

            return new NoteData(
                    Guid.NewGuid(),
                    DateTime.UtcNow.AddDays(1),
                    new ReadOnlyCollection<NoteSnapshot>(new[] { emptySnapshot }),
                    new ReadOnlyDictionary<Guid, byte[]>(new Dictionary<Guid, byte[]>()));
        }
    }
}
