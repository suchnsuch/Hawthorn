namespace BehaviorTrees;

public abstract class BehaviorNodeContainer<A> : IBehaviorNodeBranch<A>
{
	public BehaviorNodeContainer(IEnumerable<IBehaviorNode<A>> children)
	{
		Children = children.ToArray();
	}

	public BehaviorNodeContainer(IBehaviorNode<A>[] children)
	{
		Children = children;
	}

	public abstract Result Run(Tick<A> tick);
	public IBehaviorNode<A>[] Children { get; protected set; }
}
