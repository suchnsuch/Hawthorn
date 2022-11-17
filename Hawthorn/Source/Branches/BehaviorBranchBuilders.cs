namespace Hawthorn;

public static class BehaviorBranchBuilders
{
	public static BehaviorBranchBuilder<A> Sequence<A>(this BehaviorBuilder<A> builder, params IBehaviorNodeBuilder<A>[] children)
	{
		return new BehaviorBranchBuilder<A>(BehaviorBranchBuilder<A>.BranchType.Sequence, children);
	}

	public static BehaviorBranchBuilder<A> StatefulSequence<A>(this BehaviorBuilder<A> builder, params IBehaviorNodeBuilder<A>[] children)
	{
		return new BehaviorBranchBuilder<A>(BehaviorBranchBuilder<A>.BranchType.AsyncSequence, children);
	}

	public static BehaviorBranchBuilder<A> Selector<A>(this BehaviorBuilder<A> builder, params IBehaviorNodeBuilder<A>[] children)
	{
		return new BehaviorBranchBuilder<A>(BehaviorBranchBuilder<A>.BranchType.Selector, children);
	}

	public static BehaviorBranchBuilder<A> StatefulSelector<A>(this BehaviorBuilder<A> builder, params IBehaviorNodeBuilder<A>[] children)
	{
		return new BehaviorBranchBuilder<A>(BehaviorBranchBuilder<A>.BranchType.AsyncSelector, children);
	}

	public static BehaviorBranchBuilder<A> Parallel<A>(this BehaviorBuilder<A> builder, params IBehaviorNodeBuilder<A>[] children)
	{
		return new BehaviorBranchBuilder<A>(BehaviorBranchBuilder<A>.BranchType.Parallel, children);
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

	public IBehaviorNodeBuilder<A>[] Children { get; init; }

	public BehaviorBranchBuilder(BranchType type, IBehaviorNodeBuilder<A>[] children)
	{
		Type = type;
		Children = children;
	}

	public IBehaviorNode<A> Build()
	{
		IBehaviorNode<A>[] childNodes = Children.Select(c => c.Build()).ToArray();

		return Type switch
		{
			BranchType.Sequence => new BehaviorSequence<A>(childNodes),
			BranchType.AsyncSequence => new StatefulBehaviorSequence<A>(childNodes),
			BranchType.Selector => new BehaviorSelector<A>(childNodes),
			BranchType.AsyncSelector => new StatefulBehaviorSelector<A>(childNodes),
			BranchType.Parallel => new BehaviorParallel<A>(childNodes),
			_ => throw new Exception("Unknown type: " + Type)
		};
	}
}
