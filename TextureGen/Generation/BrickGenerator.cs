namespace TextureGen.Generation;

public class BrickGenerator(ImageSize imageSize) : IGenerator<BrickGenerator.Parameters>
{
    public Texture Generate(Parameters parameters)
    {
        float GetRandomBrick() =>
            parameters.MaximumHeight - Random.Shared.NextSingle() *
            MathF.Max(0f, parameters.MaximumHeight - parameters.MinimumHeight) * parameters.HeightVariance;

        var bricks = 
            Enumerable.Range(0, parameters.Columns)
                .Select(_ => 
                    Enumerable.Range(0, parameters.Rows)
                        .Select(_ => GetRandomBrick()).ToArray())
                .ToArray();

        var intImageSize = (int)imageSize;

        var rowHeight = (int)(intImageSize / (float)parameters.Rows);
        var rowGap = (int)((1f - parameters.BrickHeight) * rowHeight);
        var columnWidth = (int)(intImageSize / (float)parameters.Columns);
        var columnGap = (int)((1f - parameters.BrickWidth) * columnWidth);

        var data = new byte[intImageSize * intImageSize * Color.Size];
        var dataSpan = data.AsSpan();

        for (var row = 0; row < parameters.Rows; ++row)
        {
            var rowOffset = row % 2 == 0 ? 0 : (int)(parameters.Offset * columnWidth);

            var initialBrickEnd = (int)(rowOffset - (1f - parameters.BrickWidth) * columnWidth);
            (int Start, int Length, int BrickIndex)[] brickLines =
                Enumerable.Range(0, parameters.Columns)
                    .Select(c => (rowOffset + c * columnWidth, columnWidth - columnGap, c))
                    .Prepend((0, initialBrickEnd, parameters.Columns - 1))
                    .ToArray();

            foreach (var (start, length, brickIndex) in brickLines)
            {
                if (length <= 0) continue;

                for (var y = 0; y < rowHeight - rowGap; ++y)
                {
                    dataSpan.Slice(((row * rowHeight + y) * intImageSize + start) * Color.Size, length * Color.Size).Fill((byte)(bricks[brickIndex][row] * 255));
                }
            }
        }

        return new Texture(imageSize, data);
    }

    public class Parameters
    {
        public required PositiveInt Rows { get; init; }
        public required PositiveInt Columns { get; init; }
        public Percentage Offset { get; init; } = 0.5f;
        public Percentage BrickWidth { get; init; } = 1.0f;
        public Percentage BrickHeight { get; init; } = 1.0f;
        public Percentage MinimumHeight { get; init; } = 0.0f;
        public Percentage MaximumHeight { get; init; } = 1.0f;
        public Percentage HeightVariance { get; init; } = 0.0f;
    }
}