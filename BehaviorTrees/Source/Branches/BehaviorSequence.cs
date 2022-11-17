namespace BehaviorTrees;

public class BehaviorSequence<A> : BehaviorNodeContainer<A>
{
	public BehaviorSequence(IBehaviorNode<A>[] children)
		: base(children)
	{
	}

	public override Result Run(Tick<A> tick)
	{
		foreach (var child in Children)
		{
			var result = child.Run(tick);
			switch (result)
			{
				case Result.Busy:
				case Result.Failed:
					return result;
			}
		}

		return Result.Succeeded;
	}
}

public class StatefulBehaviorSequence<A> : BehaviorNodeContainer<A>, IStatefulBehaviorNode<A>
{
	public StatefulBehaviorSequence(IBehaviorNode<A>[] children)
		: base(children)
	{
	}

	public int StateID { get; set; }

	public override Result Run(Tick<A> tick)
	{
		int startingIndex = tick.GetState<int>(this);

		for (int index = startingIndex; index < Children.Length; index++)
		{
			var result = Children[index].Run(tick);
			switch (result)
			{
				case Result.Busy:
					// child in progress, will resume here
					tick.SetState(this, index);
					tick.MarkActive(this);
					return result;
				case Result.Failed:
					// child failed, will restart on next entry
					tick.SetState(this, 0);
					return result;
			}
		}
		// Succeeded, will restart on next entry
		tick.SetState(this, 0);
		return Result.Succeeded;
	}

	public object GetInitialState(Tick<A> tick)
	{
		return 0;
	}

	public void Sleep(Tick<A> tick)
	{
		tick.SetState(this, 0);
	}
}
