namespace TextureGen;

using ImageMagick;

public static class Persistence
{
    public static void SavePng(this TextureBase texture, string path)
    {
        using var image = new ImageMagick.MagickImage();
        image.ReadPixels(
            texture.Data.ToArray(), 
            new PixelReadSettings((int)texture.ImageSize, (int)texture.ImageSize, StorageType.Char, PixelMapping.BGRA));

        var data = image.ToByteArray(MagickFormat.Png);
        File.WriteAllBytes(path, data);
    }
}