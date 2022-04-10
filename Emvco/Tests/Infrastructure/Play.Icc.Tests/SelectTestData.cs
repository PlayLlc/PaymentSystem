using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

using Play.Icc.FileSystem.DedicatedFiles;
using Play.Icc.Messaging.Apdu.SelectFile;
using Play.Testing.Infrastructure.BaseTestClasses;
using Play.Testing.Infrastructure.Icc.Apdu;

using Xunit;

namespace Play.Icc.Tests;

public class SelectTestData : TestBase
{
    #region Instance Members

    [Fact]
    public void DedicatedFileName1_CreatingSelectCommand_ReturnsActual()
    {
        DedicatedFileName dedicatedFileName = new(ApduTestData.CApdu.Select.Applet1.DedicatedFileName);
        byte[] expected = SelectApduCommand.DedicatedFile(dedicatedFileName).Serialize();
        byte[] actual = ApduTestData.CApdu.Select.Applet1.CApdu;
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void DedicatedFileName2_CreatingSelectCommand_ReturnsActual()
    {
        DedicatedFileName dedicatedFileName = new(ApduTestData.CApdu.Select.Applet2.DedicatedFileName);
        byte[] expected = SelectApduCommand.DedicatedFile(dedicatedFileName).Serialize();
        byte[] actual = ApduTestData.CApdu.Select.Applet2.CApdu;

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Ppse_CreatingSelectCommand_ReturnsActual()
    {
        byte[] expected = ApduTestData.CApdu.Select.Ppse.PpseBytes;
        byte[] actual = SelectApduCommand.SelectProximityPaymentSystemEnvironment().Serialize();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    #endregion
}