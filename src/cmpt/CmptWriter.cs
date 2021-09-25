using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cmpt.Tile
{
    public struct CmptWriter
    {
        public static byte[] Write(IEnumerable<byte[]> tiles)
        {
            if (!tiles.Any())
            {
                throw new ArgumentException("Inner tiles must be defined");
            }
            var stream = new MemoryStream();
            var binaryWriter = new BinaryWriter(stream);

            var header = new CmptHeader();
            header.TilesLength = tiles.Count();

            var paddedTiles = new List<byte[]>();
            foreach (var tile in tiles)
            {
                // optional: we can do other checks here to validate inner tile
                if(tile.Length % 8!= 0)
                {
                    throw new ArgumentException("Inner tile must be 8 byte aligned");

                }
                var tilePadded = tile;
                paddedTiles.Add(tilePadded);
            }

            header.ByteLength = 16 + paddedTiles.Sum(i => i.Length);
            var headerBytes = header.AsBinary();
            var headerBytesPadded = headerBytes;

            binaryWriter.Write(headerBytesPadded);

            foreach (var paddedTile in paddedTiles)
            {
                binaryWriter.Write(paddedTile);
            }

            binaryWriter.Flush();
            binaryWriter.Close();

            return stream.ToArray();
        }
    }
}
