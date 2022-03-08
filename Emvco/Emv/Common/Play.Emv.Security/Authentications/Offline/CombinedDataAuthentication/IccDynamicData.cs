﻿using System;

using Play.Codecs;
using Play.Emv.DataElements.Emv.Primitives.Card.Icc;
using Play.Emv.DataElements.Emv.Primitives.Security;
using Play.Encryption.Hashing;

namespace Play.Emv.Security.Authentications.Offline.CombinedDataAuthentication;

internal class IccDynamicData
{
    #region Instance Values

    private readonly byte[] _Value;

    #endregion

    #region Constructor

    public IccDynamicData(ReadOnlySpan<byte> value)
    {
        _Value = value.ToArray();
    }

    #endregion

    #region Instance Members

    public ApplicationCryptogram GetCryptogram() =>
        ApplicationCryptogram.Decode(_Value[(GetIccDynamicNumberLength() + 2)..(GetIccDynamicNumberLength() + 10)].AsSpan());

    public CryptogramInformationData GetCryptogramInformationData() => new(_Value[GetIccDynamicNumberLength() + 1]);

    public IccDynamicNumber GetIccDynamicNumber() =>
        new(PlayCodec.UnsignedBinaryCodec.DecodeToUInt64(_Value[1..GetIccDynamicNumberLength()]));

    public byte GetIccDynamicNumberLength() => _Value[0];
    public Hash GetTransactionDataHashCode() => new(_Value[(GetIccDynamicNumberLength() + 10)..Hash.Length]);

    #endregion
}