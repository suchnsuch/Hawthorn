namespace BehaviorTrees;

/*
 * These allow for quickly adding simple code-based behavior when you just need to operate on an actor.
 * Thanks to generics, they are strongly typed.
 */

public class ActorDelegateBehavior<A> : IBehaviorNode<A>, IBehaviorNodeBuilder<A>
{
	public delegate Result ActorDelegate(A actor);
	
	ActorDelegate Handler;

	public ActorDelegateBehavior(ActorDelegate handler)
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
	public delegate Result ActorDeltaDelegate(A actor, float delta);
	
	ActorDeltaDelegate Handler;

	public ActorDeltaDelegateBehavior(ActorDeltaDelegate handler)
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

public static class DelegateBehaviorBuilders
{
	public static IBehaviorNodeBuilder<A> Lambda<A>(this BehaviorBuilder<A> builder, ActorDelegateBehavior<A>.ActorDelegate lambda)
	{
		return new ActorDelegateBehavior<A>(lambda);
	}

	public static IBehaviorNodeBuilder<A> Lambda<A>(this BehaviorBuilder<A> builder, ActorDeltaDelegateBehavior<A>.ActorDeltaDelegate lambda)
	{
		return new ActorDeltaDelegateBehavior<A>(lambda);
	}
}
