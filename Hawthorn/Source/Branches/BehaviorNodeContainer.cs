namespace Hawthorn;

public abstract class BehaviorNodeContainer<A> : IBehaviorNodeBranch<A>
{
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
}
