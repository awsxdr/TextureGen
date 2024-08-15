namespace TextureGen;

using System.Numerics;
using Func;

public static class Normals
{
    public static NormalTexture CalculateNormals(this Texture texture)
    {
        var colorMap = Color.ArrayFromBytes(texture.Data);

        var normals = new Vector3[texture.ImageSize * texture.ImageSize];

        var normalIndex = 0;
        for (var y = 0; y < texture.ImageSize; ++y)
        {
            for (var x = 0; x < texture.ImageSize; ++x)
            {
                normals[normalIndex++] = Vector3.Normalize(
                    new Vector3(
                        2f * (GetPixelAverage(x + 1, y) - GetPixelAverage(x - 1, y)),
                        2f * (GetPixelAverage(x, y + 1) - GetPixelAverage(x, y - 1)),
                        -4f)
                    );
            }
        }

        return new NormalTexture(texture.ImageSize, normals);

        float GetPixelAverage(int x, int y) =>
            colorMap[(x + texture.ImageSize) % texture.ImageSize + ((y + texture.ImageSize) % texture.ImageSize) * texture.ImageSize]
                .Map(c => (c.Red + c.Green + c.Blue) / 3f / 255f);

    }
}