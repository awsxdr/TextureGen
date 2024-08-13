namespace TextureGen;

public struct Percentage(float value)
{
    private readonly float _value = MathF.Min(1f, MathF.Max(0f, value));

    public static implicit operator float(Percentage value) => value._value;
    public static implicit operator Percentage(float value) => new(value);
}