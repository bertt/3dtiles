using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cmpt.Tile
{
    public struct CmptReader
    {
        public static Cmpt Read(Stream stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                var cmpt = new Cmpt();
                var cmptHeader = new CmptHeader(reader);
                cmpt.CmptHeader = cmptHeader;
                var tiles = new List<byte[]>();
                var magics = new List<string>();

                for (var i=0;i< cmptHeader.TilesLength; i++)
                {
                    var currentPosition = reader.BaseStream.Position;
                    var magic = Encoding.UTF8.GetString(reader.ReadBytes(4));
                    magics.Add(magic);
                    var version = (int)reader.ReadUInt32();
                    var byteLength = (int)reader.ReadUInt32();

                    reader.BaseStream.Position = currentPosition;
                    var bytesInnertile = reader.ReadBytes(byteLength);
                    tiles.Add(bytesInnertile);
                }

                cmpt.Tiles = tiles;
                cmpt.Magics = magics;


                return cmpt;
            }
        }
    }
}