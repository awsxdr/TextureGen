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
        Color.FromBytes(Data.Slice((x % ImageSize) * Color.Size + (y % ImageSize) * ImageSize * Color.Size, Color.Size));
}

public class Texture : TextureBase
{
    public Texture(ImageSize size, Memory<byte> data) : base(size, data) { }
    public Texture(ImageSize size, ColorFactoryMethod colorFactory) : base(size, colorFactory) { }

    public static Texture operator *(Texture texture1, Texture texture2)
    {
        if (texture1.ImageSize != texture2.ImageSize) throw new TextureSizesDoNotMatchException();

        var texture1Data = texture1.Data.Span;
        var texture2Data = texture2.Data.Span;
        var resultData = new byte[texture1Data.Length];
        var resultSpan = resultData.AsSpan();

        for (var i = 0; i < texture1Data.Length; ++i)
        {
            resultSpan[i] = (byte)((((texture1Data[i] + 1) * texture2Data[i]) & 0xFF00) >> 8);
        }

        return new(texture1.ImageSize, resultData);
    }

    public static Texture operator ~(Texture texture)
    {
        var textureData = texture.Data.Span;
        var resultData = new byte[textureData.Length];
        var resultSpan = resultData.AsSpan();

        for (var i = 0; i < textureData.Length; ++i)
        {
            resultSpan[i] = (byte)~textureData[i];

        }

        return new(texture.ImageSize, resultData);
    }
}

public class NormalTexture : TextureBase
{
    public NormalTexture(ImageSize size, Memory<byte> data) : base(size, data) { }
    public NormalTexture(ImageSize size, ColorFactoryMethod colorFactory) : base(size, colorFactory) { }

    public NormalTexture(ImageSize size, IEnumerable<Vector3> normals) : base(size, GetNormalsData(size, normals))
    {
    }

    private static byte[] GetNormalsData(ImageSize size, IEnumerable<Vector3> normals)
    {
        var data = new byte[size * size * Color.Size];
        var dataSpan = data.AsSpan();
        using var normalEnumerator = normals.GetEnumerator();

        for (var i = 0; i < data.Length; i += Color.Size)
        {
            normalEnumerator.MoveNext();

            var normal = normalEnumerator.Current;
            dataSpan[i + 2] = VectorValueToByte(normal.X);
            dataSpan[i + 1] = VectorValueToByte(normal.Y);
            dataSpan[i] = VectorValueToByte(-normal.Z);
        }

        return data;

        byte VectorValueToByte(float value) => (byte)(255 * (0.5f + value / 2f));
    }
}