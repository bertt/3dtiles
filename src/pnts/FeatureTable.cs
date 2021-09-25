using System.Text.Json.Serialization;

namespace Pnts.Tile
{
    public class FeatureTable
    {
        // todo: POSITION_QUANTIZED, RGBA, RGB565, NORMAL, NORMAL_OCT16P, BATCH_ID, QUANTIZED_VOLUME_OFFSET, QUANTIZED_VOLUME_SCALE, CONSTANT_RGBA,BATCH_LENGTH

        [JsonPropertyName("POINTS_LENGTH")]
        public int points_length { get; set; }
        public Position Position { get; set; }
        public Rgb Rgb { get; set; }

        [JsonPropertyName("RTC_CENTER")]
        public float[] Rtc_Center { get; set; }
    }
}
