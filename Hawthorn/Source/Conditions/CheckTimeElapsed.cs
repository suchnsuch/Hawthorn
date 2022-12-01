namespace Hawthorn;

public class CheckTimeElapsed<A> : IBehaviorNode<A>
{
	readonly string TimeKey;
	readonly double? DefaultOffset;
	readonly double ElapsedValue;
	readonly Comparison ElapsedComparison;
	readonly Result ResultOnPass;
	readonly Result ResultOnFail;

	public CheckTimeElapsed(
		string key,
		double? defaultOffset,
		double elapsedValue,
		Comparison elapsedComparison,
		Result passResult,
		Result failResult
	) {
		TimeKey = key;
		DefaultOffset = defaultOffset;
		ElapsedValue = elapsedValue;
		ElapsedComparison = elapsedComparison;
		ResultOnPass = passResult;
		ResultOnFail = failResult;
	}

	public Result Run(Tick<A> tick)
	{
		double storedTime;
		if (!tick.State.TryGet<double>(TimeKey, out storedTime))
		{
			if (DefaultOffset.HasValue)
			{
				// Initialize value
				storedTime = tick.Time + DefaultOffset.Value;
				tick.State.Set(TimeKey, storedTime);
			}
			else
			{
				// No value, cannot set
				return Result.Failed;
			}
		}

		double timeDelta = tick.Time - storedTime;
		bool comparison = ElapsedComparison.Compare(timeDelta, ElapsedValue);
		return comparison ? ResultOnPass : ResultOnFail;
	}
}

public class CheckTimeElapsedBuilder<A> : IBehaviorNodeBuilder<A>
{
	string TimeKey;
	double? DefaultOffset = null;
	double ElapsedValue = 0;
	Comparison ElapsedComparison = Comparison.GreaterThanOrEqual;
	Result ResultOnPass = Result.Succeeded;
	Result ResultOnFail = Result.Failed;

	public CheckTimeElapsedBuilder(string key)
	{
		TimeKey = key;
	}

	public CheckTimeElapsedBuilder<A> DefaultTo(double value)
	{
		DefaultOffset = value;
		return this;
	}

	public CheckTimeElapsedBuilder<A> Equals(double value)
	{
		ElapsedValue = value;
		ElapsedComparison = Comparison.Equal;
		return this;
	}

	public CheckTimeElapsedBuilder<A> DoesNotEqual(double value)
	{
		ElapsedValue = value;
		ElapsedComparison = Comparison.NotEqual;
		return this;
	}

	public CheckTimeElapsedBuilder<A> GreaterThan(double value)
	{
		ElapsedValue = value;
		ElapsedComparison = Comparison.GreaterThan;
		return this;
	}

	public CheckTimeElapsedBuilder<A> GreaterThanOrEquals(double value)
	{
		ElapsedValue = value;
		ElapsedComparison = Comparison.GreaterThanOrEqual;
		return this;
	}

	public CheckTimeElapsedBuilder<A> LessThan(double value)
	{
		ElapsedValue = value;
		ElapsedComparison = Comparison.LessThan;
		return this;
	}

	public CheckTimeElapsedBuilder<A> LessThanOrEquals(double value)
	{
		ElapsedValue = value;
		ElapsedComparison = Comparison.LessThanOrEqual;
		return this;
	}

	public CheckTimeElapsedBuilder<A> BusyWhileWaiting()
	{
		ResultOnFail = Result.Busy;
		return this;
	}

	public IBehaviorNode<A> Build()
	{
		return new CheckTimeElapsed<A>(
			TimeKey,
			DefaultOffset,
			ElapsedValue,
			ElapsedComparison,
			ResultOnPass,
			ResultOnFail
		);
	}
}

public static class ChecktimeElapsedExtensions
{
	public static CheckTimeElapsedBuilder<A> TimeElapsed<A>(this BehaviorBuilder<A> b, string key)
	{
		return new CheckTimeElapsedBuilder<A>(key);
	}
}
