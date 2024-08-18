namespace TextureGen;

public static class Mirror
{
    public static Texture MirrorLeftToRight(this Texture texture)
    {
        var data = new byte[texture.ImageSize * texture.ImageSize * Color.Size];
        var dataSpan = data.AsSpan();
        var sourceSpan = texture.Data.Span;

        for (var row = 0; row < texture.ImageSize; ++row)
        {
            var rowSourceSpan = sourceSpan.Slice(row * texture.ImageSize * Color.Size, texture.ImageSize / 2 * Color.Size);
            var rowDestinationSpan = dataSpan.Slice(row * texture.ImageSize * Color.Size, texture.ImageSize * Color.Size);
            var leftRowDestinationSpan = rowDestinationSpan.Slice(0, texture.ImageSize / 2 * Color.Size);
            var rightRowDestinationSpan = rowDestinationSpan.Slice(texture.ImageSize / 2 * Color.Size, texture.ImageSize / 2 * Color.Size);

            rowSourceSpan.CopyTo(leftRowDestinationSpan);

            for (var column = 0; column < texture.ImageSize / 2; ++column)
            {
                rowSourceSpan.Slice((texture.ImageSize / 2 - column - 1) * Color.Size, Color.Size)
                    .CopyTo(rightRowDestinationSpan.Slice(column * Color.Size, Color.Size));
            }
        }

        return new Texture(texture.ImageSize, data);
    }

    public static Texture MirrorRightToLeft(this Texture texture)
    {
        var data = new byte[texture.ImageSize * texture.ImageSize * Color.Size];
        var dataSpan = data.AsSpan();
        var sourceSpan = texture.Data.Span;

        for (var row = 0; row < texture.ImageSize; ++row)
        {
            var rowSourceSpan = sourceSpan.Slice((row * texture.ImageSize + texture.ImageSize / 2) * Color.Size, texture.ImageSize / 2 * Color.Size);
            var rowDestinationSpan = dataSpan.Slice(row * texture.ImageSize * Color.Size, texture.ImageSize * Color.Size);
            var leftRowDestinationSpan = rowDestinationSpan.Slice(0, texture.ImageSize / 2 * Color.Size);
            var rightRowDestinationSpan = rowDestinationSpan.Slice(texture.ImageSize / 2 * Color.Size, texture.ImageSize / 2 * Color.Size);

            rowSourceSpan.CopyTo(rightRowDestinationSpan);

            for (var column = 0; column < texture.ImageSize / 2; ++column)
            {
                rowSourceSpan.Slice((texture.ImageSize / 2 - column - 1) * Color.Size, Color.Size)
                    .CopyTo(leftRowDestinationSpan.Slice(column * Color.Size, Color.Size));
            }
        }

        return new Texture(texture.ImageSize, data);
    }

    public static Texture MirrorTopToBottom(this Texture texture)
    {
        var data = new byte[texture.ImageSize * texture.ImageSize * Color.Size];
        var dataSpan = data.AsSpan();
        var sourceSpan = texture.Data.Span;

        for (var row = 0; row < texture.ImageSize / 2; ++row)
        {
            var rowSource = sourceSpan.Slice(row * texture.ImageSize * Color.Size, texture.ImageSize * Color.Size);
            var rowTopDestination = dataSpan.Slice(row * texture.ImageSize * Color.Size, texture.ImageSize * Color.Size);
            var rowBottomDestination = dataSpan.Slice((texture.ImageSize - row - 1) * texture.ImageSize * Color.Size, texture.ImageSize * Color.Size);

            rowSource.CopyTo(rowTopDestination);
            rowSource.CopyTo(rowBottomDestination);
        }

        return new Texture(texture.ImageSize, data);
    }

    public static Texture MirrorBottomToTop(this Texture texture)
    {
        var data = new byte[texture.ImageSize * texture.ImageSize * Color.Size];
        var dataSpan = data.AsSpan();
        var sourceSpan = texture.Data.Span;

        for (var row = 0; row < texture.ImageSize / 2; ++row)
        {
            var rowSource = sourceSpan.Slice((row + texture.ImageSize / 2) * texture.ImageSize * Color.Size, texture.ImageSize * Color.Size);
            var rowTopDestination = dataSpan.Slice((texture.ImageSize / 2 - row - 1) * texture.ImageSize * Color.Size, texture.ImageSize * Color.Size);
            var rowBottomDestination = dataSpan.Slice((texture.ImageSize / 2 + row) * texture.ImageSize * Color.Size, texture.ImageSize * Color.Size);

            rowSource.CopyTo(rowTopDestination);
            rowSource.CopyTo(rowBottomDestination);
        }

        return new Texture(texture.ImageSize, data);
    }
}