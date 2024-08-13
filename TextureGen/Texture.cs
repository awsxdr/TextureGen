namespace TextureGen;

public class Texture
{
    public ReadOnlyMemory<byte> Data { get; }

    public ImageSize ImageSize { get; }

    public Texture(ImageSize size)
    {
        var dataLength = (int)size * (int)size * 4;

        Data = new Memory<byte>(Enumerable.Repeat((byte)0, dataLength).ToArray());
        ImageSize = size;
    }

    public Texture(ImageSize size, Memory<byte> data)
    {
        var expectedDataLength = (int)size * (int)size * 4;

        if (data.Length != expectedDataLength) throw new InvalidDataLengthException();

        var internalData = new Memory<byte>(new byte[data.Length]);
        data.CopyTo(internalData);
        Data = internalData;
        ImageSize = size;
    }

    public Texture(ImageSize size, Func<int, int, Color> colorFactory)
    {
        var sizeAsInt = (int)size;
        var colors = Enumerable.Range(0, sizeAsInt * sizeAsInt).Select(i => colorFactory(i % sizeAsInt, i / sizeAsInt));

        Data = colors.ToBytes();
        ImageSize = size;
    }

    public static Texture operator *(Texture texture1, Texture texture2)
    {
        if (texture1.ImageSize != texture2.ImageSize) throw new TextureSizesDoNotMatchException();

        var newData = texture1.ToByteArray().Zip(texture2.ToByteArray(), (b1, b2) => (byte)((b1 / 255f) * (b2 / 255f) * 255f)).ToArray();

        return new(texture1.ImageSize, newData);
    }

    public byte[] ToByteArray() => Data.ToArray();

    public Color ColorAt(int x, int y) =>
        Color.FromBytes(Data.Slice((x % (int)ImageSize) * 4 + (y % (int)ImageSize) * (int)ImageSize * 4, 4));

    public class InvalidDataLengthException : ArgumentException;
    public class TextureSizesDoNotMatchException : ArgumentException;
}