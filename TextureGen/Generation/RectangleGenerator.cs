namespace TextureGen.Generation;

public class RectangleGenerator(ImageSize imageSize) : IGenerator<RectangleGenerator.Parameters>
{
    public Texture Generate(Parameters parameters)
    {
        var columnStart = (int)(imageSize * parameters.Left);
        var columnCount = Math.Min((int)(imageSize * parameters.Width), imageSize - columnStart);
        var rowStart = (int)(imageSize * parameters.Top);
        var rowEnd = Math.Min((int)(imageSize * (parameters.Top + parameters.Height)), imageSize);

        var data = new byte[imageSize * imageSize * Color.Size];
        var dataSpan = data.AsSpan();

        for (var row = rowStart; row < rowEnd; ++row)
        {
            var slice = dataSpan.Slice((row * imageSize + columnStart) * Color.Size, columnCount * Color.Size);
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