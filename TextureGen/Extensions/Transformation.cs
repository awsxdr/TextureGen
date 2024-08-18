namespace TextureGen;

public static class Transformation
{
    public static Texture Translate(this Texture texture, float x, float y)
    {
        var sourceStart = Math.Max(0, (int)Math.Round(texture.ImageSize * -y));
        var sourceEnd = Math.Min(texture.ImageSize, (int)Math.Round(texture.ImageSize * (1f - y)));
        var destinationStart = Math.Max(0, (int)Math.Round(texture.ImageSize * y));

        Console.WriteLine(sourceStart);
        Console.WriteLine(sourceEnd);
        Console.WriteLine(destinationStart);

        var data = new byte[texture.ImageSize * texture.ImageSize * Color.Size];
        var dataSpan = data.AsSpan();
        var sourceSpan = texture.Data.Span;

        for (var row = 0; row < sourceEnd - sourceStart; ++row)
        {
            var rowSourceStart = Math.Max(0, (int)Math.Round(texture.ImageSize * -x));
            var rowSourceEnd = Math.Min(texture.ImageSize, (int)Math.Round(texture.ImageSize * (1f - x)));
            var rowDestinationStart = Math.Max(0, (int)Math.Round(texture.ImageSize * x));
            var rowDestinationEnd = Math.Min(texture.ImageSize, (int)Math.Round(texture.ImageSize * (1f + x)));

            var sourceRowSlice = sourceSpan.Slice(((row + sourceStart) * texture.ImageSize + rowSourceStart) * Color.Size, (rowSourceEnd - rowSourceStart) * Color.Size);
            var destinationRowSlice = dataSpan.Slice(((row + destinationStart) * texture.ImageSize + rowDestinationStart) * Color.Size, (rowDestinationEnd - rowDestinationStart) * Color.Size);

            sourceRowSlice.CopyTo(destinationRowSlice);
        }

        return new Texture(texture.ImageSize, data);
    }
}