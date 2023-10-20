namespace Hawthorn;

public class BehaviorSelector<A> : BehaviorNodeContainer<A>
{
	public BehaviorSelector(string name, IBehaviorNode<A>[] children)
		: base(name, children)
	{
	}

	public override Result Run(Tick<A> tick)
	{
#if DEBUG
		MarkDebugPosition(tick);
#endif

		foreach (var child in Children)
		{
			var result = child.Run(tick);

#if DEBUG
			LogChildResult(tick, child, result);
#endif

			switch (result)
			{
				case Result.Succeeded:
				case Result.Busy:
					return result;
			}
		}

		return Result.Failed;
	}

	public override string ToString()
    {
        return $"Selector({Name})";
    }
}

public class StatefulBehaviorSelector<A> : BehaviorNodeContainer<A>, IStatefulBehaviorNode<A>
{
	public StatefulBehaviorSelector(string name, IBehaviorNode<A>[] children)
		: base(name, children)
	{
	}

	public int StateID { get; set; }

	public override Result Run(Tick<A> tick)
	{
#if DEBUG
		MarkDebugPosition(tick);
#endif

		int startingIndex = tick.GetState<int>(this);

		for (int index = startingIndex; index < Children.Length; index++)
		{
			var result = Children[index].Run(tick);

#if DEBUG
			LogChildResult(tick, Children[index], result);
#endif

			switch (result)
			{
				case Result.Busy:
					// child in progress, will resume here
					tick.SetState(this, index);
					tick.MarkActive(this);
					return result;
				case Result.Succeeded:
					// child succeeded, will restart on next entry
					tick.SetState(this, 0);
					return result;
			}
		}
		// Failed, will restart on next entry
		tick.SetState(this, 0);
		return Result.Failed;
	}

	public object GetInitialState(Tick<A> tick)
	{
		return 0;
	}

	public void Sleep(Tick<A> tick)
	{
		tick.SetState(this, 0);
	}

	public override string ToString()
    {
        return $"StatefulSelector({Name})";
    }
}
