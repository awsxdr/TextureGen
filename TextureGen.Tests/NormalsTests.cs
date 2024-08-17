namespace TextureGen.Tests;

using System.Diagnostics;
using Generation;

public class NormalsTests
{
    [TestCaseSource(typeof(CommonTestCases), nameof(CommonTestCases.AllImageSizes))]
    public void RecordGeneratorPerformance(ImageSize imageSize)
    {
        var generator = new SimpleNoiseGenerator(imageSize);

        var texture = generator.Generate(new());

        var time = Enumerable.Range(0, 10).Select(_ =>
        {
            var stopwatch = Stopwatch.StartNew();

            texture.CalculateNormals();

            return stopwatch.ElapsedMilliseconds;
        }).Average();

        Console.WriteLine($"{time}ms");
    }
}