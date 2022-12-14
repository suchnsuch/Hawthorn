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

public interface IBehaviorNode<A>
{
	Result Run(Tick<A> tick);
}

public interface IBehaviorNodeBranch<A> : IBehaviorNode<A>
{
	IBehaviorNode<A>[] Children { get; }
}

public interface IStatefulBehaviorNode<A> : IBehaviorNode<A>
{
	int StateID { get; set; }
	object GetInitialState(Tick<A> tick);
	void Sleep(Tick<A> tick);
}
