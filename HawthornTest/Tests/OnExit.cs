using Xunit;
using Hawthorn;

namespace HawthornTest;

public class OnExitTests
{
	[Fact]
	public void OnExitIsCalledWhenChildDrops()
	{
		var _ = new BehaviorBuilder<List<string>>();

		Result targetResult = Result.Failed;
		bool calledExit = false;
		
		var ticker = _.Selector(
			_.Lambda(a => {
				return targetResult;
			}).OnExit(t => calledExit = true),
			_.Lambda(a => true)
		).ToTree().CreateTestTicker();

		Assert.Equal(Result.Succeeded, ticker.Update(1));
		Assert.False(calledExit);

		targetResult = Result.Busy;

		Assert.Equal(Result.Busy, ticker.Update(1));
		Assert.False(calledExit);

		targetResult = Result.Failed;

		ticker.Update(1);

		Assert.True(calledExit);
	}

	[Fact]
	public void OnExitDoesNotMessUpDebugPath()
	{
		/*
		 * This exists to catch a bug where OnExit was not correctly propegating depth to its child
		 */
		var _ = new BehaviorBuilder<object>();

		var ticker = _.Selector(
			_.Sequence("Sequence A",
				_.Lambda(_ => false)
			),
			_.Sequence("Sequence B",
				_.Lambda(_ => false)
			).OnExit(_ => {}),
			_.Sequence("Sequence C")
		).ToTree().CreateTestTicker();

		ticker.Update(1);

		var path = ticker.GetLastDebugPath();
		Assert.Equal(new [] { "", "Sequence C" }, path);
	}
}
