namespace TextureGen;

public sealed class ImageSize
{
    private readonly int _value;

    private ImageSize(int value) => _value = value;

    public static implicit operator int(ImageSize size) => size._value;
    public static implicit operator ImageSize(int size) => new(size);

    public static readonly ImageSize Size16 = 16;
    public static readonly ImageSize Size32 = 32;
    public static readonly ImageSize Size64 = 64;
    public static readonly ImageSize Size128 = 128;
    public static readonly ImageSize Size256 = 256;
    public static readonly ImageSize Size512 = 512;
    public static readonly ImageSize Size1024 = 1024;
    public static readonly ImageSize Size2048 = 2048;
    public static readonly ImageSize Size4096 = 4096;
    public static readonly ImageSize Size8192 = 8192;
}