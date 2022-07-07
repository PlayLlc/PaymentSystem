using Play.Globalization.Time;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Globalization.Tests.Tests.Time.Timers;

public class TimeoutManagerTests : TestBase
{
    #region Instance Members

    [Fact]
    public void NewTimeoutManager_StartAndStop_ExpectedElapsedTimeWithSecondPrecision()
    {
        TimeoutManager sut = new();

        Globalization.Time.Seconds timeout = new(2);

        sut.Start(timeout);

        Thread.Sleep(2000);

        Microseconds elapsed = sut.Stop();

        Assert.True(timeout <= elapsed.AsSeconds());
    }

    [Fact]
    public void NewTimeoutManager_StartAndStop_ExpectedElapsedTimeWithDeciSecondPrecision()
    {
        TimeoutManager sut = new();

        Globalization.Time.Seconds timeout = new(2);

        sut.Start(timeout);

        Thread.Sleep(2000);

        Microseconds elapsed = sut.Stop();

        Deciseconds expected = new(20);
        Assert.True(expected <= elapsed.AsDeciSeconds());
    }

    [Fact]
    public void NewTimeOutManager_StartTOMThenStartItAgain_ExceptionIsThrown()
    {
        TimeoutManager sut = new();

        Globalization.Time.Seconds timeout = new(2);

        sut.Start(timeout);

        Assert.Throws<InvalidOperationException>(() => { sut.Start(timeout); });
    }

    #endregion

    #region IsTimeOut

    [Fact]
    public void NewTimeOutManager_TOMStarted_IsNotTimedOut()
    {
        TimeoutManager sut = new();

        Globalization.Time.Seconds timeout = new(3);

        sut.Start(timeout);

        Thread.Sleep(2000);

        Assert.False(sut.IsTimedOut());
    }

    [Fact]
    public void NewTimeOutManager_TOMStarted_IsTimedOut()
    {
        TimeoutManager sut = new();

        Globalization.Time.Seconds timeout = new(1);

        sut.Start(timeout);

        Thread.Sleep(3000);

        Assert.True(sut.IsTimedOut());
    }

    #endregion

    #region TimeOut with Action Handler

    [Fact]
    public void NewTimeOutManager_TOMStartedWithActionHandler_ActionHandlerGetsTriggeredAfterTimeoutIsReached()
    {
        TimeoutManager sut = new();

        Globalization.Time.Seconds timeout = new(1);

        int j = 0;

        sut.Start(timeout, () => { j = 11; });

        Thread.Sleep(3000);
        Assert.Equal(11, j);
    }

    [Fact]
    public void NewTimeOutManager_TryingToStopTOMWhenTimeoutHasReached_ThrowsException()
    {
        TimeoutManager sut = new();

        Globalization.Time.Seconds timeout = new(1);

        sut.Start(timeout);

        Thread.Sleep(3000);
        Assert.Throws<InvalidOperationException>(() => { sut.Stop(); });
    }

    #endregion
}