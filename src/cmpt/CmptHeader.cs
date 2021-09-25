using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Cmpt.Tile
{
    public class CmptHeader
    {
        public CmptHeader()
        {
            Magic = "cmpt";
            Version = 1;
        }

        public CmptHeader(BinaryReader reader)
        {
            Magic = Encoding.UTF8.GetString(reader.ReadBytes(4));
            Version = (int)reader.ReadUInt32();
            ByteLength = (int)reader.ReadUInt32();
            TilesLength = (int)reader.ReadUInt32();
        }

        public string Magic { get; set; }
        public int Version { get; set; }
        public int ByteLength { get; set; }
        public int TilesLength { get; set; }

        public byte[] AsBinary()
        {
            var magicBytes = Encoding.UTF8.GetBytes(Magic);
            var versionBytes = BitConverter.GetBytes(Version);
            var byteLengthBytes = BitConverter.GetBytes(ByteLength);
            var TileLengthBytes = BitConverter.GetBytes(TilesLength);

            return magicBytes.
                Concat(versionBytes).
                Concat(byteLengthBytes).
                Concat(TileLengthBytes).
                ToArray();
        }
    }
}
