using System;

using Play.Globalization.Time.Seconds;
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
        Microseconds dut = new Microseconds(input);

        Assertion(() => Assert.True(dut.Equals(input)));
    }

    [Fact]
    public void Microsecond_InstantiateFromUInt_CorrectlyInstantiated()
    {
        uint input = 26313425;
        Microseconds dut = new Microseconds(input);

        Assertion(() => Assert.True(dut.Equals(input)));
    }

    [Fact]
    public void Microsecond_InstantiateFromUShort_CorrectlyInstantiated()
    {
        ushort input = 26313;
        Microseconds dut = new Microseconds(input);

        Assertion(() => Assert.True(dut == (long)input));
    }

    [Fact]
    public void Microsecond_InstantiateFromByte_CorrectlyInstantiated()
    {
        byte input = 240;
        Microseconds dut = new Microseconds(input);

        Assertion(() => Assert.True(dut == (long)input));
    }

    [Fact]
    public void Microsecond_InstantiateFromTimeSpanPrecisionOf10TicksPerMicrosecond_CorrectlyInstantiated()
    {
        long input = 6123123;
        TimeSpan timeSpan = new TimeSpan(input);
        Microseconds dut = new Microseconds(timeSpan);

        TimeSpan expected = new TimeSpan(6123120);
        Microseconds other = new Microseconds(expected);

        Assertion(() => Assert.Equal(expected, dut.AsTimeSpan()));
        Assertion(() => Assert.Equal(other, dut));
    }

    [Fact]
    public void Microsecond_CompareOperators()
    {
        TimeSpan timeSpan = new TimeSpan(61233123);
        Microseconds input = new Microseconds(timeSpan);

        TimeSpan secondTimeSpan = new TimeSpan(6100123);
        Microseconds other = new Microseconds(secondTimeSpan);

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
        TimeSpan timeSpan = new TimeSpan(123);
        Microseconds input = new Microseconds(timeSpan);

        TimeSpan secondTimeSpan = new TimeSpan(28);
        Microseconds other = new Microseconds(secondTimeSpan);

        Microseconds multiply = new Microseconds(12 * 2);
        Microseconds actual = input * other;

        Assertion(() =>
        {
            Assert.Equal(multiply, actual);
        }, Build.Equals.Message(multiply, actual));
    }
}
