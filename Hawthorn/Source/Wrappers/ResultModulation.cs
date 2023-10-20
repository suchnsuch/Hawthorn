namespace Hawthorn;

public class ResultModulator<A> : BehaviorNodeWrapper<A>
{
	public Result ResultOnFail { get; init; } = Result.Failed;
	public Result ResultOnSuccess { get; init; } = Result.Succeeded;
	public Result ResultOnBusy { get; init; } = Result.Busy;

	public ResultModulator(IBehaviorNode<A> child)
		: base(child)
	{
	}

	public override Result Run(Tick<A> tick)
	{
#if DEBUG
		MarkDebugPosition(tick);
#endif

		return Child.Run(tick) switch {
			Result.Failed => ResultOnFail,
			Result.Succeeded => ResultOnSuccess,
			Result.Busy => ResultOnBusy,
			_ => Result.Failed // Won't happen
		};
	}
}

public class ResultModulatorBuilder<A> : IBehaviorNodeBuilder<A>
{
	public Result ResultOnFail { get; set; } = Result.Failed;
	public Result ResultOnSuccess { get; set; } = Result.Succeeded;
	public Result ResultOnBusy { get; set; } = Result.Busy;

	IBehaviorNodeBuilder<A> Child;

	public ResultModulatorBuilder(IBehaviorNodeBuilder<A> child)
	{
		Child = child;
	}

	public IBehaviorNode<A> Build()
	{
		return new ResultModulator<A>(Child.Build())
		{
			ResultOnFail = ResultOnFail,
			ResultOnSuccess = ResultOnSuccess,
			ResultOnBusy = ResultOnBusy
		};
	}
}

public static class ResultModulatorExtensions
{
	public static ResultModulatorBuilder<A> AlwaysFail<A>(this IBehaviorNodeBuilder<A> child)
	{
		return new ResultModulatorBuilder<A>(child)
		{
			ResultOnSuccess = Result.Failed,
			ResultOnBusy = Result.Failed
		};
	}

	public static ResultModulatorBuilder<A> AlwaysSucceed<A>(this IBehaviorNodeBuilder<A> child)
	{
		return new ResultModulatorBuilder<A>(child)
		{
			ResultOnFail = Result.Succeeded,
			ResultOnBusy = Result.Succeeded
		};
	}

	public static ResultModulatorBuilder<A> AlwaysBusy<A>(this IBehaviorNodeBuilder<A> child)
	{
		return new ResultModulatorBuilder<A>(child)
		{
			ResultOnSuccess = Result.Busy,
			ResultOnFail = Result.Busy
		};
	}

	public static ResultModulatorBuilder<A> SucceedOnFailure<A>(this IBehaviorNodeBuilder<A> child)
	{
		return new ResultModulatorBuilder<A>(child)
		{
			ResultOnFail = Result.Succeeded
		};
	}

	public static ResultModulatorBuilder<A> FailOnSuccess<A>(this IBehaviorNodeBuilder<A> child)
	{
		return new ResultModulatorBuilder<A>(child)
		{
			ResultOnSuccess = Result.Failed
		};
	}

	public static ResultModulatorBuilder<A> BusyOnSuccess<A>(this IBehaviorNodeBuilder<A> child)
	{
		return new ResultModulatorBuilder<A>(child)
		{
			ResultOnSuccess = Result.Busy
		};
	}
}
