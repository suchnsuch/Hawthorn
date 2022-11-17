using Xunit;
using BehaviorTrees;

namespace BehaviorTreesTest;

public class Parallel
{
	[Fact]
	public void ParallelRunsAll()
	{
		var _ = new BehaviorBuilder<List<string>>();

		var ticker = _.Parallel(
			_.Lambda(a => {
				a.Add("1");
				return Result.Failed;
			}),
			_.Lambda(a => {
				a.Add("2");
				return Result.Busy;
			}),
			_.Lambda(a => {
				a.Add("3");
				return Result.Succeeded;
			})
		).ToTree().CreateTestTicker();

		Assert.Equal(Result.Failed, ticker.Update(1));
		Assert.Equal(new [] { "1", "2", "3" }, ticker.Actor);
	}

	[Fact]
	public void ParallelReturnsWorst()
	{
		var _ = new BehaviorBuilder<List<string>>();

		var ticker = _.Parallel(
			_.Lambda(a => {
				a.Add("1");
				return Result.Busy;
			}),
			_.Lambda(a => {
				a.Add("2");
				return Result.Succeeded;
			}),
			_.Lambda(a => {
				a.Add("3");
				return Result.Succeeded;
			})
		).ToTree().CreateTestTicker();

		Assert.Equal(Result.Busy, ticker.Update(1));
		Assert.Equal(new [] { "1", "2", "3" }, ticker.Actor);
	}
}
