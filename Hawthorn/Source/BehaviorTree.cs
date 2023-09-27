namespace Hawthorn;

public class BehaviorTree<A>
{
	public IBehaviorNodeContainer<A> RootNode { get; protected set; }
	List<IStatefulBehaviorNode<A>> StatefulNodes = new List<IStatefulBehaviorNode<A>>();

	public BehaviorTree(IBehaviorNodeContainer<A> rootNode)
	{
		RootNode = rootNode;
		GatherStatefulNodes(rootNode);

#if DEBUG
		if (rootNode is BehaviorNodeContainer<A> container)
		{
			container.FlowDepth(0);
		}
#endif
	}

	void GatherStatefulNodes(IBehaviorNode<A> node)
	{
		if (node is IStatefulBehaviorNode<A> statefulNode)
		{
			statefulNode.StateID = StatefulNodes.Count;
			StatefulNodes.Add(statefulNode);
		}

		if (node is IBehaviorNodeContainer<A> branchNode)
		{
			foreach (var child in branchNode.ChildNodes)
			{
				GatherStatefulNodes(child);
			}
		}
	}

	public IStatefulBehaviorNode<A> GetStatefulNode(int nodeID)
	{
		return StatefulNodes[nodeID];
	}

	public object[] BuildNodeStates(Tick<A> tick)
	{
		var result = new object[StatefulNodes.Count];

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

	public string PrintTree()
	{
		return PrintNode("", RootNode);
	}

	string PrintNode(string prefix, IBehaviorNode<A> node)
	{
		var result = prefix + node.GetType().Name;
		if (node is IStatefulBehaviorNode<A> stateful)
		{
			result += " " + stateful.StateID;
		}
		if (node is IBehaviorNodeContainer<A> container)
		{
			result += " [\n";
			foreach (var child in container.ChildNodes)
			{
				result += PrintNode(prefix + "\t", child) + "\n";
			}

			result += prefix + "]";
		}
		return result;
	}
}

public class BehaviorTreeState
{
	
}
