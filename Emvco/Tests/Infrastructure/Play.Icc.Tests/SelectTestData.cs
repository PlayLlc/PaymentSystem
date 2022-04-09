using Play.Icc.FileSystem.DedicatedFiles;
using Play.Icc.Messaging.Apdu.SelectFile;
using Play.Testing.Infrastructure.Icc.Apdu;

using Xunit;

namespace Play.Icc.Tests;

public class SelectTestData
{
    #region Instance Members

    [Fact]
    public void DedicatedFileName1_CreatingSelectCommand_ReturnsExpectedResult()
    {
        DedicatedFileName dedicatedFileName = new(ApduTestData.CApdu.Select.Applet1.DedicatedFileName);
        byte[] selectAid = SelectApduCommand.DedicatedFile(dedicatedFileName).Serialize();
        byte[] expectedResult = ApduTestData.CApdu.Select.Applet1.CApdu;

        Assert.Equal(selectAid, expectedResult);
    }

    [Fact]
    public void DedicatedFileName2_CreatingSelectCommand_ReturnsExpectedResult()
    {
        DedicatedFileName dedicatedFileName = new(ApduTestData.CApdu.Select.Applet2.DedicatedFileName);
        byte[] selectAid = SelectApduCommand.DedicatedFile(dedicatedFileName).Serialize();
        byte[] expectedResult = ApduTestData.CApdu.Select.Applet2.CApdu;

        Assert.Equal(selectAid, expectedResult);
    }

    [Fact]
    public void Ppse_CreatingSelectCommand_ReturnsExpectedResult()
    {
        byte[] expectedResult = ApduTestData.CApdu.Select.Ppse.PpseBytes;
        byte[] selectPpseCApdu = SelectApduCommand.SelectProximityPaymentSystemEnvironment().Serialize();

        Assert.Equal(selectPpseCApdu, expectedResult);
    }

    #endregion
}