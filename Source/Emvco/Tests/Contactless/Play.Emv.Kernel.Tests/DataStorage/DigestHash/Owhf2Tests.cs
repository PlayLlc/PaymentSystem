using System;

using AutoFixture;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Kernel.Services;
using Play.Encryption.Ciphers.Symmetric;
using Play.Testing.Emv.Contactless.AutoFixture;

using Xunit;

namespace Play.Emv.Kernel.Tests.DataStorage.DigestHash;

public class Owhf2Tests
{
    #region Instance Values

    private readonly IFixture _Fixture;
    private readonly ITlvReaderAndWriter _Database;
    private static readonly TripleDesCodec _DesCodec = new TripleDesCodec(new BlockCipherConfiguration(BlockCipherMode.Cbc, BlockPaddingMode.None, KeySize._128, BlockSize._8,
        new Iso7816PlainTextPreprocessor(BlockSize._8)));

    #endregion

    #region Constructor

    public Owhf2Tests()
    {
        _Fixture = new ContactlessFixture().Create();
        _Database = ContactlessFixture.CreateDefaultDatabase(_Fixture);
    }

    #endregion

    #region Tests

    [Fact]
    public void DesEncryptionService_InvokingSignOnValidBlockLengthMessage_IsNotNull()
    {
        //Arrange
        ReadOnlySpan<byte> message = stackalloc byte[] { 31, 18, 68, 78, 91, 102, 34, 63 };

        Owhf2TestsConfigurationSetup.RegisterDefaultConfiguration(_Database);

        //Act
        byte[] encryptedResult = Owhf2.Sign(_Database, message);

        //Assert
        Assert.NotNull(encryptedResult);
    }

    [Fact]
    public void DesEncryptionService_InvokingSignOnInvalidBlockLengthMessage_PadsMissingBlockSizeAndEncryptsSuccesfully()
    {
        //Arrange
        ReadOnlySpan<byte> message = stackalloc byte[] { 31, 18, 68, 78, 91, 102, 34, 63, 32, 33 };

        Owhf2TestsConfigurationSetup.RegisterDefaultConfiguration(_Database);

        //Act
        byte[] encryptedResult = Owhf2.Sign(_Database, message);

        //Assert
        Assert.NotNull(encryptedResult);
    }

    [Fact]
    public void DesEncryptionService_InvokingSign_ReturnsExpectedResult()
    {
        //Arrange
        ReadOnlySpan<byte> message = stackalloc byte[] { 31, 18, 68, 78, 91, 102, 34, 63 };

        Owhf2TestsConfigurationSetup.RegisterDefaultConfiguration(_Database);

        byte[] expectedKey = { 22, 24, 42, 64, 74, 84, 5, 6, 24, 42, 64, 74, 84, 106, 0, 0 };

        //Act
        byte[] encryptedResult = Owhf2.Sign(_Database, message);
        byte[] decryptedResult = _DesCodec.Decrypt(encryptedResult, expectedKey);

        //Assert
        Assert.NotNull(encryptedResult);
        Assert.Equal(message.ToArray(), decryptedResult);
    }

    [Theory]
    [MemberData(nameof(Owhf2TestFixtures.GetRandomMessages), 10, 3, 8, MemberType = typeof(Owhf2TestFixtures))]
    public void RandomMessage_InvokingSign_ReturnsExpectedResult(byte[] message)
    {
        //Arrange
        Owhf2TestsConfigurationSetup.RegisterDefaultConfiguration(_Database);

        byte[] expectedKey = { 22, 24, 42, 64, 74, 84, 5, 6, 24, 42, 64, 74, 84, 106, 0, 0 };

        //Act
        byte[] encryptedResult = Owhf2.Sign(_Database, message);
        byte[] decryptedResult = _DesCodec.Decrypt(encryptedResult, expectedKey);

        //Assert
        Assert.NotNull(encryptedResult);
        Assert.Equal(message, decryptedResult);
    }

    #endregion
}
