using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Pnts.Tile
{
    public static class PntsSerializer
    {
        public static Pnts Deserialize(Stream stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                // spec: https://github.com/AnalyticalGraphicsInc/3d-tiles/tree/master/specification/TileFormats/PointCloud
                // feature table spec: https://github.com/AnalyticalGraphicsInc/3d-tiles/blob/master/specification/TileFormats/FeatureTable/README.md

                // first 4 bytes must be 'pnts' otherwise its not a pnts file
                var magic = Encoding.UTF8.GetString(reader.ReadBytes(4));
                var version = reader.ReadUInt32();
                var tileByteLength = reader.ReadUInt32();  
                var featureTableJsonByteLength = reader.ReadUInt32(); 
                var featureTableBinByteLength = reader.ReadUInt32();
                var batchTableJsonByteLength = reader.ReadUInt32();
                var batchTableBinByteLength = reader.ReadUInt32();

                var featureTableJsonBytes = reader.ReadBytes((int)featureTableJsonByteLength);
                var featureTableJson = Encoding.UTF8.GetString(featureTableJsonBytes); // "{\"POINTS_LENGTH\":164,\"POSITION\":{\"byteOffset\":0},\"RGB\":{\"byteOffset\":1968},\"RTC_CENTER\":[3830004.5,323597.5,5072948.5]}\n"
                var featureTableMetadata = JsonSerializer.Deserialize<FeatureTable>(featureTableJson);

                var featureTableBinBytes = reader.ReadBytes((int)featureTableBinByteLength);

                var featureTableStream = new MemoryStream(featureTableBinBytes);
                var binaryReader = new BinaryReader(featureTableStream);


                var points = new List<Point>();
                for (var  i = 0; i < featureTableMetadata.points_length; i++){
                    var x = binaryReader.ReadSingle(); 
                    var y = binaryReader.ReadSingle();
                    var z = binaryReader.ReadSingle();

                    var p = new Point { X = x, Y = y, Z = z };
                    points.Add(p);
                }

                var colors = new List<Color>();
                for (var i = 0; i < featureTableMetadata.points_length; i++)
                {
                    var r = (int)binaryReader.ReadByte(); 
                    var g = (int)binaryReader.ReadByte(); 
                    var b = (int)binaryReader.ReadByte();

                    var c = Color.FromArgb(r, g, b);
                    colors.Add(c);
                }

                var pnts = new Pnts() { Magic = magic, Version = (int)version, Points = points, Colors = colors, FeatureTableMetadata = featureTableMetadata };
                return pnts;
            }
        }
    }
}
