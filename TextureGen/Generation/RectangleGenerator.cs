namespace TextureGen.Generation;

public class RectangleGenerator(ImageSize imageSize) : IGenerator<RectangleGenerator.Parameters>
{
    public Texture Generate(Parameters parameters)
    {
        var intImageSize = (int)imageSize;

        var columnStart = (int)(intImageSize * parameters.Left);
        var columnCount = Math.Min((int)(intImageSize * parameters.Width), intImageSize - columnStart);
        var rowStart = (int)(intImageSize * parameters.Top);
        var rowEnd = Math.Min((int)(intImageSize * (parameters.Top + parameters.Height)), intImageSize);

        var data = new byte[intImageSize * intImageSize * Color.Size];
        var dataSpan = data.AsSpan();

        for (var row = rowStart; row < rowEnd; ++row)
        {
            var slice = dataSpan.Slice((row * intImageSize + columnStart) * Color.Size, columnCount * Color.Size);
            slice.Fill(255);
        }

        return new Texture(imageSize, data);
    }

    public class Parameters
    {
        public Percentage Left { get; init; } = 0.0f;
        public Percentage Top { get; init; } = 0.0f;
        public Percentage Width { get; init; } = 1.0f;
        public Percentage Height { get; init;} = 1.0f;
    }
}