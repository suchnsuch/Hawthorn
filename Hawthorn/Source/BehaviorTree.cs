namespace Hawthorn;

public class BehaviorTree<A>
{
	public IBehaviorNodeBranch<A> RootNode { get; protected set; }
	List<IStatefulBehaviorNode<A>> StatefulNodes = new List<IStatefulBehaviorNode<A>>();

	public BehaviorTree(IBehaviorNodeBranch<A> rootNode)
	{
		RootNode = rootNode;
		GatherStatefulNodes(rootNode);
	}

	void GatherStatefulNodes(IBehaviorNode<A> node)
	{
		if (node is IStatefulBehaviorNode<A> statefulNode)
		{
			statefulNode.StateID = StatefulNodes.Count;
			StatefulNodes.Add(statefulNode);
		}

		if (node is IBehaviorNodeBranch<A> branchNode)
		{
			foreach (var child in branchNode.Children)
			{
				GatherStatefulNodes(child);
			}
		}
	}

	public IStatefulBehaviorNode<A> GetStatefulNode(int nodeID)
	{
		return StatefulNodes[nodeID];
	}

	public object?[] BuildNodeStates(Tick<A> tick)
	{
		var result = new object?[StatefulNodes.Count];

		for (int i = 0; i < result.Length; i++)
		{
			result[i] = StatefulNodes[i].GetInitialState(tick);
		}

		return result;
	}

	public bool[] BuildActivityStates()
	{
		var result = new bool[StatefulNodes.Count];
		for (int i = 0; i < result.Length; i++)
		{
			result[i] = false;
		}
		return result;
	}
}

public class BehaviorTreeState
{
	
}
