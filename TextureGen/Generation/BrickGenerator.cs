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

        var rowHeight = (int)imageSize / (float)parameters.Rows;
        var columnWidth = (int)imageSize / (float)parameters.Columns;

        return new Texture(imageSize, (x, y) =>
        {
            var yIndex = (int)(y / rowHeight);
            var offsetX = (x + (yIndex % 2 == 0 ? 0 : (int)(parameters.Offset * columnWidth))) % (int)imageSize;
            var xIndex = (int)(offsetX / columnWidth);
            var brickColor = Color.Gray((byte)(bricks[xIndex][yIndex] * 255));

            var xPercentage = offsetX % columnWidth / columnWidth;
            var yPercentage = y % rowHeight / rowHeight;

            if (xPercentage > parameters.BrickWidth || yPercentage > parameters.BrickHeight)
                brickColor = Color.FromArgb(255, 0, 0, 0);

            return brickColor;
        });
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