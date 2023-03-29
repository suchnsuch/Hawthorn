namespace Hawthorn;

public enum Result
{
	Succeeded,
	Busy,
	Failed
}

public static class ResultExtensions
{
	public static Result Worst(this Result result, Result other)
	{
		return result > other ? result : other;
	}

	public static Result Best(this Result result, Result other)
	{
		return result < other ? result : other;
	}

	public static Result ToSucceededOrFailed(this bool b)
	{
		return b ? Result.Succeeded : Result.Failed;
	}

	public static Result ToBusyOrFailed(this bool b)
	{
		return b ? Result.Busy : Result.Failed;
	}
}

/// <summary>
/// A node within a <see cref="BehaviorTree"/>
/// </summary>
/// <typeparam name="A">
/// The firm type of the actor using the behavior.
/// Ensures that client code has access to what it needs without casting.
/// </typeparam>
public interface IBehaviorNode<A>
{
	Result Run(Tick<A> tick);
}

/// <summary>
/// A node that contains one or more children.
/// </summary>
public interface IBehaviorNodeContainer<A> : IBehaviorNode<A>
{
	IEnumerable<IBehaviorNode<A>> ChildNodes { get; }
}

public interface IStatefulBehaviorNode<A> : IBehaviorNode<A>
{
	int StateID { get; set; }
	object GetInitialState(Tick<A> tick);
	void Sleep(Tick<A> tick);
}
