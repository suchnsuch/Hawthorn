namespace Hawthorn;

public abstract class BehaviorNodeContainer<A> : IBehaviorNodeContainer<A>
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

	public IEnumerable<IBehaviorNode<A>> ChildNodes => Children;

	public abstract Result Run(Tick<A> tick);

#if DEBUG
	public void FlowDepth(int depth)
	{
		Depth = depth;
		int childDepth = Depth + 1;
		foreach (var child in Children)
		{
			if (child is IBehaviorNodeContainer<A> container)
			{
				container.FlowDepth(childDepth);
			}
		}
	}

	protected void MarkDebugPosition(Tick<A> tick)
	{
		tick.MarkDebugPosition(Depth, Name ?? "");
	}

	public DebugFlags DebugLogging { get; set; }

	protected void LogChildResult(Tick<A> tick, IBehaviorNode<A> child, Result result)
	{
		if (result == Result.Failed && DebugLogging.HasFlag(DebugFlags.Failures))
		{
			tick.DebugLog(DebugLogLevel.Message, $"Child {Array.IndexOf(Children, child)} of {Name}, \"{child}\" failed.", Depth);
		}
		else if (result == Result.Busy && DebugLogging.HasFlag(DebugFlags.Busy))
		{
			tick.DebugLog(DebugLogLevel.Message, $"Child {Array.IndexOf(Children, child)} of {Name}, \"{child}\" is running.", Depth);
		}
		else if (result == Result.Succeeded && DebugLogging.HasFlag(DebugFlags.Successess))
		{
			tick.DebugLog(DebugLogLevel.Message, $"Child {Array.IndexOf(Children, child)} of {Name}, \"{child}\" succeeded.", Depth);
		}
	}
#endif
}
