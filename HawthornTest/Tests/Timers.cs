using Xunit;
using Hawthorn;

namespace HawthornTest;

public class Timers
{
	[Fact]
	public void TimersFailWithoutValue()
	{
		var _ = new BehaviorBuilder<object>();

		string result = "not run";

		var ticker = _.Selector(
			_.Sequence("Check Timer",
				_.Parallel(
					_.TimeElapsed("test").GreaterThanOrEquals(5).BusyWhileWaiting(),
					_.Lambda(a => result = "Timer checked")
				),
				_.Lambda(a => result = "Timer Completed")
			),
			_.Sequence("Set Timer",
				_.MarkTime("test"),
				_.Lambda(a => result = "Timer set")
			)
		).ToTree().CreateTestTicker();

		Assert.Equal(Result.Succeeded, ticker.Update(1));
		Assert.Equal("Timer set", result);

		Assert.Equal(Result.Busy, ticker.Update(2));
		Assert.Equal("Timer checked", result);

		Assert.Equal(Result.Succeeded, ticker.Update(3));
		Assert.Equal("Timer Completed", result);
	}
}
