namespace TextureGen;

public class Color
{
    public byte Alpha { get; init; }
    public byte Red { get; init; }
    public byte Green { get; init; }
    public byte Blue { get; init; }

    public static Color FromBytes(ReadOnlyMemory<byte> bytes)
    {
        if (bytes.Length != 4) throw new InvalidSpanLengthException();

        return FromBytesUnchecked(bytes);
    }

    public static Color[] ArrayFromBytes(ReadOnlyMemory<byte> bytes)
    {
        if (bytes.Length % 4 != 0) throw new InvalidSpanLengthException();

        return Enumerable.Range(0, bytes.Length / 4)
            .Select(i => bytes.Slice(i * 4, 4))
            .Select(FromBytesUnchecked)
            .ToArray();
    }

    public static Color FromArgb(byte alpha, byte red, byte green, byte blue) => FromBytes(new Memory<byte>([blue, green, red, alpha]));

    public static Color Gray(byte value) => FromArgb(255, value, value, value);

    public static Color operator *(Color color, float value) => 
        FromArgb(
            color.Alpha,
            (byte)MathF.Min(255f, color.Red * value),
            (byte)MathF.Min(255f, color.Green * value),
            (byte)MathF.Min(255f, color.Blue * value));

    public static Color operator +(Color color, Color value) =>
        FromArgb(
            (byte)((color.Alpha + value.Alpha) / 2f), 
            (byte)Math.Min(255, color.Red + value.Red), 
            (byte)Math.Min(255, color.Green + value.Green),
            (byte)Math.Min(255, color.Blue + value.Blue));

    public byte[] ToBytes() => [Blue, Green, Red, Alpha];

    private static Color FromBytesUnchecked(ReadOnlyMemory<byte> bytes)
    {
        var span = bytes.Span;
        return new Color
        {
            Alpha = span[3],
            Red = span[2],
            Green = span[1],
            Blue = span[0],
        };
    }

    public class InvalidSpanLengthException : ArgumentException;
}