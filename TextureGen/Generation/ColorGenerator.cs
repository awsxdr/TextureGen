namespace TextureGen.Generation;

public class ColorGenerator(ImageSize imageSize) : IGenerator<ColorGenerator.Parameters>
{
    public Texture Generate(Parameters parameters) => new(imageSize, (_, _) => parameters.Color);

    public class Parameters
    {
        public required Color Color { get; init; }
    }
}