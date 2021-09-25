using NUnit.Framework;
using System.IO;
using Pnts.Tile;
using System.Text.Json;

namespace Tests
{
    public class PntsParserTests
    {
        Stream pntsfile;
        string expectedMagicHeader = "pnts";
        int expectedVersionHeader = 1;

        [SetUp]
        public void Setup()
        {
            const string testfile = "fixtures/1-0-1-1.pnts";
            pntsfile = File.OpenRead(testfile);
            Assert.IsTrue(pntsfile != null);
        }

        [Test]
        public void TestPointsFile()
        {
            // source file: https://github.com/CesiumGS/3d-tiles-samples/blob/master/tilesets/TilesetWithRequestVolume/points.pnts
            var pntsFile =  File.OpenRead(@"fixtures/points.pnts");
            var pnts = PntsSerializer.Deserialize(pntsFile);
            Assert.IsTrue(expectedMagicHeader == pnts.Magic);
            Assert.IsTrue(expectedVersionHeader == pnts.Version);
            Assert.IsTrue(pnts.Points.Count == 125000);
            Assert.IsTrue(pnts.Colors.Count == 125000);
            Assert.IsTrue(pnts.Points[0].X == (float)-1.1413337);
            Assert.IsTrue(pnts.Points[0].Y == (float)0.359452039);
            Assert.IsTrue(pnts.Points[0].Z == (float)-0.361457467);
            Assert.IsTrue(pnts.Colors[0].R == 182);
            Assert.IsTrue(pnts.Colors[0].G == 215);
            Assert.IsTrue(pnts.Colors[0].B == 153);
        }

        [Test]
        public void ParsePntsTest()
        {
            // arrange

            // act
            var pnts = PntsSerializer.Deserialize(pntsfile);

            // assert
            Assert.IsTrue(expectedMagicHeader == pnts.Magic);
            Assert.IsTrue(expectedVersionHeader == pnts.Version);
            Assert.IsTrue(pnts.Points != null);
            Assert.IsTrue(pnts.Points.Count > 0);
            Assert.IsTrue(pnts.Points[0].X == (float)144.78);
            Assert.IsTrue(pnts.Points[0].Y == (float)-64.85);
            Assert.IsTrue(pnts.Points[0].Z == (float)-174.68);
            Assert.IsTrue(pnts.Colors != null);
            Assert.IsTrue(pnts.Colors.Count > 0);
            Assert.IsTrue(pnts.Colors[0].R == 75);
            Assert.IsTrue(pnts.Colors[0].G == 91);
            Assert.IsTrue(pnts.Colors[0].B == 88);
            Assert.IsTrue(pnts.FeatureTableMetadata.points_length == 164);
            var rtc = pnts.FeatureTableMetadata.Rtc_Center;
            Assert.IsTrue(rtc[0] == 3830004.5);
            Assert.IsTrue(rtc[1] == 323597.5);
            Assert.IsTrue(rtc[2] == 5072948.5);
        }

        [Test]
        public void FeatureTableDeserialize()
        {
            var featureTableJson = "{\"POINTS_LENGTH\":125000,\"POSITION\":{\"byteOffset\":0},\"RGB\":{\"byteOffset\":1500000}}       ";
            var featureTableMetadata = JsonSerializer.Deserialize<FeatureTable>(featureTableJson);
            Assert.IsTrue(featureTableMetadata.points_length == 125000);
        }
    }
}