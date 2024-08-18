using System.Diagnostics;
using TextureGen.Generation;

namespace TextureGen.Tests;

public class CircleGeneratorTests
{
    [TestCaseSource(typeof(CommonTestCases), nameof(CommonTestCases.AllImageSizes))]
    public void RecordSolidGeneratorPerformance(ImageSize imageSize)
    {
        var generator = new CircleGenerator(imageSize);

        var time = Enumerable.Range(0, 10).Select(_ =>
        {
            var stopwatch = Stopwatch.StartNew();

            generator.Generate(new CircleGenerator.Parameters { FillType = CircleFillType.Solid, Radius = 0.5f });

            return stopwatch.ElapsedMilliseconds;
        }).Average();

        Console.WriteLine($"{time}ms");
    }

    [TestCaseSource(typeof(CommonTestCases), nameof(CommonTestCases.AllImageSizes))]
    public void RecordLinearGeneratorPerformance(ImageSize imageSize)
    {
        var generator = new CircleGenerator(imageSize);

        var time = Enumerable.Range(0, 10).Select(_ =>
        {
            var stopwatch = Stopwatch.StartNew();

            generator.Generate(new CircleGenerator.Parameters { FillType = CircleFillType.Linear, Radius = 0.5f });

            return stopwatch.ElapsedMilliseconds;
        }).Average();

        Console.WriteLine($"{time}ms");
    }

    [TestCaseSource(typeof(CommonTestCases), nameof(CommonTestCases.AllImageSizes))]
    public void RecordCurvedGeneratorPerformance(ImageSize imageSize)
    {
        var generator = new CircleGenerator(imageSize);

        var time = Enumerable.Range(0, 10).Select(_ =>
        {
            var stopwatch = Stopwatch.StartNew();

            generator.Generate(new CircleGenerator.Parameters { FillType = CircleFillType.Curved, Radius = 0.5f });

            return stopwatch.ElapsedMilliseconds;
        }).Average();

        Console.WriteLine($"{time}ms");
    }
}