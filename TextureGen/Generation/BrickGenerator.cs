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

        var rowHeight = (int)(imageSize / (float)parameters.Rows);
        var rowGap = (int)((1f - parameters.BrickHeight) * rowHeight);
        var columnWidth = (int)(imageSize / (float)parameters.Columns);
        var columnGap = (int)((1f - parameters.BrickWidth) * columnWidth);

        var data = new byte[imageSize * imageSize * Color.Size];
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
                    dataSpan.Slice(((row * rowHeight + y) * imageSize + start) * Color.Size, length * Color.Size).Fill((byte)(bricks[brickIndex][row] * 255));
                }
            }
        }

        return new Texture(imageSize, data);
    }

    public class Parameters
    {
        public required int Rows { get; init; }
        public required int Columns { get; init; }
        public float Offset { get; init; } = 0.5f;
        public float BrickWidth { get; init; } = 1.0f;
        public float BrickHeight { get; init; } = 1.0f;
        public float MinimumHeight { get; init; } = 0.0f;
        public float MaximumHeight { get; init; } = 1.0f;
        public float HeightVariance { get; init; } = 0.0f;
    }
}