﻿namespace TextureGen.Tests;

using System.Diagnostics;
using Generation;

public class RectangleGeneratorTests
{
    [TestCaseSource(typeof(CommonTestCases), nameof(CommonTestCases.AllImageSizes))]
    public void RecordGeneratorPerformance(ImageSize imageSize)
    {
        var generator = new RectangleGenerator(imageSize);

        var time = Enumerable.Range(0, 10).Select(_ =>
        {
            var stopwatch = Stopwatch.StartNew();

            generator.Generate(new RectangleGenerator.Parameters
            {
                Left = 0.25f,
                Top = 0.25f,
                Width = 0.5f,
                Height = 0.5f,
            });

            return stopwatch.ElapsedMilliseconds;
        }).Average();

        Console.WriteLine($"{time}ms");
    }
}