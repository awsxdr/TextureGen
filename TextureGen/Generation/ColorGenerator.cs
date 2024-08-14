namespace TextureGen.Generation;

public class ColorGenerator(ImageSize imageSize) : IGenerator<ColorGenerator.Parameters>
{
    public Texture Generate(Parameters parameters)
    {
        var data = Enumerable.Repeat(parameters.Color, (int)imageSize * (int)imageSize).ToArray().ToBytes();

        return new(imageSize, data);
    }

    public class Parameters
    {
        public required Color Color { get; init; }
    }
}