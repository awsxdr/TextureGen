namespace TextureGen;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Size = 3)]
public readonly struct Color
{
    [field: FieldOffset(2)] public byte Red { get; init; }
    [field: FieldOffset(1)] public byte Green { get; init; }
    [field: FieldOffset(0)] public byte Blue { get; init; }

    public const int Size = 3;

    public static Color FromBytes(ReadOnlyMemory<byte> bytes)
    {
        if (bytes.Length != 3) throw new InvalidSpanLengthException();

        return FromBytesUnchecked(bytes);
    }

    public static Color[] ArrayFromBytes(ReadOnlyMemory<byte> bytes)
    {
        if (bytes.Length % 3 != 0) throw new InvalidSpanLengthException();

        return MemoryMarshal.Cast<byte, Color>(bytes.Span).ToArray();
    }

    public static Color FromRgb(byte red, byte green, byte blue) => FromBytesUnchecked(new Memory<byte>([blue, green, red]));

    public static Color Gray(byte value) => FromRgb(value, value, value);

    public static Color operator *(Color color, float value) => 
        FromRgb(
            (byte)MathF.Min(255f, color.Red * value),
            (byte)MathF.Min(255f, color.Green * value),
            (byte)MathF.Min(255f, color.Blue * value));

    public static Color operator +(Color color, Color value) =>
        FromRgb(
            (byte)Math.Min(255, color.Red + value.Red), 
            (byte)Math.Min(255, color.Green + value.Green),
            (byte)Math.Min(255, color.Blue + value.Blue));

    public byte[] ToBytes() => [Blue, Green, Red];

    private static Color FromBytesUnchecked(ReadOnlyMemory<byte> bytes) =>
        MemoryMarshal.AsRef<Color>(bytes.Span);

    public class InvalidSpanLengthException : ArgumentException;
}