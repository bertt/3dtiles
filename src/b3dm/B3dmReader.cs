using System.IO;
using System.Text;
using B3dm.Tile;

namespace Tiles3D.Serialization
{
    public static class B3dmReader
    {
        public static B3dm.Tile.B3dm ReadB3dm(BinaryReader reader)
        {
            var b3dmHeader = new B3dmHeader(reader);
            var featureTableJson = Encoding.UTF8.GetString(reader.ReadBytes(b3dmHeader.FeatureTableJsonByteLength));
            var featureTableBytes = reader.ReadBytes(b3dmHeader.FeatureTableBinaryByteLength);
            var batchTableJson = Encoding.UTF8.GetString(reader.ReadBytes(b3dmHeader.BatchTableJsonByteLength));
            var batchTableBytes = reader.ReadBytes(b3dmHeader.BatchTableBinaryByteLength);

            var glbLength = b3dmHeader.ByteLength - b3dmHeader.Length;
            var glbBuffer = reader.ReadBytes(glbLength);

            var b3dm = new B3dm.Tile.B3dm {
                B3dmHeader = b3dmHeader,
                GlbData = glbBuffer,
                FeatureTableJson = featureTableJson,
                FeatureTableBinary = featureTableBytes,
                BatchTableJson = batchTableJson,
                BatchTableBinary = batchTableBytes
            };
            return b3dm;
        }

        public static B3dm.Tile.B3dm ReadB3dm(Stream stream)
        {
            using (var reader = new BinaryReader(stream)) {
                var b3dm = ReadB3dm(reader);
                return b3dm;
            }
        }
    }
}