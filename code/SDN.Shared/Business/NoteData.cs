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
    public sealed class NoteData : BaseReadOnlyObject
    {
        private static readonly int guidSize = Guid.NewGuid().ToByteArray().Length;

        public NoteData(Guid identity, DateTime removeAtUtc, ImmutableList<NoteSnapshot> snapshots, ImmutableDictionary<Guid, byte[]> images)
        {
            Check.ObjectIsNotNull(snapshots, "snapshots");

            Identity = identity;
            RemoveAtUtc = removeAtUtc;
            Snapshots = snapshots;
            Images = images;

            CurrentSnapshot = snapshots.LastOrDefault();

            Check.ObjectIsNotNull(CurrentSnapshot);
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

        public NoteSnapshot CurrentSnapshot
        {
            get;
            private set;
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
