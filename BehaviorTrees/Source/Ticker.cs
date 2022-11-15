namespace BehaviorTrees;

/// <summary>
/// The per-instance state for running a `BehaviorTree` on an Actor.
/// </summary>
/// <typeparam name="A">The Actor type.</typeparam>
public class Ticker<A> : Tick<A>
{
	public A Actor { get; protected set; }
	public Blackboard State { get; protected set; }
	public BehaviorTree<A> Tree { get; protected set; }

	object?[] NodeStates;
	bool[] LastNodeActivity;
	bool[] ThisNodeActivity;

	float delta;

	public Ticker(A actor, Blackboard state, BehaviorTree<A> tree)
	{
		Actor = actor;
		State = state;
		Tree = tree;

		NodeStates = tree.BuildNodeStates(this);
		LastNodeActivity = tree.BuildActivityStates();
		ThisNodeActivity = tree.BuildActivityStates();
	}

	public Result Update(float delta)
	{
		// Prepare
		this.delta = delta;
		ClearActivityArray(ThisNodeActivity);

		// Execute
		var result = Tree.RootNode.Run(this);

		// Sleep nodes no longer running
		for (int i = 0; i < ThisNodeActivity.Length; i++)
		{
			if (LastNodeActivity[i] && !ThisNodeActivity[i])
			{
				Tree.GetStatefulNode(i).Sleep(this);
			}
		}

		// Swap activity buffers
		var next = LastNodeActivity;
		LastNodeActivity = ThisNodeActivity;
		ThisNodeActivity = next;

		return result;
	}

	public T? GetState<T>(IStatefulBehaviorNode<A> node)
	{
		var value = NodeStates[node.StateID];
		if (value == null) return default(T);
		return (T)value;
	}
	
	public void SetState(IStatefulBehaviorNode<A> node, object? value)
	{
		NodeStates[node.StateID] = value;
	}

	void ClearActivityArray(bool[] array)
	{
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = false;
		}
	}

	// Keep the fact that this is actually a Tick a little obfuscated
	float Tick<A>.Delta => delta;

	bool Tick<A>.MarkActive(IStatefulBehaviorNode<A> node)
	{
		return ThisNodeActivity[node.StateID] = true;
	}
}
