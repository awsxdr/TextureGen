namespace TextureGen;

public struct PositiveInt
{
    private readonly int _value;

    public PositiveInt(int value)
    {
        if (value < 1) throw new ValueMustBeGreaterThanZeroException();

        _value = value;
    }

    public static implicit operator int(PositiveInt value) => value._value;
    public static implicit operator PositiveInt(int value) => new(value);

    public class ValueMustBeGreaterThanZeroException : ArgumentOutOfRangeException;
}