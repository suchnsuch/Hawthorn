namespace Hawthorn;

using Godot;

public class DistanceTo3DIsFloat<A> : IBehaviorNode<A> where A : Node3D
{
	readonly string ValueKey;
	readonly Comparison Comparison;
	readonly float ComparisonValue;

	public DistanceTo3DIsFloat(string key, Comparison comparison, float value)
	{
		ValueKey = key;
		Comparison = comparison;
		ComparisonValue = value;
	}

	public Result Run(Tick<A> tick)
	{
		Vector3 actorPosition = tick.Actor.GlobalPosition;

		Vector3 targetPosition;

		if (!tick.State.TryGetPosition(ValueKey, out targetPosition))
		{
			return Result.Failed;
		}

		float squareDistance = actorPosition.DistanceSquaredTo(targetPosition);

		return Comparison.Compare(squareDistance, ComparisonValue * ComparisonValue).ToResult();
	}
}

public class DistanceTo3DIsKey<A> : IBehaviorNode<A> where A : Node3D
{
	readonly string ValueKey;
	readonly Comparison Comparison;
	readonly string ComparisonValueKey;

	public DistanceTo3DIsKey(string key, Comparison comparison, string valueKey)
	{
		ValueKey = key;
		Comparison = comparison;
		ComparisonValueKey = valueKey;
	}

	public Result Run(Tick<A> tick)
	{
		Vector3 actorPosition = tick.Actor.GlobalPosition;

		Vector3 targetPosition;

		if (!tick.State.TryGetPosition(ValueKey, out targetPosition))
		{
			return Result.Failed;
		}

		float squareDistance = actorPosition.DistanceSquaredTo(targetPosition);

		float targetValue = tick.State.Get<float>(ComparisonValueKey);

		return Comparison.Compare(squareDistance, targetValue * targetValue).ToResult();
	}
}


public static class DistanceToBuilders
{
	public static DistanceTo3DBuilder<A> DistanceTo<A>(this BehaviorBuilder<A> builder, string key) where A : Node3D
	{
		return new DistanceTo3DBuilder<A>(key);
	}
}

public class DistanceTo3DBuilder<A> : IBehaviorNodeBuilder<A> where A : Node3D
{
	string ValueKey;
	Comparison Comparison;
	float ComparisonValue;
	string? ComparisonValueKey;

	public DistanceTo3DBuilder(string targetKey)
	{
		ValueKey = targetKey;
	}

	public DistanceTo3DBuilder<A> Equals(float value)
	{
		Comparison = Comparison.Equal;
		ComparisonValue = value;
		return this;
	}

	public DistanceTo3DBuilder<A> DoesNotEqual(float value)
	{
		Comparison = Comparison.NotEqual;
		ComparisonValue = value;
		return this;
	}

	public DistanceTo3DBuilder<A> IsLessThan(float value)
	{
		Comparison = Comparison.LessThan;
		ComparisonValue = value;
		return this;
	}

	public DistanceTo3DBuilder<A> IsLessThanOrEqualTo(float value)
	{
		Comparison = Comparison.LessThanOrEqual;
		ComparisonValue = value;
		return this;
	}

	public DistanceTo3DBuilder<A> IsGreaterThan(float value)
	{
		Comparison = Comparison.GreaterThan;
		ComparisonValue = value;
		return this;
	}

	public DistanceTo3DBuilder<A> IsGreaterThanOrEqualTo(float value)
	{
		Comparison = Comparison.GreaterThanOrEqual;
		ComparisonValue = value;
		return this;
	}

	public DistanceTo3DBuilder<A> Equals(string value)
	{
		Comparison = Comparison.Equal;
		ComparisonValueKey = value;
		return this;
	}

	public DistanceTo3DBuilder<A> DoesNotEqual(string value)
	{
		Comparison = Comparison.NotEqual;
		ComparisonValueKey = value;
		return this;
	}

	public DistanceTo3DBuilder<A> IsLessThan(string value)
	{
		Comparison = Comparison.LessThan;
		ComparisonValueKey = value;
		return this;
	}

	public DistanceTo3DBuilder<A> IsLessThanOrEqualTo(string value)
	{
		Comparison = Comparison.LessThanOrEqual;
		ComparisonValueKey = value;
		return this;
	}

	public DistanceTo3DBuilder<A> IsGreaterThan(string value)
	{
		Comparison = Comparison.GreaterThan;
		ComparisonValueKey = value;
		return this;
	}

	public DistanceTo3DBuilder<A> IsGreaterThanOrEqualTo(string value)
	{
		Comparison = Comparison.GreaterThanOrEqual;
		ComparisonValueKey = value;
		return this;
	}

	public IBehaviorNode<A> Build()
	{
		if (ComparisonValueKey != null)
		{
			return new DistanceTo3DIsKey<A>(ValueKey, Comparison, ComparisonValueKey);
		}
		return new DistanceTo3DIsFloat<A>(ValueKey, Comparison, ComparisonValue);
	}
}
