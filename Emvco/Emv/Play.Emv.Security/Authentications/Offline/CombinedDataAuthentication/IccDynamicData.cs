using System;

using Play.Codecs;
using Play.Emv.DataElements;
using Play.Emv.Security.Cryptograms;
using Play.Encryption.Encryption.Hashing;

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
        new ApplicationCryptogram(_Value[(GetIccDynamicNumberLength() + 2)..(GetIccDynamicNumberLength() + 10)]);

    public CryptogramInformationData GetCryptogramInformationData() =>
        new CryptogramInformationData(_Value[GetIccDynamicNumberLength() + 1]);

    public IccDynamicNumber GetIccDynamicNumber() =>
        new IccDynamicNumber(PlayEncoding.UnsignedBinary.GetUInt64(_Value[1..GetIccDynamicNumberLength()]));

    public byte GetIccDynamicNumberLength() => _Value[0];
    public Hash GetTransactionDataHashCode() => new Hash(_Value[(GetIccDynamicNumberLength() + 10)..Hash.Length]);

    #endregion
}