namespace TextureGen;

public static class Scaling
{
    public static Texture ScaleToSizeNearest(this Texture texture, ImageSize size)
    {
        if (texture.ImageSize == size) return texture;

        var scaleFactor = texture.ImageSize / (float)size;

        return new Texture(size, (x, y) =>
        {
            var sourceX = (int)(x * scaleFactor);
            var sourceY = (int)(y * scaleFactor);

            return texture.ColorAt(sourceX, sourceY);
        });
    }

    public static Texture ScaleToSizeBilinear(this Texture texture, ImageSize size)
    {
        if (texture.ImageSize == size) return texture;

        var scaleFactor = texture.ImageSize / (float)size;

        var colorMap = Color.ArrayFromBytes(texture.Data);

        return scaleFactor < 1f
            ? new Texture(size, (x, y) =>
            {
                var sourceX = (int)(x * scaleFactor);
                var sourceY = (int)(y * scaleFactor);

                var progressX = x * scaleFactor % 1f;
                var progressY = y * scaleFactor % 1f;

                return
                    GetColor(sourceX, sourceY) * (1f - progressX) * (1f - progressY)
                    + GetColor(sourceX + 1, sourceY) * progressX * (1f - progressY)
                    + GetColor(sourceX, sourceY + 1) * (1f - progressX) * progressY
                    + GetColor(sourceX + 1, sourceY + 1) * progressX * progressY;
            })
            : new Texture(size, (x, y) =>
            {
                var sourceX = (int)(x * scaleFactor);
                var sourceY = (int)(y * scaleFactor);
                var sourceColors =
                    Enumerable.Range(sourceX, (int)scaleFactor).SelectMany(px =>
                            Enumerable.Range(sourceY, (int)scaleFactor).Select(py => (px, py)))
                        .Select(p => GetColor(p.px, p.py).ToBytes())
                        .ToArray();

                var totals =
                    sourceColors.Aggregate(
                        new int[3],
                        (c, v) =>
                        [
                            c[0] + v[0],
                            c[1] + v[1],
                            c[2] + v[2],
                        ]);

                return Color.FromRgb(
                    (byte)(totals[0] / (float)sourceColors.Length),
                    (byte)(totals[1] / (float)sourceColors.Length),
                    (byte)(totals[2] / (float)sourceColors.Length));
            });

        Color GetColor(int x, int y) => colorMap[(y % texture.ImageSize) * texture.ImageSize + (x % texture.ImageSize)];
    }
}