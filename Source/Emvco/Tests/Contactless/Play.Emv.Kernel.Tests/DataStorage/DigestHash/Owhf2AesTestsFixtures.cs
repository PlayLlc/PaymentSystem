using System;
using System.Collections.Generic;

using AutoFixture;

using Play.Encryption.Ciphers.Symmetric;

namespace Play.Emv.Kernel.Tests.DataStorage.DigestHash;

public static class Owhf2AesTestsFixtures
{
    #region Static Metadata

    private static readonly Random _Random = new();

    #endregion

    #region Instance Members

    public static void RegisterDefaultBlockCypherConfiguration(IFixture fixture)
    {
        byte[] initializationVector = GetRandomArray(8);

        BlockCipherConfiguration configuration = new(BlockCipherMode.Cbc, BlockPaddingMode.None, KeySize._128, BlockSize._16,
        new Iso7816PlainTextPreprocessor(BlockSize._16), initializationVector);

        fixture.Register(() => configuration);
    }

    public static IEnumerable<object[]> GetRandomInputs(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] { GetRandomArray(8) };
    }

    #endregion

    #region Private Methods

    private static byte[] GetRandomArray(int length)
    {
        byte[] result = new byte[length];

        for (int i = 0; i < length; i++)
            result[i] = GetRandom();

        return result;
    }

    private static byte GetRandom() => (byte)_Random.Next(byte.MinValue, byte.MaxValue);

    #endregion
}
