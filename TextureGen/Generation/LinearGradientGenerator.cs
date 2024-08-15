namespace TextureGen.Generation;

public class LinearGradientGenerator(ImageSize imageSize) : IGenerator<LinearGradientGenerator.Parameters>
{
    public Texture Generate(Parameters parameters)
    {
        var resultData = new byte[imageSize * imageSize * Color.Size];
        var resultSpan = resultData.AsSpan();

        for (var row = 0; row < imageSize; ++row)
        {
            var rowValue = (byte)(row / (float)imageSize * 255);
            resultSpan.Slice(row * imageSize * Color.Size, imageSize * Color.Size).Fill(rowValue);
        }

        return new(imageSize, resultData);
    }

    public class Parameters
    {
    }
}