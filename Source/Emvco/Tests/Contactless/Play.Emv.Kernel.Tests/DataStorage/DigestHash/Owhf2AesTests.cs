using System;

using AutoFixture;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Kernel.Services;
using Play.Encryption.Ciphers.Symmetric;
using Play.Testing.Emv.Contactless.AutoFixture;

using Xunit;

namespace Play.Emv.Kernel.Tests.DataStorage.DigestHash;

public class Owhf2AesTests
{
    #region Instance Values

    private readonly IFixture _Fixture;
    private readonly ITlvReaderAndWriter _Database;
    private static readonly AesCodec _AesCodec = new(new BlockCipherConfiguration(BlockCipherMode.Cbc, BlockPaddingMode.None, KeySize._128, BlockSize._16,
        new Iso7816PlainTextPreprocessor(BlockSize._16)));

    #endregion

    #region Constructors

    public Owhf2AesTests()
    {
        _Fixture = new ContactlessFixture().Create();
        _Database = ContactlessFixture.CreateDefaultDatabase(_Fixture);
    }

    #endregion

    #region Tests

    [Fact]
    public void AesEncryptionService_InvokingSignOnValidBlockLengthInputC_ResultIsNotNull()
    {
        //Arrange
        ReadOnlySpan<byte> message = stackalloc byte[] { 31, 18, 68, 78, 91, 102, 34, 63 };

        RegisterDefaultConfiguration(_Database);

        //Act
        byte[] encryptedResult = Owhf2Aes.ComputeR(_Database, message);

        //Assert
        Assert.NotNull(encryptedResult);
    }

    [Fact]
    public void AesEncryptionService_InvokingSignOnInvalidBlockLengthInputC_ExceptionIsThrown()
    {
        //Arrange
        RegisterDefaultConfiguration(_Database);

        //Act & Assert

        Assert.Throws<TerminalDataException>(() =>
        {
            ReadOnlySpan<byte> message = stackalloc byte[] { 31, 18, 68, 78, 91, 102, 34, 63, 32, 33 };

            byte[] encryptedResult = Owhf2Aes.ComputeR(_Database, message);
        });
    }

    [Fact]
    public void AesEncryptionService_InvokingSign_ReturnsExpectedResult()
    {
        //Arrange
        ReadOnlySpan<byte> inputC = stackalloc byte[] { 31, 18, 68, 78, 91, 102, 34, 63 };

        RegisterDefaultConfiguration(_Database);

        byte[] expectedKey = { 0, 0, 0, 11, 12, 33, 44, 55, 66, 77, 88, 6, 7, 8, 63, 0 };
        byte[] expectedMessage = CreateExpectedAesMessage(inputC, _Database.Get<DataStorageRequestedOperatorId>(DataStorageRequestedOperatorId.Tag));

        //Act
        byte[] encryptedResult = Owhf2Aes.ComputeR(_Database, inputC);

        byte[] expectedEncryptedResult = _AesCodec.Sign(expectedMessage, expectedKey);
        Span<byte> result = stackalloc byte[expectedEncryptedResult.Length];

        ComputeXor(expectedEncryptedResult, expectedMessage, result);

        //Assert
        Assert.Equal(result[^8..].ToArray(), encryptedResult);
    }

    [Theory]
    [MemberData(nameof(Owhf2AesTestsFixtures.GetRandomInputs), 10, MemberType = typeof(Owhf2AesTestsFixtures))]
    public void RandomInputC_InvokingComputeR_ReturnsExpectedResult(byte[] inputC)
    {
        //Arrange
        RegisterDefaultConfiguration(_Database);

        byte[] expectedKey = { 0, 0, 0, 11, 12, 33, 44, 55, 66, 77, 88, 6, 7, 8, 63, 0 };
        byte[] expectedMessage = CreateExpectedAesMessage(inputC, _Database.Get<DataStorageRequestedOperatorId>(DataStorageRequestedOperatorId.Tag));

        //Act
        byte[] encryptedResult = Owhf2Aes.ComputeR(_Database, inputC);

        byte[] expectedEncryptedResult = _AesCodec.Sign(expectedMessage, expectedKey);
        Span<byte> result = stackalloc byte[expectedEncryptedResult.Length];

        ComputeXor(expectedEncryptedResult, expectedMessage, result);

        //Assert
        Assert.Equal(result[^8..].ToArray(), encryptedResult);
    }

    #endregion

    private static void ComputeXor(byte[] left, byte[] right, Span<byte> output)
    {
        if (left.Length != right.Length)
            throw new Exception("Something went wrong, this should not happen ");

        for (int i = 0; i < left.Length; i++)
        {
            output[i] = (byte)(left[i] ^ right[i]);
        }
    }

    private static byte[] CreateExpectedAesMessage(ReadOnlySpan<byte> inputC, DataStorageRequestedOperatorId objectId)
    {
        Span<byte> buffer = stackalloc byte[16];

        inputC.CopyTo(buffer);
        objectId.EncodeValue().CopyTo(buffer[^8..]);

        return buffer.ToArray();
    }

    private static void RegisterDefaultConfiguration(ITlvReaderAndWriter database)
    {
        ReadOnlyMemory<byte> dataStorageIdInput = new byte[] { 11, 12, 33, 44, 55, 66, 177, 88 };
        DataStorageId dataStorageId = DataStorageId.Decode(dataStorageIdInput);
        database.Update(dataStorageId);

        ReadOnlyMemory<byte> dataStorageRequestedOperatorIdInput = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        DataStorageRequestedOperatorId operatorId = DataStorageRequestedOperatorId.Decode(dataStorageRequestedOperatorIdInput);
        database.Update(operatorId);

        ReadOnlyMemory<byte> dataStorageOperatorDataSetInfo = new byte[] { 1 };
        DataStorageOperatorDataSetInfo info = DataStorageOperatorDataSetInfo.Decode(dataStorageOperatorDataSetInfo);
        database.Update(info);
    }

}
