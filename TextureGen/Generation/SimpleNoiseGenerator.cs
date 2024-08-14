namespace TextureGen.Generation;

public class SimpleNoiseGenerator(ImageSize imageSize) : IGenerator<SimpleNoiseGenerator.Parameters>
{
    public Texture Generate(Parameters parameters)
    {
        var randomData = new byte[(int)imageSize * (int)imageSize];
        Random.Shared.NextBytes(randomData);

        var data = new byte[randomData.Length * Color.Size];
        var dataSpan = data.AsSpan();

        for (var i = 0; i < randomData.Length; ++i)
        {
            dataSpan.Slice(i * Color.Size, Color.Size).Fill(randomData[i]);
        }

        return new Texture(imageSize, data);
    }

    public class Parameters;
}