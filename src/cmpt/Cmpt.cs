using System.Collections.Generic;

namespace Cmpt.Tile
{
    public class Cmpt
    {
        public CmptHeader CmptHeader { get; set; }

        public IEnumerable<byte[]> Tiles { get; set; }

        public IEnumerable<string> Magics { get; set; }
    }
}
