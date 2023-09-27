
namespace Hawthorn;

public abstract class BehaviorNodeWrapper<A> : IBehaviorNodeContainer<A>
{
	protected readonly IBehaviorNode<A> Child;

#if DEBUG
	protected int Depth;
#endif

	public BehaviorNodeWrapper(IBehaviorNode<A> child)
	{
		Child = child;
	}

	public abstract Result Run(Tick<A> tick);

	public IEnumerable<IBehaviorNode<A>> ChildNodes
	{
		get
		{
			yield return Child;
		}
	}

#if DEBUG
	public void FlowDepth(int depth)
	{
		Depth = depth;
		if (Child is IBehaviorNodeContainer<A> container)
		{
			container.FlowDepth(depth + 1);
		}
	}

	protected void MarkDebugPosition(Tick<A> tick)
	{
		tick.MarkDebugPosition(Depth, "");
	}
#endif
}
