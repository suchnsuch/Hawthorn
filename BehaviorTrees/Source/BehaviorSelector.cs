namespace BehaviorTrees
{
	public class BehaviorSelector<A> : BehaviorNodeContainer<A>
	{
		public BehaviorSelector(IBehaviorNode<A>[] children)
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
					case Result.Succeeded:
					case Result.Busy:
						return result;
				}
			}

			return Result.Failed;
		}
	}

	public class AsyncBehaviorSelector<A> : BehaviorNodeContainer<A>, IStatefulBehaviorNode<A>
	{
		public AsyncBehaviorSelector(IBehaviorNode<A>[] children)
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
	}
}
