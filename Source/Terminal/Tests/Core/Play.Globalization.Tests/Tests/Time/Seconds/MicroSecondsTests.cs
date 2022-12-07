using Play.Globalization.Time;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Globalization.Tests.Tests.Time.Seconds;

public class MicroSecondsTests : TestBase
{
    [Fact]
    public void MicroSecond_PublicStaticZero_CorrectValueInstantiated()
    {
        Microseconds zero = Microseconds.Zero;

        Assertion(() => Assert.Equal(0, zero));
    }

    [Fact]
    public void Microsecond_InstantiateFromLong_CorrectlyInstantiated()
    {
        long input = 26313425;
        Microseconds dut = new(input);

        Assertion(() => Assert.True(dut.Equals(input)));
    }

    [Fact]
    public void Microsecond_InstantiateFromUInt_CorrectlyInstantiated()
    {
        uint input = 26313425;
        Microseconds dut = new(input);

        Assertion(() => Assert.True(dut.Equals(input)));
    }

    [Fact]
    public void Microsecond_InstantiateFromUShort_CorrectlyInstantiated()
    {
        ushort input = 26313;
        Microseconds dut = new(input);

        Assertion(() => Assert.True(dut == (long) input));
    }

    [Fact]
    public void Microsecond_InstantiateFromByte_CorrectlyInstantiated()
    {
        byte input = 240;
        Microseconds dut = new(input);

        Assertion(() => Assert.True(dut == (long) input));
    }

    [Fact]
    public void Microsecond_InstantiateFromTimeSpanPrecisionOf10TicksPerMicrosecond_CorrectlyInstantiated()
    {
        long input = 6123123;
        TimeSpan timeSpan = new(input);
        Microseconds dut = new(timeSpan);

        TimeSpan expected = new(6123120);
        Microseconds other = new(expected);

        Assertion(() => Assert.Equal(expected, dut.AsTimeSpan()));
        Assertion(() => Assert.Equal(other, dut));
    }

    [Fact]
    public void Microsecond_CompareOperators()
    {
        TimeSpan timeSpan = new(61233123);
        Microseconds input = new(timeSpan);

        TimeSpan secondTimeSpan = new(6100123);
        Microseconds other = new(secondTimeSpan);

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
    public void Microsecond_MathOperators()
    {
        TimeSpan timeSpan = new(123);
        Microseconds input = new(timeSpan);

        TimeSpan secondTimeSpan = new(28);
        Microseconds other = new(secondTimeSpan);

        Microseconds multiply = new(12 * 2);
        Microseconds actual = input * other;

        Assertion(() => { Assert.Equal(multiply, actual); }, Build.Equals.Message(multiply, actual));
    }

    [Fact]
    public void Microsecond_CastingToTicks_ReturnsExpectedResult()
    {
        //10 ticks per microsecond
        Microseconds sut = new(1);

        Ticks expected = new((uint) 10);
        Ticks actual = sut.AsTicks();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Microsecond_CastingToSeconds_ReturnsExpectedResults()
    {
        long microseconds = 1000000;
        Microseconds sut = new(microseconds);

        Globalization.Time.Seconds expected = new(1);
        Globalization.Time.Seconds actual = sut.AsSeconds();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Microsecond_CastingToDeciseconds_ReturnsExpectedResult()
    {
        long microseconds = 100000;
        Microseconds sut = new(microseconds);

        Deciseconds expected = new(1);
        Deciseconds actual = sut.AsDeciSeconds();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Microsecond_CastingToMiliseconds_ReturnsExpectedResult()
    {
        long microseconds = 1000;
        Microseconds sut = new(microseconds);
        Milliseconds expected = new(1);
        Milliseconds actual = sut.AsMilliseconds();

        Assertion(() => { Assert.Equal(expected, actual); }, Build.Equals.Message(expected, actual));
    }
}