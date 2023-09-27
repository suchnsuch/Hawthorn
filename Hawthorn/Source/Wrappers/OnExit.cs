namespace Hawthorn;

/// <summary>
/// Wrap some node and call a lambda on exit
/// </summary>
public class OnExit<A> : BehaviorNodeWrapper<A>, IStatefulBehaviorNode<A>
{
	public delegate void ExitHandler(Tick<A> tick);

	public int StateID { get; set; }

	readonly ExitHandler Handler;

	public OnExit(IBehaviorNode<A> child, ExitHandler handler)
		: base(child)
	{
		Handler = handler;
	}

	public override Result Run(Tick<A> tick)
	{
#if DEBUG
		MarkDebugPosition(tick);
#endif
		Result result = Child.Run(tick);
		if (result != Result.Failed)
		{
			tick.MarkActive(this);
		}
		return result;
	}

	public object GetInitialState(Tick<A> tick) => null;

	public void Sleep(Tick<A> tick)
	{
		Handler(tick);
	}
}

public class OnExitBuilder<A> : IBehaviorNodeBuilder<A>
{
	IBehaviorNodeBuilder<A> Child;
	OnExit<A>.ExitHandler Handler;

	public OnExitBuilder(IBehaviorNodeBuilder<A> child, OnExit<A>.ExitHandler handler)
	{
		Child = child;
		Handler = handler;
	}

	public IBehaviorNode<A> Build()
	{
		return new OnExit<A>(Child.Build(), Handler);
	}
}

public static class OnExitExtensions
{
	public static OnExitBuilder<A> OnExit<A>(this IBehaviorNodeBuilder<A> child, OnExit<A>.ExitHandler handler)
	{
		return new OnExitBuilder<A>(child, handler);
	}
}
