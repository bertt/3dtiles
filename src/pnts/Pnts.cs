using System.Collections.Generic;
using System.Drawing;

namespace Pnts.Tile
{
    public class Pnts
    {
        public string Magic { get; set; }
        public int Version { get; set; }
        public List<Point> Points { get; set; }
        public List<Color> Colors { get; set; }
        public FeatureTable FeatureTableMetadata { get; set; }
    }
}
