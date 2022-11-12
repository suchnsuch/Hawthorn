using System.Collections.Generic;

namespace BehaviorTrees
{
	public class BehaviorTree<A>
	{
		public IBehaviorNodeBranch<A> RootNode { get; protected set; }
		List<IStatefulBehaviorNode<A>> StatefulNodes;

		public BehaviorTree(IBehaviorNodeBranch<A> rootNode)
		{
			RootNode = rootNode;
			GatherStatefulNodes(rootNode);
		}

		int GatherStatefulNodes(IBehaviorNode<A> node, int nextID = 0)
		{
			if (node.Cast<IStatefulBehaviorNode<A>>(out var statefulNode))
			{
				statefulNode.StateID = nextID;
				nextID++;
				StatefulNodes.Add(statefulNode);
			}

			return nextID;
		}
	}

	public class BehaviorTreeState
	{
		
	}
}
