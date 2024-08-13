namespace TextureGen;

using System.Numerics;
using Func;

public static class Normals
{
    public static Texture CalculateNormals(this Texture texture)
    {
        var colorMap = Color.ArrayFromBytes(texture.Data);

        var intSize = (int)texture.ImageSize;
        var normals = new Vector3[intSize, intSize];

        for (var y = 0; y < intSize; ++y)
        {
            for (var x = 0; x < intSize; ++x)
            {
                normals[x, y] = Vector3.Normalize(
                    new Vector3(
                        2f * (GetPixelAverage(x + 1, y) - GetPixelAverage(x - 1, y)),
                        2f * (GetPixelAverage(x, y + 1) - GetPixelAverage(x, y - 1)),
                        -4f)
                    );
            }
        }

        return new Texture(texture.ImageSize, (x, y) => NormalToColor(normals[x, y]));

        float GetPixelAverage(int x, int y) =>
            colorMap[(x + intSize) % intSize + ((y + intSize) % intSize) * intSize]
                .Map(c => (c.Red + c.Green + c.Blue) / 3f / 255f);

        byte VectorValueToByte(float value) => (byte)(255 * (0.5f + value / 2f));

        Color NormalToColor(Vector3 normal) =>
            Color.FromArgb(255, VectorValueToByte(normal.X), VectorValueToByte(normal.Y), VectorValueToByte(-normal.Z));

    }
}