namespace Hawthorn;

public class DeleteValueBehavior<A> : IBehaviorNode<A>
{
	readonly string ValueKey;
	readonly bool AlwaysSucceed;

	public DeleteValueBehavior(string key, bool alwaysSucceed)
	{
		ValueKey = key;
		AlwaysSucceed = alwaysSucceed;
	}

	public Result Run(Tick<A> tick)
	{
		return (tick.State.Delete(ValueKey) || AlwaysSucceed).ToSucceededOrFailed();
	}
}

public class DeleteValueBuilder<A> : IBehaviorNodeBuilder<A>
{
	string ValueKey;
	bool AlwaysSucceed = true;

	public DeleteValueBuilder(string key)
	{
		ValueKey = key;
	}

	public IBehaviorNode<A> Build()
	{
		return new DeleteValueBehavior<A>(ValueKey, AlwaysSucceed);
	}

	public DeleteValueBuilder<A> FailOnNoValue()
	{
		AlwaysSucceed = false;
		return this;
	}
}

public static class DeleteValueExtensions
{
	public static DeleteValueBuilder<A> Delete<A>(string key)
	{
		return new DeleteValueBuilder<A>(key);
	}
}
