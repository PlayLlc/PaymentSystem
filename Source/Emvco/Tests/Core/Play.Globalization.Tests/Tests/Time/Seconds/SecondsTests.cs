
using Play.Globalization.Time;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Globalization.Tests.Tests.Time.Seconds;
public class SecondsTests : TestBase
{
    [Fact]
    public void Seconds_CastingToTicks_ReturnsExpectedResult()
    {
        //10mil  ticks per microsecond
        Globalization.Time.Seconds sut = new(1);

        Ticks expected = new((uint)10000000);
        Ticks actual = sut.AsTicks();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Seconds_CastingToMicroseconds_ReturnsExpectedResults()
    {
        long seconds = 1;
        Globalization.Time.Seconds sut = new(seconds);

        Microseconds expected = new((long)1000000);
        Microseconds actual = sut.AsMicroseconds();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Seconds_CastingToDeciseconds_ReturnsExpectedResult()
    {
        long seconds = 1;
        Globalization.Time.Seconds sut = new(seconds);

        Deciseconds expected = new(10);
        Deciseconds actual = sut.AsDeciSeconds();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Seconds_CastingToMiliseconds_ReturnsExpectedResult()
    {
        long seconds = 1;
        Globalization.Time.Seconds sut = new(seconds);

        Milliseconds expected = new(1000);
        Milliseconds actual = sut.AsMilliseconds();

        Assertion(() =>
        {
            Assert.Equal(expected, actual);
        });
    }

    [Fact]
    public void Seconds_PublicStaticZero_CorrectValueInstantiated()
    {
        Globalization.Time.Seconds zero = Globalization.Time.Seconds.Zero;

        Assertion(() => Assert.Equal(0, (long)zero));
    }

    [Fact]
    public void Seconds_InstantiateFromLong_CorrectlyInstantiated()
    {
        long input = 26313425;
        Globalization.Time.Seconds sut = new Globalization.Time.Seconds(input);

        Assertion(() => Assert.Equal(input, (long)sut));
    }

    [Fact]
    public void Seconds_InstantiateFromInt_CorrectlyInstantiated()
    {
        int input = 26313425;
        Globalization.Time.Seconds sut = new Globalization.Time.Seconds(input);

        Assertion(() => Assert.Equal(input, (long)sut));
    }

    [Fact]
    public void Seconds_InstantiateFromByte_CorrectlyInstantiated()
    {
        byte input = 240;
        Globalization.Time.Seconds sut = new Globalization.Time.Seconds(input);

        Assertion(() => Assert.Equal(input, (long)sut));
    }

    [Fact]
    public void Microsecond_CompareOperators()
    {
        Globalization.Time.Seconds input = new Globalization.Time.Seconds(61233123);
        Globalization.Time.Seconds other = new Globalization.Time.Seconds(6100123);

        Assertion(() =>
        {
            Assert.True(input > other);
            Assert.False(input == other);
            Assert.True(input != other);
            Assert.True(other < input);
            Assert.True(input >= other);
            Assert.True(other <= input);
            Assert.False(input <= other);
        });
    }
}
