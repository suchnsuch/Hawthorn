namespace BehaviorTrees;

/// <summary>
/// This serves as a launch point for extension methods to build off from
/// </summary>
public class BehaviorBuilder<A>
{
	
}

public interface IBehaviorNodeBuilder<A>
{
	IBehaviorNode<A> Build();
}

public static class BehaviorBuilderExtensions
{
	public static BehaviorTree<A> ToTree<A>(this IBehaviorNodeBuilder<A> nodeBuilder)
	{
		var node = nodeBuilder.Build();
		if (node is IBehaviorNodeBranch<A> branch)
		{
			return new BehaviorTree<A>(branch);
		}

		throw new System.ArgumentException("Cannot build a behavior tree from a leaf node. Use a branch node instead.");
	}
}
