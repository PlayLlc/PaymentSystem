using System;
using System.Security.Cryptography;

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
    private readonly TripleDesCodec _DesCodec;

    #endregion

    #region Constructor

    public Owhf2Tests()
    {
        _Fixture = new ContactlessFixture().Create();
        _Database = ContactlessFixture.CreateDefaultDatabase(_Fixture);
        _DesCodec = new TripleDesCodec(new BlockCipherConfiguration(BlockCipherMode.Cbc, BlockPaddingMode.None, KeySize._128, BlockSize._8,
        new Iso7816PlainTextPreprocessor(BlockSize._8)));
    }

    #endregion

    #region Tests

    [Fact]
    public void DesEncryptionService_InvokingSignOnValidBlockLengthMessage_IsNotNull()
    {
        //Arrange
        ReadOnlySpan<byte> message = stackalloc byte[] { 31, 18, 68, 78, 91, 102, 34, 63 };

        ReadOnlyMemory<byte> dataStorageIdInput = new byte[] { 11, 12, 33, 44, 55, 66, 77, 88 };
        DataStorageId dataStorageId = DataStorageId.Decode(dataStorageIdInput);
        _Database.Update(dataStorageId);

        ReadOnlyMemory<byte> dataStorageRequestedOperatorIdInput = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        DataStorageRequestedOperatorId operatorId = DataStorageRequestedOperatorId.Decode(dataStorageRequestedOperatorIdInput);
        _Database.Update(operatorId);

        ReadOnlyMemory<byte> dataStorageOperatorDataSetInfo = new byte[] { 1 };
        DataStorageOperatorDataSetInfo info = DataStorageOperatorDataSetInfo.Decode(dataStorageOperatorDataSetInfo);
        _Database.Update(info);

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

        ReadOnlyMemory<byte> dataStorageIdInput = new byte[] { 11, 12, 33, 44, 55, 66, 77, 88 };
        DataStorageId dataStorageId = DataStorageId.Decode(dataStorageIdInput);
        _Database.Update(dataStorageId);

        ReadOnlyMemory<byte> dataStorageRequestedOperatorIdInput = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        DataStorageRequestedOperatorId operatorId = DataStorageRequestedOperatorId.Decode(dataStorageRequestedOperatorIdInput);
        _Database.Update(operatorId);

        ReadOnlyMemory<byte> dataStorageOperatorDataSetInfo = new byte[] { 1 };
        DataStorageOperatorDataSetInfo info = DataStorageOperatorDataSetInfo.Decode(dataStorageOperatorDataSetInfo);
        _Database.Update(info);

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

        ReadOnlyMemory<byte> dataStorageIdInput = new byte[] { 11, 12, 33, 44, 55, 66, 77, 88 };
        DataStorageId dataStorageId = DataStorageId.Decode(dataStorageIdInput);
        _Database.Update(dataStorageId);

        ReadOnlyMemory<byte> dataStorageRequestedOperatorIdInput = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        DataStorageRequestedOperatorId operatorId = DataStorageRequestedOperatorId.Decode(dataStorageRequestedOperatorIdInput);
        _Database.Update(operatorId);

        ReadOnlyMemory<byte> dataStorageOperatorDataSetInfo = new byte[] { 1 };
        DataStorageOperatorDataSetInfo info = DataStorageOperatorDataSetInfo.Decode(dataStorageOperatorDataSetInfo);
        _Database.Update(info);

        byte[] expectedKey = { 22, 24, 42, 64, 74, 84, 5, 6, 24, 42, 64, 74, 84, 106, 0, 0 };

        //Act
        byte[] encryptedResult = Owhf2.Sign(_Database, message);
        byte[] decryptedResult = _DesCodec.Decrypt(encryptedResult, expectedKey);

        //Assert
        Assert.NotNull(encryptedResult);
        Assert.Equal(message.ToArray(), decryptedResult);
    }

    #endregion
}
