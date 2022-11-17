using Xunit;
using Hawthorn;

namespace HawthornTest;

public class Sequences
{
	[Fact]
	public void SequencesOccurInImmediateOrder()
	{
		var _ = new BehaviorBuilder<List<string>>();

		var ticker = _.Sequence(
			_.Lambda(a => a.Add("1")),
			_.Lambda(a => a.Add("2")),
			_.Lambda(a => a.Add("3"))
		).ToTree().CreateTestTicker();

		Assert.Equal(Result.Succeeded, ticker.Update(1));
		Assert.Equal(new [] { "1", "2", "3" }, ticker.Actor);

		Assert.Equal(Result.Succeeded, ticker.Update(1));
		Assert.Equal(new [] { "1", "2", "3", "1", "2", "3" }, ticker.Actor);
	}

	[Fact]
	public void StatefulSequencesCanOccurInImmediateOrder()
	{
		var _ = new BehaviorBuilder<List<string>>();

		var ticker = _.StatefulSequence(
			_.Lambda(a => a.Add("1")),
			_.Lambda(a => a.Add("2")),
			_.Lambda(a => a.Add("3"))
		).ToTree().CreateTestTicker();

		Assert.Equal(Result.Succeeded, ticker.Update(1));
		Assert.Equal(new [] { "1", "2", "3" }, ticker.Actor);

		Assert.Equal(Result.Succeeded, ticker.Update(1));
		Assert.Equal(new [] { "1", "2", "3", "1", "2", "3" }, ticker.Actor);
	}

	[Fact]
	public void SequencesRestartOnBusy()
	{
		var _  = new BehaviorBuilder<List<string>>();

		var ticker = _.Sequence(
			_.Lambda(a => a.Add("1")),
			_.Lambda(a => a.Add("2")),
			_.Lambda(a => Result.Busy)
		).ToTree().CreateTestTicker();

		Assert.Equal(Result.Busy, ticker.Update(1));
		Assert.Equal(new [] { "1", "2" }, ticker.Actor);

		Assert.Equal(Result.Busy, ticker.Update(1));
		Assert.Equal(new [] { "1", "2", "1", "2" }, ticker.Actor);
	}

	[Fact]
	public void StatefulSequencesContinueOnBusy()
	{
		var _  = new BehaviorBuilder<List<string>>();

		var ticker = _.StatefulSequence(
			_.Lambda(a => a.Add("1")),
			_.Lambda(a => a.Add("2")),
			_.Lambda(a => {
				a.Add("3");
				return Result.Busy;
			})
		).ToTree().CreateTestTicker();

		Assert.Equal(Result.Busy, ticker.Update(1));
		Assert.Equal(new [] { "1", "2", "3" }, ticker.Actor);

		Assert.Equal(Result.Busy, ticker.Update(1));
		Assert.Equal(new [] { "1", "2", "3", "3" }, ticker.Actor);
	}

	[Fact]
	public void StatefulSequencesRestartOnFailure()
	{
		var _  = new BehaviorBuilder<List<string>>();

		var ticker = _.StatefulSequence(
			_.Lambda(a => a.Add("1")),
			_.Lambda(a => a.Add("2")),
			_.Lambda(a => {
				a.Add("3");
				return Result.Failed;
			})
		).ToTree().CreateTestTicker();

		Assert.Equal(Result.Failed, ticker.Update(1));
		Assert.Equal(new [] { "1", "2", "3" }, ticker.Actor);

		Assert.Equal(Result.Failed, ticker.Update(1));
		Assert.Equal(new [] { "1", "2", "3", "1", "2", "3" }, ticker.Actor);
	}
}
