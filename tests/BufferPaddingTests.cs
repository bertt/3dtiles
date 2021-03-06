using NUnit.Framework;
using Tile3D.Utils;

namespace i3dm.tile.tests
{
    public class BufferPaddingTests
    {
        [Test]
        public void Initial()
        {

            var featureTableJson = "{\"INSTANCES_LENGTH\":2,\"POSITION\":{\"byteOffset\":0},\"EAST_NORTH_UP\":false,\"RTC_CENTER\":[10,10,10]}";
            var paddedJson = BufferPadding.AddPadding(featureTableJson);
            Assert.IsTrue(paddedJson == "{\"INSTANCES_LENGTH\":2,\"POSITION\":{\"byteOffset\":0},\"EAST_NORTH_UP\":false,\"RTC_CENTER\":[10,10,10]}");
        }
    }
}
