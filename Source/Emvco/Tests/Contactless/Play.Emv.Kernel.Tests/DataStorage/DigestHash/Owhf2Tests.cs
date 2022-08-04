﻿using System;

using AutoFixture;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
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

    private static readonly byte[] _InitializationVector = { 18, 114, 31, 64, 7, 18, 20, 11 };

    private static readonly TripleDesCodec _DesCodec = new TripleDesCodec(new BlockCipherConfiguration(BlockCipherMode.Cbc, BlockPaddingMode.None, KeySize._128, BlockSize._8,
        new Iso7816PlainTextPreprocessor(BlockSize._8)));

    #endregion

    #region Constructor

    public Owhf2Tests()
    {
        _Fixture = new ContactlessFixture().Create();
        _Database = ContactlessFixture.CreateDefaultDatabase(_Fixture);

        //This is only for testing.
        Owhf2.UpdateInitializationVector(_InitializationVector);
        _DesCodec.SetInitializationVector(_InitializationVector);
    }

    #endregion

    #region Tests

    [Fact]
    public void DesEncryptionService_InvokingComputeROnValidBlockLengthMessage_IsNotNull()
    {
        //Arrange
        ReadOnlySpan<byte> message = stackalloc byte[] { 31, 18, 68, 78, 91, 102, 34, 63 };

        Owhf2TestsConfigurationSetup.RegisterDefaultConfiguration(_Database);

        //Act
        byte[] encryptedResult = Owhf2.Hash(_Database, message);

        //Assert
        Assert.NotNull(encryptedResult);
    }

    [Fact]
    public void DesEncryptionService_InvokingComputeROnInvalidBlockLengthMessage_PadsMissingBlockSizeAndEncryptsSuccesfully()
    {
        //Arrange
        Owhf2TestsConfigurationSetup.RegisterDefaultConfiguration(_Database);

        //Act & Assert
        Assert.Throws<TerminalDataException>(() =>
        {
            ReadOnlySpan<byte> message = stackalloc byte[] { 31, 18, 68, 78, 91, 102, 34, 63, 32, 33 };

            byte[] encryptedResult = Owhf2.Hash(_Database, message);
        });
    }

    [Fact]
    public void DesEncryptionService_InvokingComputeR_ReturnsExpectedResult()
    {
        //Arrange
        ReadOnlySpan<byte> message = stackalloc byte[] { 31, 18, 68, 78, 91, 102, 34, 63 };

        Owhf2TestsConfigurationSetup.RegisterDefaultConfiguration(_Database);

        byte[] expectedKey = { 22, 24, 42, 64, 74, 84, 5, 6, 24, 42, 64, 74, 84, 106, 7, 8 };
        byte[] expectedMessage = ComputeExpectedMessage(message.ToArray(), _Database.Get<DataStorageRequestedOperatorId>(DataStorageRequestedOperatorId.Tag));
        byte[] encryption = _DesCodec.Encrypt(expectedMessage, expectedKey);
        byte[] expectedOwhf2Encryption = ComputeExpectedEncryptedResult(encryption, message.ToArray());

        //Act
        byte[] encryptedResult = Owhf2.Hash(_Database, message);

        //Assert
        Assert.NotNull(encryptedResult);
        Assert.Equal(expectedOwhf2Encryption, encryptedResult);
    }

    [Theory]
    [MemberData(nameof(Owhf2TestFixtures.GetRandomMessages), 10, 1, 8, MemberType = typeof(Owhf2TestFixtures))]
    public void RandomMessage_InvokingComputeR_ReturnsExpectedResult(byte[] inputPD)
    {
        //Arrange
        Owhf2TestsConfigurationSetup.RegisterDefaultConfiguration(_Database);

        byte[] expectedKey = { 22, 24, 42, 64, 74, 84, 5, 6, 24, 42, 64, 74, 84, 106, 7, 8 };
        byte[] expectedMessage = ComputeExpectedMessage(inputPD, _Database.Get<DataStorageRequestedOperatorId>(DataStorageRequestedOperatorId.Tag));
        byte[] encryption = _DesCodec.Encrypt(expectedMessage, expectedKey);
        byte[] expectedOwhf2Encryption = ComputeExpectedEncryptedResult(encryption, inputPD);

        //Act
        byte[] encryptedResult = Owhf2.Hash(_Database, inputPD);

        //Assert
        Assert.NotNull(encryptedResult);
        Assert.Equal(expectedOwhf2Encryption, encryptedResult);
    }

    #endregion

    #region Private Members

    private static byte[] ComputeExpectedMessage(byte[] inputPD, DataStorageRequestedOperatorId operatorId)
    {
        byte[] encodedObjectId = operatorId.EncodeValue();

        if (encodedObjectId.Length != inputPD.Length)
            throw new Exception("This should not happen");

        for (int i = 0; i < encodedObjectId.Length; i++)
            encodedObjectId[i] = (byte)(encodedObjectId[i] ^ inputPD[i]);

        return encodedObjectId;
    }

    private static byte[] ComputeExpectedEncryptedResult(byte[] initialEncryption, byte[] inputPD)
    {
        if (initialEncryption.Length != inputPD.Length)
            throw new Exception("This should not happen");

        for (int i = 0; i < initialEncryption.Length; i++)
            initialEncryption[i] = (byte)(initialEncryption[i] ^ inputPD[i]);

        return initialEncryption;
    }

    #endregion
}
