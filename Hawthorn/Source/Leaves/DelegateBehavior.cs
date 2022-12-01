namespace Hawthorn;

/*
 * These allow for quickly adding simple code-based behavior when you just need to operate on an actor.
 * Thanks to generics, they are strongly typed.
 */

public class ActorDelegateBehavior<A> : IBehaviorNode<A>, IBehaviorNodeBuilder<A>
{
	public delegate Result Delegate(A actor);
	
	readonly Delegate Handler;

	public ActorDelegateBehavior(Delegate handler)
	{
		Handler = handler;
	}

	public Result Run(Tick<A> tick)
	{
		return Handler(tick.Actor);
	}

	IBehaviorNode<A> IBehaviorNodeBuilder<A>.Build()
	{
		return this;
	}
}

public class ActorDeltaDelegateBehavior<A> : IBehaviorNode<A>, IBehaviorNodeBuilder<A>
{
	public delegate Result Delegate(A actor, float delta);
	
	readonly Delegate Handler;

	public ActorDeltaDelegateBehavior(Delegate handler)
	{
		Handler = handler;
	}

	public Result Run(Tick<A> tick)
	{
		return Handler(tick.Actor, tick.Delta);
	}

	IBehaviorNode<A> IBehaviorNodeBuilder<A>.Build()
	{
		return this;
	}
}

public class ActorSimpleDelegateBehavior<A> : IBehaviorNode<A>, IBehaviorNodeBuilder<A>
{
	public delegate void Delegate(A actor);
	
	readonly Delegate Handler;

	public ActorSimpleDelegateBehavior(Delegate handler)
	{
		Handler = handler;
	}

	public Result Run(Tick<A> tick)
	{
		Handler(tick.Actor);
		return Result.Succeeded;
	}

	IBehaviorNode<A> IBehaviorNodeBuilder<A>.Build()
	{
		return this;
	}
}

public class ActorBoolDelegateBehavior<A> : IBehaviorNode<A>, IBehaviorNodeBuilder<A>
{
	public delegate bool Delegate(A actor);
	
	readonly Delegate Handler;

	public ActorBoolDelegateBehavior(Delegate handler)
	{
		Handler = handler;
	}

	public Result Run(Tick<A> tick)
	{
		return Handler(tick.Actor) ? Result.Succeeded : Result.Failed;
	}

	IBehaviorNode<A> IBehaviorNodeBuilder<A>.Build()
	{
		return this;
	}
}

public class TickDelegateBehavior<A> : IBehaviorNode<A>, IBehaviorNodeBuilder<A>
{
	public delegate Result Delegate(Tick<A> tick);
	
	readonly Delegate Handler;

	public TickDelegateBehavior(Delegate handler)
	{
		Handler = handler;
	}

	public Result Run(Tick<A> tick)
	{
		return Handler(tick);
	}

	IBehaviorNode<A> IBehaviorNodeBuilder<A>.Build()
	{
		return this;
	}
}

public static class DelegateBehaviorBuilders
{
	public static IBehaviorNodeBuilder<A> Lambda<A>(this BehaviorBuilder<A> builder, ActorDelegateBehavior<A>.Delegate lambda)
	{
		return new ActorDelegateBehavior<A>(lambda);
	}

	public static IBehaviorNodeBuilder<A> Lambda<A>(this BehaviorBuilder<A> builder, ActorDeltaDelegateBehavior<A>.Delegate lambda)
	{
		return new ActorDeltaDelegateBehavior<A>(lambda);
	}

	public static IBehaviorNodeBuilder<A> Lambda<A>(this BehaviorBuilder<A> builder, ActorSimpleDelegateBehavior<A>.Delegate lambda)
	{
		return new ActorSimpleDelegateBehavior<A>(lambda);
	}

	public static IBehaviorNodeBuilder<A> Lambda<A>(this BehaviorBuilder<A> builder, ActorBoolDelegateBehavior<A>.Delegate lambda)
	{
		return new ActorBoolDelegateBehavior<A>(lambda);
	}

	public static IBehaviorNodeBuilder<A> LambdaTick<A>(this BehaviorBuilder<A> builder, TickDelegateBehavior<A>.Delegate lambda)
	{
		return new TickDelegateBehavior<A>(lambda);
	}
}
