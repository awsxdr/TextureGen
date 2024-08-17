namespace TextureGen;

using System.Numerics;

public static class Normals
{
    public static NormalTexture CalculateNormals(this Texture texture)
    {
        int imageSize = texture.ImageSize;
        var averages = new float[texture.Data.Length / Color.Size];
        var averagesSpan = averages.AsSpan();

        var data = texture.Data;
        for (int i = 0, averagesIndex = 0; i < data.Length; i += Color.Size, averagesIndex++)
        {
            var slice = data.Slice(i, Color.Size);
            var total = 0;
            foreach (var @byte in slice.Span)
            {
                total += @byte;
            }

            averagesSpan[averagesIndex] = total / (float)Color.Size / 255f;
        }

        var averagesMap = new float[imageSize + 2][];
        for (var y = -1; y <= imageSize; ++y)
        {
            averagesMap[y + 1] = new float[imageSize + 2];
            var rowSpan = averagesMap[y + 1].AsSpan();
            var wrappedY = (y + imageSize) % imageSize;
            averagesSpan.Slice(wrappedY * imageSize, imageSize).CopyTo(rowSpan[1..]);
            rowSpan[0] = averagesSpan[wrappedY * imageSize + imageSize - 1];
            rowSpan[imageSize + 1] = averagesSpan[wrappedY * imageSize];
        }

        var normals = new Vector3[imageSize * imageSize];

        var normalIndex = 0;
        for (var y = 1; y <= imageSize; ++y)
        {
            for (var x = 1; x <= imageSize; ++x)
            {
                normals[normalIndex++] = Vector3.Normalize(
                    new Vector3(
                        2f * (averagesMap[y][x + 1] - averagesMap[y][x - 1]),
                        2f * (averagesMap[y + 1][x] - averagesMap[y - 1][x]),
                        -4f)
                    );
            }
        }

        return new NormalTexture(imageSize, normals);
    }
}