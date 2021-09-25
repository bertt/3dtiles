using I3dm.Tile;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Cmpt.Tile.Tests
{
    public class CmptWriterTests
    {
        [Test]
        public void TestArgumentExceptionWhenNoInnertiles()
        {
            // arrange
            var tiles = new List<byte[]>();

            // assert, should throw argumentexception
            Assert.Throws<ArgumentException>(() => CmptWriter.Write(tiles));
        }


        [Test]
        public void TestArgumentExceptionWhenNot8byteAligned()
        {
            // arrange
            var treeUrlGlb = "https://mymodels/tree.glb";
            var pos1 = new Vector3(100, 101, 102);
            var positions = new List<Vector3>() { pos1 };
            var i3dm = new I3dm.Tile.I3dm(positions, treeUrlGlb);
            var tiles = new List<byte[]>();
            var tileBytes = I3dmWriter.Write(i3dm);
            
            // act
            // make i3dm tile not 8 byte aligned
            var wrongTile = tileBytes.SkipLast(1).ToArray();
            tiles.Add(wrongTile);

            // assert, should throw argumentexception
            Assert.Throws<ArgumentException>(() => CmptWriter.Write(tiles));
        }

        [Test]
        public void FirstCmptWriterTest()
        {
            // arrange
            var treeUrlGlb = "https://bertt.github.io/mapbox_3dtiles_samples/samples/instanced/trees_external_gltf/tree.glb";
            var pos1 = new Vector3(100, 101, 102);
            var pos2 = new Vector3(200, 201, 202);
            var positions = new List<Vector3>() { pos1, pos2 };

            var i3dm = new I3dm.Tile.I3dm(positions, treeUrlGlb);
            i3dm.RtcCenter = new Vector3(100, 100, 100);

            var tileBytes= I3dmWriter.Write(i3dm);
            var tiles = new List<byte[]>();
            tiles.Add(tileBytes);

            // act
            var cmptBytes = CmptWriter.Write(tiles);

            // assert
            Assert.IsTrue(cmptBytes.Length > 0);
        }

        [Test]
        public void MultipleInnertilesCmptWriterTest()
        {
            // arrange
            var treeUrlGlb = "https://bertt.github.io/mapbox_3dtiles_samples/samples/instanced/trees_external_gltf/tree.glb";
            var pos1 = new Vector3(100, 101, 102);
            var pos2 = new Vector3(200, 201, 202);
            var positions = new List<Vector3>() { pos1, pos2 };

            var i3dm = new I3dm.Tile.I3dm(positions, treeUrlGlb);
            i3dm.RtcCenter = new Vector3(100, 100, 100);

            var i3dm1 = new I3dm.Tile.I3dm(positions, treeUrlGlb);
            i3dm1.RtcCenter = new Vector3(200, 200, 200);

            var i3dmBytes = I3dmWriter.Write(i3dm);

            File.WriteAllBytes(@"d:\aaa\i3dmvalid.i3dm", i3dmBytes);

            var i3dm1Bytes = I3dmWriter.Write(i3dm1);

            var tiles = new List<byte[]>();
            tiles.Add(i3dmBytes);
            tiles.Add(i3dm1Bytes);

            // act
            var cmptBytes = CmptWriter.Write(tiles);

            // assert
            Assert.IsTrue(cmptBytes.Length > 0);

            var ms = new MemoryStream(cmptBytes);
            var cmpt = CmptReader.Read(ms);
            Assert.IsTrue(cmpt.Tiles.Count() == 2);
        }

        [Test]
        public void WriteFromInnerTiles()
        {
            // arrange
            var tile0 = File.ReadAllBytes(@"fixtures/inner/tile0.i3dm");
            var tile1 = File.ReadAllBytes(@"fixtures/inner/tile1.i3dm");

            var tiles = new List<byte[]>();
            tiles.Add(tile0);
            tiles.Add(tile1);
            
            // act
            var cmptBytes = CmptWriter.Write(tiles);

            // assert
            var cmpt = CmptReader.Read(new MemoryStream(cmptBytes));
            Assert.IsTrue(cmpt.Tiles.Count()==2);
        }
    }
}
