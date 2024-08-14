namespace TextureGen;

using System.Runtime.InteropServices;

public static class ColorExtensionMethods
{
    public static byte[] ToBytes(this IEnumerable<Color> colors) =>
        MemoryMarshal.AsBytes(colors.ToArray().AsSpan()).ToArray();
}