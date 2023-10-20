namespace Hawthorn;

public static class BehaviorBranchBuilders
{
	public static BehaviorBranchBuilder<A> Sequence<A>(this BehaviorBuilder<A> builder, params IBehaviorNodeBuilder<A>[] children)
	{
		return new BehaviorBranchBuilder<A>(BehaviorBranchBuilder<A>.BranchType.Sequence, children);
	}

	public static BehaviorBranchBuilder<A> Sequence<A>(this BehaviorBuilder<A> builder, string name, params IBehaviorNodeBuilder<A>[] children)
	{
		return new BehaviorBranchBuilder<A>(BehaviorBranchBuilder<A>.BranchType.Sequence, name, children);
	}

	public static BehaviorBranchBuilder<A> StatefulSequence<A>(this BehaviorBuilder<A> builder, params IBehaviorNodeBuilder<A>[] children)
	{
		return new BehaviorBranchBuilder<A>(BehaviorBranchBuilder<A>.BranchType.AsyncSequence, children);
	}

	public static BehaviorBranchBuilder<A> StatefulSequence<A>(this BehaviorBuilder<A> builder, string name, params IBehaviorNodeBuilder<A>[] children)
	{
		return new BehaviorBranchBuilder<A>(BehaviorBranchBuilder<A>.BranchType.AsyncSequence, name, children);
	}

	public static BehaviorBranchBuilder<A> Selector<A>(this BehaviorBuilder<A> builder, params IBehaviorNodeBuilder<A>[] children)
	{
		return new BehaviorBranchBuilder<A>(BehaviorBranchBuilder<A>.BranchType.Selector, children);
	}

	public static BehaviorBranchBuilder<A> Selector<A>(this BehaviorBuilder<A> builder, string name, params IBehaviorNodeBuilder<A>[] children)
	{
		return new BehaviorBranchBuilder<A>(BehaviorBranchBuilder<A>.BranchType.Selector, name, children);
	}

	public static BehaviorBranchBuilder<A> StatefulSelector<A>(this BehaviorBuilder<A> builder, params IBehaviorNodeBuilder<A>[] children)
	{
		return new BehaviorBranchBuilder<A>(BehaviorBranchBuilder<A>.BranchType.AsyncSelector, children);
	}

	public static BehaviorBranchBuilder<A> StatefulSelector<A>(this BehaviorBuilder<A> builder, string name, params IBehaviorNodeBuilder<A>[] children)
	{
		return new BehaviorBranchBuilder<A>(BehaviorBranchBuilder<A>.BranchType.AsyncSelector, name, children);
	}

	public static BehaviorBranchBuilder<A> Parallel<A>(this BehaviorBuilder<A> builder, params IBehaviorNodeBuilder<A>[] children)
	{
		return new BehaviorBranchBuilder<A>(BehaviorBranchBuilder<A>.BranchType.Parallel, children);
	}

	public static BehaviorBranchBuilder<A> Parallel<A>(this BehaviorBuilder<A> builder, string name, params IBehaviorNodeBuilder<A>[] children)
	{
		return new BehaviorBranchBuilder<A>(BehaviorBranchBuilder<A>.BranchType.Parallel, name, children);
	}
}

public class BehaviorBranchBuilder<A> : IBehaviorNodeBuilder<A>
{
	public enum BranchType
	{
		Sequence,
		AsyncSequence,
		Selector,
		AsyncSelector,
		Parallel
	}

	public BranchType Type { get; init; }

	public string Name { get; init; }
	public IBehaviorNodeBuilder<A>[] Children { get; init; }

	public BehaviorBranchBuilder(BranchType type, IBehaviorNodeBuilder<A>[] children)
	{
		Type = type;
		Children = children;
	}

	public BehaviorBranchBuilder(BranchType type, string name, IBehaviorNodeBuilder<A>[] children)
	{
		Name = name;
		Type = type;
		Children = children;
	}

	public IBehaviorNode<A> Build()
	{
		IBehaviorNode<A>[] childNodes = Children.Select(c => c.Build()).ToArray();

		BehaviorNodeContainer<A> node = Type switch
		{
			BranchType.Sequence => new BehaviorSequence<A>(Name, childNodes),
			BranchType.AsyncSequence => new StatefulBehaviorSequence<A>(Name, childNodes),
			BranchType.Selector => new BehaviorSelector<A>(Name, childNodes),
			BranchType.AsyncSelector => new StatefulBehaviorSelector<A>(Name, childNodes),
			BranchType.Parallel => new BehaviorParallel<A>(Name, childNodes),
			_ => throw new Exception("Unknown type: " + Type)
		};

#if DEBUG
		node.DebugLogging = DebugLogging;
		return node;
#endif
	}

#if DEBUG
	DebugFlags DebugLogging;

	public BehaviorBranchBuilder<A> LogFailures()
	{
		DebugLogging |= DebugFlags.Failures;
		return this;
	}

	public BehaviorBranchBuilder<A> LogSuccesses()
	{
		DebugLogging |= DebugFlags.Successess;
		return this;
	}

	public BehaviorBranchBuilder<A> LogAll()
	{
		DebugLogging |= DebugFlags.Failures | DebugFlags.Busy | DebugFlags.Successess;	
		return this;
	}
#endif
}
