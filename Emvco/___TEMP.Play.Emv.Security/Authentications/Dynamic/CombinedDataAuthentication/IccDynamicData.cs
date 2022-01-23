using ___TEMP.Play.Emv.Security.Cryptograms;
using ___TEMP.Play.Emv.Security.Encryption.Hashing;

using Play.Codecs;
using Play.Emv.DataElements;

namespace ___TEMP.Play.Emv.Security.Authentications.Dynamic.CombinedDataAuthentication;

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

    public ApplicationCryptogram GetCryptogram()
    {
        return new(_Value[(GetIccDynamicNumberLength() + 2)..(GetIccDynamicNumberLength() + 10)]);
    }

    public CryptogramInformationData GetCryptogramInformationData()
    {
        return new(_Value[GetIccDynamicNumberLength() + 1]);
    }

    public IccDynamicNumber GetIccDynamicNumber()
    {
        return new(PlayEncoding.UnsignedBinary.GetUInt64(_Value[1..GetIccDynamicNumberLength()]));
    }

    public byte GetIccDynamicNumberLength()
    {
        return _Value[0];
    }

    public Hash GetTransactionDataHashCode()
    {
        return new(_Value[(GetIccDynamicNumberLength() + 10)..Hash.Length]);
    }

    #endregion
}