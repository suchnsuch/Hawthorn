namespace Hawthorn;

public class MarkTimeBehavior<A> : IBehaviorNode<A>, IBehaviorNodeBuilder<A>
{
	readonly string TimeKey;
	readonly double TimeOffset;

	public MarkTimeBehavior(string key, double offset)
	{
		TimeKey = key;
		TimeOffset = offset;
	}

	public Result Run(Tick<A> tick)
	{
		tick.State.Set(TimeKey, tick.Time + TimeOffset);
		return Result.Succeeded;
	}

	IBehaviorNode<A> IBehaviorNodeBuilder<A>.Build()
	{
		return this;
	}
}

public static class MarkTimeExtensions
{
	public static MarkTimeBehavior<A> MarkTime<A>(this BehaviorBuilder<A> b, string key, double offset = 0)
	{
		return new MarkTimeBehavior<A>(key, offset);
	}
}
