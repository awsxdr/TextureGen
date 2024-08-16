namespace TextureGen;

public static class Combination
{
    public static Texture ApplyAsMask(this Texture mask, Texture texture1, Texture texture2)
    {
        if (texture1.ImageSize != texture2.ImageSize || texture1.ImageSize != mask.ImageSize) throw new TextureSizesDoNotMatchException();

        var maskData = mask.Data.Span;
        var texture1Data = texture1.Data.Span;
        var texture2Data = texture2.Data.Span;
        var resultData = new byte[texture1Data.Length];

        for (var i = 0; i < resultData.Length; ++i)
        {
            resultData[i] =
                maskData[i] switch
                {
                    0 => texture2Data[i],
                    255 => texture1Data[i],
                    var w => (byte)((w * texture1Data[i] + (255 - w) * texture2Data[i]) / 255)
                };
        }

        return new(mask.ImageSize, resultData);
    }

    public class TextureSizesDoNotMatchException : ArgumentException;
}