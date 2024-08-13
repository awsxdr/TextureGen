namespace TextureGen;

public static class Compression
{
    public static Texture CompressRange(this Texture texture, Percentage minimum, Percentage maximum)
    {
        var colorMap = Color.ArrayFromBytes(texture.Data);

        byte CompressByte(byte value) =>
            (byte)((value / 255f * (maximum - minimum) + minimum) * 255);

        return new Texture(
            texture.ImageSize,
            colorMap.Select(c => Color.FromArgb(c.Alpha, CompressByte(c.Red), CompressByte(c.Green), CompressByte(c.Blue))).ToBytes());
    }
}