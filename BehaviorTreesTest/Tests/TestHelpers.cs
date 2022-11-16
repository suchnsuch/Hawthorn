using BehaviorTrees;

namespace BehaviorTreesTest;

public static class TestHelpers
{
	public static Ticker<A> CreateTestTicker<A>(this BehaviorTree<A> tree) where A : new()
	{
		return new Ticker<A>(new A(), new Blackboard(), tree);
	}
}
