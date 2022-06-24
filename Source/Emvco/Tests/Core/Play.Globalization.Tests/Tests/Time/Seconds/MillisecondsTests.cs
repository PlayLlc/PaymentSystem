using System;

using Play.Globalization.Time.Seconds;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Globalization.Tests.Tests.Time.Seconds;

public class MillisecondsTests : TestBase
{
    [Fact]
    public void Milliseconds_CastingToTicks_ReturnsExpectedResult()
    {
        //10000 ticks per millisecond
        Milliseconds sut = new(1);

        Ticks expected = new((uint)10000);
        Ticks actual = sut.AsTicks();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Milliseconds_CastingToSeconds_ReturnsExpectedResults()
    {
        long milliseconds = 1000;
        Milliseconds sut = new(milliseconds);

        Globalization.Time.Seconds.Seconds expected = new(1);
        Globalization.Time.Seconds.Seconds actual = sut.AsSeconds();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Milliseconds_CastingToDeciseconds_ReturnsExpectedResult()
    {
        long milliseconds = 100;
        Milliseconds sut = new(milliseconds);

        Deciseconds expected = new(1);
        Deciseconds actual = sut.AsDeciSeconds();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Milliseconds_CastingToMicroseconds_ReturnsExpectedResult()
    {
        long milliseconds = 1;
        Milliseconds sut = new(milliseconds);
        Microseconds expected = new(1000);
        Microseconds actual = sut.AsMicroseconds();

        Assertion(() =>
        {
            Assert.Equal(expected, actual);
        });
    }

    [Fact]
    public void Milliseconds_PublicStaticZero_CorrectValueInstantiated()
    {
        Milliseconds zero = Milliseconds.Zero;

        Assertion(() => Assert.Equal(0, zero));
    }

    [Fact]
    public void Milliseconds_InstantiateFromLong_CorrectlyInstantiated()
    {
        long input = 26313425;
        Milliseconds dut = new Milliseconds(input);

        Assertion(() => Assert.True(dut.Equals(input)));
    }

    [Fact]
    public void Milliseconds_InstantiateFromUInt_CorrectlyInstantiated()
    {
        uint input = 26313425;
        Milliseconds dut = new Milliseconds(input);

        Assertion(() => Assert.True(dut.Equals(input)));
    }

    [Fact]
    public void Milliseconds_InstantiateFromUShort_CorrectlyInstantiated()
    {
        ushort input = 26313;
        Milliseconds dut = new Milliseconds(input);

        Assertion(() => Assert.True(dut == (long)input));
    }

    [Fact]
    public void Milliseconds_InstantiateFromByte_CorrectlyInstantiated()
    {
        byte input = 240;
        Milliseconds dut = new Milliseconds(input);

        Assertion(() => Assert.True(dut == (long)input));
    }

    [Fact]
    public void Milliseconds_CompareOperators()
    {
        Milliseconds input = new Milliseconds((long)61233123);
        Milliseconds other = new Milliseconds((long)6100123);

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
    public void Milliseconds_MathOperators()
    {
        TimeSpan timeSpan = new TimeSpan(20000);
        Milliseconds input = new Milliseconds(timeSpan);

        TimeSpan secondTimeSpan = new TimeSpan(40000);
        Milliseconds other = new Milliseconds(secondTimeSpan);

        Milliseconds multiply = new Milliseconds(2 * 4);
        Milliseconds actual = input * other;

        Assertion(() =>
        {
            Assert.Equal(multiply, actual);
        }, Build.Equals.Message(multiply, actual));
    }
}
