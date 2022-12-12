namespace Hawthorn;

public class BehaviorParallel<A> : BehaviorNodeContainer<A>
{
	public BehaviorParallel(string name, IBehaviorNode<A>[] children)
		: base(name, children)
	{
	}

	public override Result Run(Tick<A> tick)
	{
#if DEBUG
		MarkDebugPosition(tick);
#endif

		var worstResult = Result.Succeeded;
		foreach (var child in Children)
		{
			worstResult = child.Run(tick).Worst(worstResult);
		}

		return worstResult;
	}
}
