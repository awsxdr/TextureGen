namespace TextureGen.Generation;

public class SimpleNoiseGenerator(ImageSize imageSize) : IGenerator<SimpleNoiseGenerator.Parameters>
{
    public Texture Generate(Parameters parameters) =>
        new (imageSize, (_, _) => Color.Gray((byte)Random.Shared.Next(256)));

    public class Parameters;
}