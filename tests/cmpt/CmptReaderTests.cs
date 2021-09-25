using I3dm.Tile;
using NUnit.Framework;
using System.IO;
using System.Linq;
using Tiles3D.Serialization;

namespace Cmpt.Tile.Tests
{
    public class CmptReaderTests
    {
        string expectedMagicHeader = "cmpt";
        int expectedVersionHeader = 1;

        [Test]
        public void ReadCmptest()
        {
            var cmptfile = File.OpenRead(@"fixtures/0_0_0_0.cmpt");
            Assert.IsTrue(cmptfile != null);
            var cmpt = CmptReader.Read(cmptfile);
            Assert.IsTrue(cmpt.Tiles.Count() == 5);
        }

        [Test]
        public void ReadTrafficCompositeTest()
        {
            var cmptfile = File.OpenRead(@"fixtures/tile_0_14.cmpt");
            Assert.IsTrue(cmptfile != null);
            var cmpt = CmptReader.Read(cmptfile);
            Assert.IsTrue(cmpt.Tiles.Count() == 2);
        }

        [Test]
        public void ReadFirstCompositeTest()
        {
            // arrange
            // source: https://github.com/CesiumGS/cesium/tree/master/Specs/Data/Cesium3DTiles/Composite/Composite
            var cmptfile = File.OpenRead(@"fixtures/composite.cmpt");
            Assert.IsTrue(cmptfile != null);

            // act. This cmpt contains a batched model and an instanced model
            var cmpt = CmptReader.Read(cmptfile);

            // assert
            Assert.IsTrue(cmpt != null);
            Assert.IsTrue(cmpt.CmptHeader.Magic == expectedMagicHeader);
            Assert.IsTrue(cmpt.CmptHeader.Version == expectedVersionHeader);
            Assert.IsTrue(cmpt.CmptHeader.ByteLength == 13472); // The length of the entire Composite tile, including this header and each inner tile, in bytes.
            Assert.IsTrue(cmpt.CmptHeader.TilesLength == 2);
            Assert.IsTrue(cmpt.Tiles.Count() == 2);
            Assert.IsTrue(cmpt.Magics.ToArray()[0] == "b3dm");
            Assert.IsTrue(cmpt.Magics.ToArray()[1] == "i3dm");

            var b3dm = B3dmReader.ReadB3dm(new MemoryStream(cmpt.Tiles.ToArray()[0]));
            Assert.IsTrue(b3dm.FeatureTableJson == "{\"BATCH_LENGTH\":10,\"RTC_CENTER\":[1215012.8988049095,-4736313.0423059845,4081604.3368623317]}");
            Assert.IsTrue(b3dm.GlbData != null);


            var i3dm = I3dmReader.Read(new MemoryStream(cmpt.Tiles.ToArray()[1]));
            Assert.IsTrue(i3dm.Positions.Count == 25);
        }

        [Test]
        public void ReadCompositeOfInstancedTest()
        {
            // source: https://github.com/CesiumGS/cesium/blob/master/Specs/Data/Cesium3DTiles/Composite/CompositeOfInstanced/
            var cmptfile = File.OpenRead(@"fixtures/compositeOfInstanced.cmpt");
            Assert.IsTrue(cmptfile != null);
            var cmpt = CmptReader.Read(cmptfile);
            Assert.IsTrue(cmpt != null);
            Assert.IsTrue(cmpt.CmptHeader.Magic == expectedMagicHeader);
            Assert.IsTrue(cmpt.CmptHeader.Version == expectedVersionHeader);
            Assert.IsTrue(cmpt.CmptHeader.TilesLength== 2);

            var i3dm = I3dmReader.Read(new MemoryStream(cmpt.Tiles.First()));
            Assert.IsTrue(i3dm.Positions.Count == 25);
        }
    }
}
