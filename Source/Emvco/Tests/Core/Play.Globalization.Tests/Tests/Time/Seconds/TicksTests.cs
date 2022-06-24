using Play.Globalization.Time.Seconds;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Globalization.Tests.Tests.Time.Seconds;

public class TicksTests : TestBase
{
    [Fact]
    public void Ticks_CastingToSeconds_ReturnsExpectedResult()
    {
        long ticks = 10000000;
        Ticks sut = new Ticks(ticks);

        Globalization.Time.Seconds.Seconds expected = new(1);
        Globalization.Time.Seconds.Seconds actual = sut.AsSeconds();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Ticks_CastingToDeciSeconds_ReturnsExpectedResult()
    {
        long ticks = 100000;
        Ticks sut = new Ticks(ticks);

        Deciseconds expected = new(1);
        Deciseconds actual = sut.AsDeciSeconds();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Ticks_CastingToMilliSeconds_ReturnsExpectedResult()
    {
        long ticks = 100000;
        Ticks sut = new Ticks(ticks);

        Milliseconds expected = new(1);
        Milliseconds actual = sut.AsMilliSeconds();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Ticks_CastingToMicroSeconds_ReturnsExpectedResult()
    {
        long ticks = 1000;
        Ticks sut = new Ticks(ticks);

        Microseconds expected = new(1);
        Microseconds actual = sut.AsMicroSeconds();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Ticks_Equals_ReturnsExpectedResult()
    {
        long ticks = 1000;
        Ticks sut = new Ticks(ticks);
        Ticks expected = new Ticks(1000);

        Assertion(() => Assert.Equal(expected, sut));
        Assertion(() => Assert.True(expected.Equals(ticks)));
        Assertion(() => Assert.True(Ticks.Equals(expected, sut)));
    }

    [Fact]
    public void Ticks_CompareOperators_ReturnsExpectedResults()
    {
        Ticks input = new Ticks((long)61233123);
        Ticks other = new Ticks((long)6100123);

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

    [Fact]
    public void Ticks_MathOperators_ReturnsExpectedResults()
    {
        Ticks input = new Ticks((long)15);
        Ticks other = new Ticks((long)10);

        Ticks multiply = input * other;
        Ticks expected = new Ticks(15 * 10);
        Assertion(() => Assert.Equal(expected, multiply));

        Ticks addition = input + other;
        expected = new Ticks(15 + 10);
        Assertion(() => Assert.Equal(expected, addition));

        Ticks substraction = input - other;
        expected = new Ticks(15 - 10);
        Assertion(() => Assert.Equal(expected, substraction));

        Ticks division = input / other;
        expected = new Ticks(15 / 10);
        Assertion(() => Assert.Equal(expected, division));
    }
}
