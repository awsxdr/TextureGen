namespace TextureGen;

using Generation;

public static class Generate
{
    public static Texture Color(ImageSize size, ColorGenerator.Parameters parameters) =>
        new ColorGenerator(size).Generate(parameters);

    public static Texture Bricks(ImageSize size, BrickGenerator.Parameters parameters) =>
        new BrickGenerator(size).Generate(parameters);

    public static Texture SimpleNoise(ImageSize size) =>
        new SimpleNoiseGenerator(size).Generate(new());

    public static Texture Rectangle(ImageSize size, RectangleGenerator.Parameters parameters) =>
        new RectangleGenerator(size).Generate(parameters);

    public static Texture LinearGradient(ImageSize size) =>
        new LinearGradientGenerator(size).Generate(new());
}
