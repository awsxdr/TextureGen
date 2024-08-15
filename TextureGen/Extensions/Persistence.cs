namespace TextureGen;

using ImageMagick;

public static class Persistence
{
    public static void SavePng(this TextureBase texture, string path)
    {
        using var image = new MagickImage();
        image.ReadPixels(
            texture.Data.ToArray(), 
            new PixelReadSettings(texture.ImageSize, texture.ImageSize, StorageType.Char, PixelMapping.BGR));

        var data = image.ToByteArray(MagickFormat.Png);
        File.WriteAllBytes(path, data);
    }
}