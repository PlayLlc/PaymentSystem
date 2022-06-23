using Play.Core.Exceptions;
using Play.Core.Tests.Data.Fixtures.Custom;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Tests.Customs;
public class SequenceCounterThresholdTests : TestBase
{
    #region Instantiation

    [Fact]
    public void SequenceCounterThreshHold_InstantiateWithValidParameters_CounterIsInitialized()
    {
        SequenceCounterThreshold sequenceCounterThreshold = new SequenceCounterThreshold(0, 10, 1);

        Assertion(() =>
        {
            Assert.NotNull(sequenceCounterThreshold);
            Assert.Equal(0, sequenceCounterThreshold.GetSequenceValue());
        });
    }

    [Fact]
    public void SequenceCounterThreshold_InstantiateWithInvalidParameters_PlayInternalExceptionIsThrown()
    {
        Assertion(() =>
        {
            Assert.Throws<PlayInternalException>(() =>
            {
                SequenceCounterThreshold sequenceCounterThreshold = new SequenceCounterThreshold(10, 0, 1);
            });
        });
    }

    #endregion

    #region Increment

    [Fact]
    public void SequenceCounterTreshold_IncrementBy1_ReturnsExpectedResult()
    {
        SequenceCounterThreshold sequenceCounterThreshold = new SequenceCounterThreshold(0, 10, 1);

        Assertion(() => Assert.Equal(0, sequenceCounterThreshold.GetSequenceValue()));

        sequenceCounterThreshold.Increment();

        Assertion(() => Assert.Equal(1, sequenceCounterThreshold.GetSequenceValue()));
    }

    [Fact]
    public void SequenceCounterTreshold_IncrementBy2_ReturnsExpectedResult()
    {
        SequenceCounterThreshold sequenceCounterThreshold = new SequenceCounterThreshold(0, 10, 2);

        Assertion(() => Assert.Equal(0, sequenceCounterThreshold.GetSequenceValue()));

        sequenceCounterThreshold.Increment();

        Assertion(() => Assert.Equal(2, sequenceCounterThreshold.GetSequenceValue()));
    }

    [Theory]
    [MemberData(nameof(SequenceCounterFixture.SequenceCounterTreshold.GetRandom), 50, MemberType = typeof(SequenceCounterFixture.SequenceCounterTreshold))]
    public void RandomSequenceCounterTreshold_InvokesIncrement_ReturnsExpectedResult(int minValue, int maxValue, int increment)
    {
        Assert.True(minValue < maxValue);

        SequenceCounterThreshold sequenceCounterThreshold = new SequenceCounterThreshold(minValue, maxValue, increment);

        Assertion(() =>
        {
            Assert.Equal(minValue, sequenceCounterThreshold.GetSequenceValue());
            sequenceCounterThreshold.Increment();
            Assert.True(minValue + increment >= sequenceCounterThreshold.GetSequenceValue());
        });
    }

    [Fact]
    public void SequenceCounterTreshold_Increment_SequenceValueNeverGoesOverMaxValue()
    {
        int min = 1;
        int max = 10;
        int increment = 4;

        SequenceCounterThreshold sequenceCounterThreshold = new SequenceCounterThreshold(min, max, increment);

        int counter = (max / increment) + 1;

        for (int i = 0; i < counter; i++)
        {
            sequenceCounterThreshold.Increment();
        }

        Assertion(() => Assert.Equal(max, sequenceCounterThreshold.GetSequenceValue()));
    }

    #endregion

    #region Reset

    [Fact]
    public void SequenceCounterTreshold_IncrementThenReset_ReturnsExpectedResult()
    {
        SequenceCounterThreshold sequenceCounterThreshold = new SequenceCounterThreshold(0, 10, 1);

        Assertion(() => Assert.Equal(0, sequenceCounterThreshold.GetSequenceValue()));

        sequenceCounterThreshold.Increment();

        Assertion(() => Assert.Equal(1, sequenceCounterThreshold.GetSequenceValue()));

        sequenceCounterThreshold.Reset();

        Assertion(() => Assert.Equal(0, sequenceCounterThreshold.GetSequenceValue()));
    }

    [Theory]
    [MemberData(nameof(SequenceCounterFixture.SequenceCounterTreshold.GetRandom), 50, MemberType = typeof(SequenceCounterFixture.SequenceCounterTreshold))]
    public void RandomSequenceCounterTreshold_InvokesIncrementThenReset_ReturnsExpectedResult(int minValue, int maxValue, int increment)
    {
        Assert.True(minValue < maxValue);

        SequenceCounterThreshold sequenceCounterThreshold = new SequenceCounterThreshold(minValue, maxValue, increment);

        Assertion(() => Assert.Equal(minValue, sequenceCounterThreshold.GetSequenceValue()));

        sequenceCounterThreshold.Increment();
        sequenceCounterThreshold.Reset();

        Assertion(() => Assert.Equal(minValue, sequenceCounterThreshold.GetSequenceValue()));
    }

    #endregion
}
