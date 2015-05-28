using System.IO;
using System.Runtime.Serialization;
using SDN.Shared.Business;

namespace SDN.Shared
{
    public sealed class InnerSerializer
    {
        private static readonly DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(NoteDataSerialized));

        public static byte[] Serialize(NoteData note)
        {
            using (var stream = new MemoryStream())
            {
                var obj = NoteDataSerialized.Convert(note);

                dataContractSerializer.WriteObject(stream, obj);

                return stream.ToArray();
            }
        }

        public static NoteData Deserialize(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                var obj = (NoteDataSerialized)dataContractSerializer.ReadObject(stream);

                return obj.Convert();
            }
        }
    }
}
