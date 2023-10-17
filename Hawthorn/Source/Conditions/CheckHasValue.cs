namespace Hawthorn;

public class CheckHasValue<A> : IBehaviorNode<A>
{
	readonly string ValueKey;

	readonly Result ResultWithValue;
	readonly Result ResultWithoutValue;

	public CheckHasValue(string key, Result withValue, Result withoutValue)
	{
		ValueKey = key;
		ResultWithValue = withValue;
		ResultWithoutValue = withoutValue;
	}

	public Result Run(Tick<A> tick)
	{
		if (tick.State.Has(ValueKey)) return ResultWithValue;
		return ResultWithoutValue;
	}
}

public class CheckHasValueBuilder<A> : IBehaviorNodeBuilder<A>
{
	string ValueKey;

	Result ResultWithValue = Result.Succeeded;
	Result ResultWithoutValue = Result.Failed;

	public CheckHasValueBuilder(string key)
	{
		ValueKey = key;
	}

	public CheckHasValueBuilder<A> SucceedOrBusy()
	{
		ResultWithValue = Result.Succeeded;
		ResultWithoutValue = Result.Busy;
		return this;
	}

	public CheckHasValueBuilder<A> BusyOrFailed()
	{
		ResultWithValue = Result.Busy;
		ResultWithoutValue = Result.Failed;
		return this;
	}

	public CheckHasValueBuilder<A> FailedOrSucceeded()
	{
		ResultWithValue = Result.Failed;
		ResultWithoutValue = Result.Succeeded;
		return this;
	}

	public CheckHasValueBuilder<A> FailedOrBusy()
	{
		ResultWithValue = Result.Failed;
		ResultWithoutValue = Result.Busy;
		return this;
	}

	public IBehaviorNode<A> Build()
	{
		return new CheckHasValue<A>(ValueKey, ResultWithValue, ResultWithoutValue);
	}
}

public static class CheckHasValueExtensions
{
	public static CheckHasValueBuilder<A> HasValue<A>(this BehaviorBuilder<A> b, string key)
	{
		return new CheckHasValueBuilder<A>(key);
	}
}

