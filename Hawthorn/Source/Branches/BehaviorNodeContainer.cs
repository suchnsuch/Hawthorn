namespace Hawthorn;

public abstract class BehaviorNodeContainer<A> : IBehaviorNodeBranch<A>
{
#if DEBUG
	/// <summary>
	/// The depth this container is from the root
	/// </summary>
	protected int Depth;
#endif

	public string Name { get; init; }
	public IBehaviorNode<A>[] Children { get; protected set; }

	public BehaviorNodeContainer(string name, IEnumerable<IBehaviorNode<A>> children)
	{
		Name = name;
		Children = children.ToArray();
	}

	public BehaviorNodeContainer(string name, IBehaviorNode<A>[] children)
	{
		Name = name;
		Children = children;
	}

	public abstract Result Run(Tick<A> tick);

#if DEBUG
	public void FlowDepth(int depth)
	{
		Depth = depth;
		int childDepth = Depth + 1;
		foreach (var child in Children)
		{
			if (child is BehaviorNodeContainer<A> container)
			{
				container.FlowDepth(childDepth);
			}
		}
	}

	protected void MarkDebugPosition(Tick<A> tick)
	{
		tick.MarkDebugPosition(Depth, Name ?? "");
	}
#endif
}
