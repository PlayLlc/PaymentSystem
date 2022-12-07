using Play.Globalization.Time;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Globalization.Tests.Tests.Time.Stopwatches;

public class StopwatchManagerTests : TestBase
{
    #region Start

    [Fact]
    public void NewStopWatchManager_StartAndStop_ExpectedElapsedTimeWithSecondPrecision()
    {
        StopwatchManager sut = new();

        sut.Start();

        Thread.Sleep(1000);

        Microseconds elapsedTime = sut.Stop();

        Globalization.Time.Seconds expected = new(1);
        Assert.True(expected <= elapsedTime.AsSeconds());
    }

    [Fact]
    public void NewStopWatchManager_StartAndStop_ExpectedElapsedTimeWithDeciSecondPrecision()
    {
        StopwatchManager sut = new();

        sut.Start();

        Thread.Sleep(1000);

        Microseconds elapsedTime = sut.Stop();

        Deciseconds expected = new(10);
        Assert.True(expected <= elapsedTime.AsDeciSeconds());
    }

    [Fact]
    public void NewStopWatchManager_StartAndStop_AlmoastExpectedElapsedTimeWithMilliSecondPrecision()
    {
        StopwatchManager sut = new();

        sut.Start();

        Thread.Sleep(1000);

        Microseconds elapsedTime = sut.Stop();

        Milliseconds expected = new(1000);
        Assert.True(expected <= elapsedTime.AsMilliseconds());
    }

    [Fact]
    public void NewStopWatchManager_StartAndStopThentryToStopAgainTheStopWatch_ExceptionIsThrown()
    {
        StopwatchManager sut = new();

        sut.Start();

        Thread.Sleep(200);

        _ = sut.Stop();

        Assert.Throws<InvalidOperationException>(() => { sut.Stop(); });
    }

    #endregion

    #region GetElapsedTime

    [Fact]
    public void NewStopWatchManager_StartThenGetElapsedTimeThenStop_ReturnsExpectedResultWithSecondPrecision()
    {
        StopwatchManager sut = new();

        sut.Start();

        Thread.Sleep(1000);

        Microseconds elapsed = sut.GetElapsedTime();

        sut.Stop();

        Globalization.Time.Seconds expected = new(1);
        Assert.True(expected <= elapsed.AsSeconds());
    }

    [Fact]
    public void NewStopWatchManager_StartThenGetElapsedTimeThenStop_ReturnsExpectedResultWithDecisecondPrecision()
    {
        StopwatchManager sut = new();

        sut.Start();

        Thread.Sleep(1000);

        Microseconds elapsed = sut.GetElapsedTime();

        sut.Stop();

        Deciseconds expected = new(10);
        Assert.True(expected <= elapsed.AsDeciSeconds());
    }

    [Fact]
    public void NewStopWatchManager_StartThenGetElapsedTimeThenStop_ReturnsExpectedResultWithMillisecondPrecision()
    {
        StopwatchManager sut = new();

        sut.Start();

        Thread.Sleep(1000);

        Microseconds elapsed = sut.GetElapsedTime();
        sut.Stop();

        Milliseconds expected = new(1000);
        Assert.True(expected <= elapsed.AsMilliseconds());
    }

    [Fact]
    public void NewStopWatchManager_StartThenGetElapsedTime2ndTimeThenStop_ReturnsExpectedResultWithSecondPrecision()
    {
        StopwatchManager sut = new();

        sut.Start();

        Thread.Sleep(1000);

        Microseconds elapsed = sut.GetElapsedTime();

        Thread.Sleep(1000);

        Microseconds secondElapsed = sut.GetElapsedTime();

        sut.Stop();

        Globalization.Time.Seconds expected = new(1);
        Assert.True(expected <= elapsed.AsSeconds());

        Globalization.Time.Seconds secondExpected = new(2);
        Assert.True(secondExpected <= secondElapsed.AsSeconds());
    }

    [Fact]
    public void StopWatchManager_StartMultipleTimes_ExceptionIsThrown()
    {
        StopwatchManager sut = new();

        sut.Start();

        Assert.Throws<InvalidOperationException>(() => { sut.Start(); });
    }

    #endregion
}