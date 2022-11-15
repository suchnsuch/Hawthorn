namespace BehaviorTrees;

public interface Tick<A>
{
	A Actor { get; }
	Blackboard State { get; }

	float Delta { get; }

	T? GetState<T>(IStatefulBehaviorNode<A> node);
	
	void SetState(IStatefulBehaviorNode<A> node, object? value);

	bool MarkActive(IStatefulBehaviorNode<A> node);
}
