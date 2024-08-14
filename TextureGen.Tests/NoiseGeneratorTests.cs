namespace TextureGen.Tests;

using System.Diagnostics;
using Generation;

public class NoiseGeneratorTests
{
    [TestCaseSource(typeof(CommonTestCases), nameof(CommonTestCases.AllImageSizes))]
    public void RecordGeneratorPerformance(ImageSize imageSize)
    {
        var generator = new SimpleNoiseGenerator(imageSize);

        var time = Enumerable.Range(0, 10).Select(_ =>
        {
            var stopwatch = Stopwatch.StartNew();

            generator.Generate(new());

            return stopwatch.ElapsedMilliseconds;
        }).Average();

        Console.WriteLine($"{time}ms");
    }
}
