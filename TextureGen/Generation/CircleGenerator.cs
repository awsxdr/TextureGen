namespace TextureGen.Generation;

public class CircleGenerator(ImageSize imageSize) : IGenerator<CircleGenerator.Parameters>
{
    public Texture Generate(Parameters parameters) =>
        parameters.FillType switch
        {
            CircleFillType.Solid => GenerateSolid(parameters),
            CircleFillType.Linear => GenerateLinear(parameters),
            CircleFillType.Curved => GenerateCurved(parameters),
            _ => throw new FillTypeNotValidException()
        };

    private Texture GenerateSolid(Parameters parameters)
    {
        var data = new byte[imageSize * imageSize * Color.Size];
        var dataSpan = data.AsSpan();

        var imageRadius = imageSize * parameters.Radius;
        var midpoint = imageSize / 2;
        var rowStart = (int)(midpoint - imageRadius);

        for (var row = rowStart; row < midpoint; ++row)
        {
            var halfWidth = Math.Max(0, (int)(MathF.Sin(MathF.Acos((midpoint - row) / imageRadius)) * imageRadius));
            var start = midpoint - halfWidth;

            dataSpan.Slice((int)(row * imageSize + start) * Color.Size, halfWidth * 2 * Color.Size).Fill(255);
            dataSpan.Slice((int)((imageSize - row - 1) * imageSize + start) * Color.Size, halfWidth * 2 * Color.Size).Fill(255);
        }

        return new Texture(imageSize, data);
    }

    private Texture GenerateLinear(Parameters parameters)
    {
        var data = new byte[imageSize * imageSize * Color.Size];
        var dataSpan = data.AsSpan();

        return new Texture(imageSize, data);
    }

    private Texture GenerateCurved(Parameters parameters)
    {
        var data = new byte[imageSize * imageSize * Color.Size];
        var dataSpan = data.AsSpan();

        return new Texture(imageSize, data);
    }

    public class Parameters
    {
        public float Radius { get; init; } = 1.0f;
        public CircleFillType FillType { get; init; } = CircleFillType.Solid;
    }

    public class FillTypeNotValidException : ArgumentException;
}

public enum CircleFillType
{
    Solid,
    Linear,
    Curved,
}
