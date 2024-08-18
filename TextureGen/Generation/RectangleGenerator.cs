namespace TextureGen.Generation;

public class RectangleGenerator(ImageSize imageSize) : IGenerator<RectangleGenerator.Parameters>
{
    public Texture Generate(Parameters parameters) =>
        parameters.Gradient > 0f
            ? GenerateWithGradient(parameters)
            : GenerateWithoutGradient(parameters);

    private Texture GenerateWithoutGradient(Parameters parameters)
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

    private Texture GenerateWithGradient(Parameters parameters)
    {
        var columnStart = (int)(imageSize * parameters.Left);
        var columnCount = Math.Min((int)(imageSize * parameters.Width), imageSize - columnStart);
        var rowStart = (int)(imageSize * parameters.Top);
        var rowEnd = Math.Min((int)(imageSize * (parameters.Top + parameters.Height)), imageSize);

        var data = new byte[imageSize * imageSize * Color.Size];
        var dataSpan = data.AsSpan();

        var standardRow = new byte[imageSize * Color.Size];
        var standardRowSpan = standardRow.AsSpan();
        var gradientWidth = (int)(parameters.Gradient * columnCount / 2);

        if (gradientWidth > 0)
        {
            var gradientColumnStep = 255 / gradientWidth;

            for (var x = 0; x < gradientWidth; ++x)
            {
                standardRowSpan.Slice((x + columnStart) * Color.Size, Color.Size).Fill((byte)(x * gradientColumnStep));
                standardRowSpan.Slice((columnStart + columnCount - x - 1) * Color.Size, Color.Size)
                    .Fill((byte)(x * gradientColumnStep));
            }
        }

        standardRowSpan.Slice(
            (columnStart + gradientWidth) * Color.Size,
            (columnCount - gradientWidth * 2) * Color.Size)
            .Fill(255);

        var gradientHeight = (int)(parameters.Gradient * (rowEnd - rowStart) / 2);

        if (gradientHeight > 0)
        {
            var gradientRowStep = 255 / gradientHeight;

            for (var y = 0; y < gradientHeight; ++y)
            {
                var topRowSpan = dataSpan.Slice((y + rowStart) * imageSize * Color.Size, imageSize * Color.Size);

                if (gradientWidth > 0)
                {
                    var gradientColumnStep = 255 / gradientWidth;
                for (var x = 0; x < gradientWidth; ++x)
                {
                    var value = (byte)Math.Min(x * gradientColumnStep, y * gradientRowStep);
                    topRowSpan.Slice((x + columnStart) * Color.Size, Color.Size).Fill(value);
                    topRowSpan.Slice((columnStart + columnCount - x - 1) * Color.Size, Color.Size).Fill(value);
                }
                }

                topRowSpan.Slice((columnStart + gradientWidth) * Color.Size,
                        (columnCount - gradientWidth * 2) * Color.Size)
                    .Fill((byte)(gradientRowStep * y));

                topRowSpan.CopyTo(dataSpan.Slice((rowEnd - y - 1) * imageSize * Color.Size, imageSize * Color.Size));
            }
        }

        for (var row = rowStart + gradientHeight; row < rowEnd - gradientHeight; ++row)
        {
            var rowSpan = dataSpan.Slice(row * imageSize * Color.Size, imageSize * Color.Size);
            standardRowSpan.CopyTo(rowSpan);
        }

        return new Texture(imageSize, data);
    }

    public class Parameters
    {
        public float Left { get; init; } = 0.0f;
        public float Top { get; init; } = 0.0f;
        public float Width { get; init; } = 1.0f;
        public float Height { get; init;} = 1.0f;
        public float Gradient { get; init; } = 0.0f;
    }
}