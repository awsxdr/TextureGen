namespace TextureGen.Tests;

using System.Reflection;

public static class CommonTestCases
{
    public static IEnumerable<TestCaseData> AllImageSizes =>
        typeof(ImageSize).GetFields(BindingFlags.Static | BindingFlags.Public)
            .Where(f => f.FieldType == typeof(ImageSize))
            .Select(f => (ImageSize)f.GetValue(null)!)
            .Select(s => new TestCaseData(s).SetName($"Image size {(int)s}"));
}