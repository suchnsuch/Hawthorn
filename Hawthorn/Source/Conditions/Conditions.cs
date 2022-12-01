namespace Hawthorn;

public enum Comparison
{
	Equal,
	NotEqual,
	LessThan,
	LessThanOrEqual,
	GreaterThan,
	GreaterThanOrEqual
}

public static class Conditions
{
	const float SmallDifference = 0.00001f;

	public static bool Compare(this Comparison comparison, float a, float b)
	{
		return comparison switch {
			Comparison.Equal => MathF.Abs(a - b) < SmallDifference,
			Comparison.NotEqual => MathF.Abs(a - b) > SmallDifference,
			Comparison.LessThan => a < b,
			Comparison.LessThanOrEqual => a <= b,
			Comparison.GreaterThan => a > b,
			Comparison.GreaterThanOrEqual => a >= b,
			_ => throw new ArgumentException("Unknown comparison: " + comparison)
		};
	}

	public static bool Compare(this Comparison comparison, double a, double b)
	{
		return comparison switch {
			Comparison.Equal => Math.Abs(a - b) < SmallDifference,
			Comparison.NotEqual => Math.Abs(a - b) > SmallDifference,
			Comparison.LessThan => a < b,
			Comparison.LessThanOrEqual => a <= b,
			Comparison.GreaterThan => a > b,
			Comparison.GreaterThanOrEqual => a >= b,
			_ => throw new ArgumentException("Unknown comparison: " + comparison)
		};
	}
}
