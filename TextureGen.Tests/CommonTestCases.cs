namespace TextureGen.Tests;

public static class CommonTestCases
{
    public static IEnumerable<TestCaseData> AllImageSizes =>
        Enum.GetValues<ImageSize>().Select(s => new TestCaseData(s).SetName($"Image size {s}"));
}