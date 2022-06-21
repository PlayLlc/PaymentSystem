using System.Security.Cryptography.X509Certificates;

using Play.Globalization.Time;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Globalization.Tests.Time.Seconds;

public class DecisecondsTests : TestBase
{
    #region Instance Members

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
        Globalization.Time.Seconds testData = new Seconds(0);
        Deciseconds sut = new(testData);
        Assert.NotNull(sut);
    }

    #endregion
}