using System.Diagnostics;
using TextureGen.Generation;

namespace TextureGen.Tests;

public class TextureTests
{
    [TestCaseSource(typeof(CommonTestCases), nameof(CommonTestCases.AllImageSizes))]
    public void RecordMultiplyPerformance(ImageSize imageSize)
    {
        var texture1 = Generate.Color(imageSize, new ColorGenerator.Parameters { Color = Color.FromRgb(255, 0, 0) });
        var texture2 = Generate.Color(imageSize, new ColorGenerator.Parameters { Color = Color.FromRgb(0, 0, 255) });

        var time = Enumerable.Range(0, 10).Select(__ =>
        {
            var stopwatch = Stopwatch.StartNew();

            _ = texture1 * texture2;

            return stopwatch.ElapsedMilliseconds;
        }).Average();

        Console.WriteLine($"{time}ms");
    }

    [TestCaseSource(typeof(CommonTestCases), nameof(CommonTestCases.AllImageSizes))]
    public void RecordNotPerformance(ImageSize imageSize)
    {
        var texture = Generate.Color(imageSize, new ColorGenerator.Parameters { Color = Color.FromRgb(255, 0, 0) });

        var time = Enumerable.Range(0, 10).Select(__ =>
        {
            var stopwatch = Stopwatch.StartNew();

            _ = ~texture;

            return stopwatch.ElapsedMilliseconds;
        }).Average();

        Console.WriteLine($"{time}ms");
    }

}