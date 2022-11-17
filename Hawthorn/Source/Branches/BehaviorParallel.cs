namespace Hawthorn;

public class BehaviorParallel<A> : BehaviorNodeContainer<A>
{
	public BehaviorParallel(IBehaviorNode<A>[] children)
		: base(children)
	{
	}

	public override Result Run(Tick<A> tick)
	{
		var worstResult = Result.Succeeded;
		foreach (var child in Children)
		{
			worstResult = child.Run(tick).Worst(worstResult);
		}

		return worstResult;
	}
}
