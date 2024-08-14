namespace TextureGen.Tests;

using System.Diagnostics;
using Generation;

public class ColorGeneratorTests
{
    [TestCaseSource(typeof(CommonTestCases), nameof(CommonTestCases.AllImageSizes))]
    public void RecordGeneratorPerformance(ImageSize imageSize)
    {
        var generator = new ColorGenerator(imageSize);

        var time = Enumerable.Range(0, 10).Select(_ =>
        {
            var stopwatch = Stopwatch.StartNew();

            generator.Generate(new ColorGenerator.Parameters { Color = Color.FromRgb(250, 150, 50) });

            return stopwatch.ElapsedMilliseconds;
        }).Average();

        Console.WriteLine($"{time}ms");
    }
}