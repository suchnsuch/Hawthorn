using Xunit;
using Hawthorn;

namespace HawthornTest;

public class Results
{
	[Fact]
	public void SucceededIsBetterThanBusyAndFailed()
	{
		Assert.Equal(Result.Succeeded, Result.Succeeded.Best(Result.Busy));
		Assert.Equal(Result.Succeeded, Result.Succeeded.Best(Result.Failed));

		Assert.Equal(Result.Succeeded, Result.Busy.Best(Result.Succeeded));
		Assert.Equal(Result.Succeeded, Result.Failed.Best(Result.Succeeded));
	}

	[Fact]
	public void BusyIsBetterThanFailed()
	{
		Assert.Equal(Result.Busy, Result.Busy.Best(Result.Failed));
		Assert.Equal(Result.Busy, Result.Failed.Best(Result.Busy));
	}

	[Fact]
	public void FailedIsWorseThanBusyAndFailed()
	{
		Assert.Equal(Result.Failed, Result.Failed.Worst(Result.Busy));
		Assert.Equal(Result.Failed, Result.Failed.Worst(Result.Succeeded));

		Assert.Equal(Result.Failed, Result.Succeeded.Worst(Result.Failed));
		Assert.Equal(Result.Failed, Result.Busy.Worst(Result.Failed));
	}

	[Fact]
	public void BusyIsWorseThanSucceeded()
	{
		Assert.Equal(Result.Busy, Result.Busy.Worst(Result.Succeeded));
		Assert.Equal(Result.Busy, Result.Succeeded.Worst(Result.Busy));
	}
}
