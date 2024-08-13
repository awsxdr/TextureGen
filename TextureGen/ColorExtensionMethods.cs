namespace TextureGen;

public static class ColorExtensionMethods
{
    public static Memory<byte> ToBytes(this IEnumerable<Color> colors) =>
        new (colors.SelectMany(c => c.ToBytes()).ToArray());
}