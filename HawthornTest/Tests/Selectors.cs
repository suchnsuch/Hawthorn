using Xunit;
using Hawthorn;

namespace HawthornTest;

public class Selectors
{
	[Fact]
	public void SelectorsStopOnFirstSuccess()
	{
		var _ = new BehaviorBuilder<object>();

		var reached = 0;
		var target = 1;

		var ticker = _.Selector(
			_.Lambda(a => target == (reached = 1)),
			_.Lambda(a => target == (reached = 2)),
			_.Lambda(a => target == (reached = 3))
		).ToTree().CreateTestTicker();

		Assert.Equal(Result.Succeeded, ticker.Update(1));
		Assert.Equal(1, reached);

		reached = 0;
		target = 3;

		Assert.Equal(Result.Succeeded, ticker.Update(1));
		Assert.Equal(3, reached);
	}

	[Fact]
	public void SelectorsStopOnFirstBusy()
	{
		var _ = new BehaviorBuilder<object>();

		var reached = 0;
		var target = 1;

		var ticker = _.Selector(
			_.Lambda(a => (target == (reached = 1)).ToBusyOrFailed()),
			_.Lambda(a => (target == (reached = 2)).ToBusyOrFailed()),
			_.Lambda(a => (target == (reached = 3)).ToBusyOrFailed())
		).ToTree().CreateTestTicker();

		Assert.Equal(Result.Busy, ticker.Update(1));
		Assert.Equal(1, reached);

		reached = 0;
		target = 3;

		Assert.Equal(Result.Busy, ticker.Update(1));
		Assert.Equal(3, reached);
	}

	[Fact]
	public void StatefulSelectorsStopOnFirstSuccess()
	{
		var _ = new BehaviorBuilder<object>();

		var reached = 0;
		var target = 1;

		var ticker = _.StatefulSelector(
			_.Lambda(a => target == (reached = 1)),
			_.Lambda(a => target == (reached = 2)),
			_.Lambda(a => target == (reached = 3))
		).ToTree().CreateTestTicker();

		Assert.Equal(Result.Succeeded, ticker.Update(1));
		Assert.Equal(1, reached);

		reached = 0;
		target = 3;

		Assert.Equal(Result.Succeeded, ticker.Update(1));
		Assert.Equal(3, reached);
	}

	[Fact]
	public void SelectorsRestartOnBusy()
	{
		var _ = new BehaviorBuilder<List<string>>();

		var ticker = _.Selector(
			_.Lambda(a => {
				a.Add("1");
				return Result.Failed;
			}),
			_.Lambda(a => {
				a.Add("2");
				return Result.Busy;
			})
		).ToTree().CreateTestTicker();

		Assert.Equal(Result.Busy, ticker.Update(1));
		Assert.Equal(new [] { "1", "2" }, ticker.Actor);

		Assert.Equal(Result.Busy, ticker.Update(1));
		Assert.Equal(new [] { "1", "2", "1", "2" }, ticker.Actor);
	}

	[Fact]
	public void StatefulSelectorsResumeOnBusy()
	{
		var _ = new BehaviorBuilder<List<string>>();

		var ticker = _.StatefulSelector(
			_.Lambda(a => {
				a.Add("1");
				return Result.Failed;
			}),
			_.Lambda(a => {
				a.Add("2");
				return Result.Busy;
			})
		).ToTree().CreateTestTicker();

		Assert.Equal(Result.Busy, ticker.Update(1));
		Assert.Equal(new [] { "1", "2" }, ticker.Actor);

		Assert.Equal(Result.Busy, ticker.Update(1));
		Assert.Equal(new [] { "1", "2", "2" }, ticker.Actor);
	}

	[Fact]
	public void StatefulSelectorRestartsOnFailure()
	{
		var _ = new BehaviorBuilder<List<string>>();

		Result secondResult = Result.Busy;

		var ticker = _.StatefulSelector(
			_.Lambda(a => {
				a.Add("1");
				return Result.Failed;
			}),
			_.Lambda(a => {
				a.Add("2");
				return secondResult;
			})
		).ToTree().CreateTestTicker();

		Assert.Equal(Result.Busy, ticker.Update(1));
		Assert.Equal(new [] { "1", "2" }, ticker.Actor);

		secondResult = Result.Failed;

		ticker.Update(1);
		Assert.Equal(new [] { "1", "2", "2" }, ticker.Actor);

		ticker.Update(1);
		Assert.Equal(new [] { "1", "2", "2", "1", "2" }, ticker.Actor);
	}
}
