
using Play.Globalization.Time;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Globalization.Tests.Tests.Time;

public class DecisecondsTests : TestBase
{
    #region Instance Members

    [Fact]
    public void TwoDecisec_CastingToSeconds_ReturnsExpectedResult()
    {
        Deciseconds testData1 = new(1345);
        Deciseconds testData2 = new(1345);
    }

    #endregion

    #region Deciseconds to Ticks

    [Fact]
    public void Decisecond1_CastingToSeconds_ReturnsExpectedResult()
    {
        Deciseconds sut = new(1);
        Ticks expected = new((uint)1000000);
        Ticks actual = (Ticks)sut;
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Decisecond100_CastingToSeconds_ReturnsExpectedResult()
    {
        Deciseconds sut = new(100);
        Ticks expected = new((uint)100000000);
        Ticks actual = sut.AsTicks();
        Assert.Equal(expected, actual);
    }

    #endregion

    #region Deciseconds to Timespan

    [Fact]
    public void Decisecond1_CastingToTimeSpan_ReturnsExpectedResult()
    {
        Deciseconds sut = new(1);
        TimeSpan expected = new(1000000);
        TimeSpan actual = (TimeSpan)sut;
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Decisecond100_CastingToTimeSpan_ReturnsExpectedResult()
    {
        Deciseconds sut = new(100);
        TimeSpan expected = new(100000000);
        TimeSpan actual = sut.AsTimeSpan();
        Assert.Equal(expected, actual);
    }

    #endregion

    #region Deciseconds to Seconds

    [Fact]
    public void Decisecond_CastingToSeconds_ReturnsExpectedResult()
    {
        Deciseconds sut = new(10);
        Globalization.Time.Seconds expected = new(1);
        Globalization.Time.Seconds actual = sut.AsSeconds();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Decisecond1000_CastingToSeconds_ReturnsExpectedResult()
    {
        Deciseconds sut = new(10000);
        Globalization.Time.Seconds expected = new(1000);
        Globalization.Time.Seconds actual = sut.AsSeconds();
        Assert.Equal(expected, actual);
    }

    #endregion

    #region Initialization

    [Fact]
    public void PositiveInteger_InitializingDecisecond_ReturnsDecisecondInstance()
    {
        int testData = 12345;
        Deciseconds sut = new(testData);
        Assert.NotNull(sut);
    }

    [Fact]
    public void NegativeInteger_InitializingDecisecond_ReturnsDecisecondInstance()
    {
        int testData = -12345;
        Deciseconds sut = new(testData);
        Assert.NotNull(sut);
    }

    [Fact]
    public void Zero_InitializingDecisecond_ReturnsDecisecondInstance()
    {
        int testData = 0;
        Deciseconds sut = new(testData);
        Assert.NotNull(sut);
    }

    [Fact]
    public void Seconds_InitializingDecisecond_ReturnsDecisecondInstance()
    {
        Globalization.Time.Seconds testData = new(0);
        Deciseconds sut = new(testData);
        Assert.NotNull(sut);
    }

    #endregion
}