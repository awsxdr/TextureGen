namespace TextureGen;

using System.Numerics;
using Func;

public delegate Color ColorFactoryMethod(int x, int y);

public abstract class TextureBase
{
    public ReadOnlyMemory<byte> Data { get; }

    public ImageSize ImageSize { get; }

    protected TextureBase(ImageSize size, Memory<byte> data)
    {
        var expectedDataLength = (int)size * (int)size * Color.Size;

        if (data.Length != expectedDataLength) throw new InvalidDataLengthException();

        var internalData = new Memory<byte>(new byte[data.Length]);
        data.CopyTo(internalData);
        Data = internalData;
        ImageSize = size;
    }

    protected TextureBase(ImageSize size, ColorFactoryMethod colorFactory)
    {
        var sizeAsInt = (int)size;
        var colors = Enumerable.Range(0, sizeAsInt * sizeAsInt).Select(i => colorFactory(i % sizeAsInt, i / sizeAsInt));

        Data = colors.ToBytes();
        ImageSize = size;
    }

    public class InvalidDataLengthException : ArgumentException;
    public class TextureSizesDoNotMatchException : ArgumentException;

    public byte[] ToByteArray() => Data.ToArray();

    public Color ColorAt(int x, int y) =>
        Color.FromBytes(Data.Slice((x % (int)ImageSize) * Color.Size + (y % (int)ImageSize) * (int)ImageSize * Color.Size, Color.Size));
}

public class Texture : TextureBase
{
    public Texture(ImageSize size, Memory<byte> data) : base(size, data) { }
    public Texture(ImageSize size, ColorFactoryMethod colorFactory) : base(size, colorFactory) { }

    public static Texture operator *(Texture texture1, Texture texture2)
    {
        if (texture1.ImageSize != texture2.ImageSize) throw new TextureSizesDoNotMatchException();

        var newData = texture1.ToByteArray().Zip(texture2.ToByteArray(), (b1, b2) => (byte)((b1 / 255f) * (b2 / 255f) * 255f)).ToArray();

        return new(texture1.ImageSize, newData);
    }
}

public class NormalTexture : TextureBase
{
    public NormalTexture(ImageSize size, Memory<byte> data) : base(size, data) { }
    public NormalTexture(ImageSize size, ColorFactoryMethod colorFactory) : base(size, colorFactory) { }

    public NormalTexture(ImageSize size, IEnumerable<Vector3> normals) : base(size, NormalsToColors(size, normals))
    {
    }

    private static ColorFactoryMethod NormalsToColors(ImageSize size, IEnumerable<Vector3> normals)
    {
        normals = normals.ToArray();

        return (x, y) => GetNormal(x, y).Map(NormalToColor);

        Vector3 GetNormal(int x, int y) => normals.ElementAt(x + y * (int)size);

        byte VectorValueToByte(float value) => (byte)(255 * (0.5f + value / 2f));

        Color NormalToColor(Vector3 normal) =>
            Color.FromRgb(VectorValueToByte(normal.X), VectorValueToByte(normal.Y),
                VectorValueToByte(-normal.Z));
    }
}