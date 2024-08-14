namespace TextureGen.Tests;

using System.Diagnostics;
using Generation;

public class BrickGeneratorTests
{
    [TestCaseSource(typeof(CommonTestCases), nameof(CommonTestCases.AllImageSizes))]
    public void RecordGeneratorPerformance(ImageSize imageSize)
    {
        var generator = new BrickGenerator(imageSize);

        var time = Enumerable.Range(0, 10).Select(_ =>
        {
            var stopwatch = Stopwatch.StartNew();

            generator.Generate(new BrickGenerator.Parameters
            {
                Rows = 10,
                Columns = 10,
                BrickWidth = 0.8f,
                BrickHeight = 0.8f,
                HeightVariance = 0.5f
            });

            return stopwatch.ElapsedMilliseconds;
        }).Average();

        Console.WriteLine($"{time}ms");
    }
}