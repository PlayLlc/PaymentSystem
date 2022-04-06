using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Encryption.Hashing;

namespace Play.Emv.Security.Authentications;

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
    public IccDynamicNumber GetIccDynamicNumber() => new(PlayCodec.BinaryCodec.DecodeToUInt64(_Value[1..GetIccDynamicNumberLength()]));
    public byte GetIccDynamicNumberLength() => _Value[0];
    public Hash GetTransactionDataHashCode() => new(_Value[(GetIccDynamicNumberLength() + 10)..Hash.Length]);
    private bool IsDataStorageSummaryAvailable() => _Value.Length > (30 + GetIccDynamicNumberLength());

    /// <exception cref="Ber.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    public bool TryGetAdditionalData(out PrimitiveValue[] result)
    {
        if (!IsDataStorageSummaryAvailable())
        {
            result = Array.Empty<PrimitiveValue>();

            return false;
        }

        result = DecodeAdditionalData(_Value.AsSpan()).ToArray();

        return true;
    }

    /// <exception cref="Ber.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    private IEnumerable<PrimitiveValue> DecodeAdditionalData(ReadOnlySpan<byte> value)
    {
        EmvCodec codec = EmvCodec.GetBerCodec();
        TagLengthValue[] values = codec.DecodeTagLengthValues(_Value.AsSpan()[(GetIccDynamicNumberLength() + 30)..]);

        for (int i = 0; i < values.Length; i++)
        {
            if (values[i].GetTag() == DataStorageSummary2.Tag)
                yield return DataStorageSummary2.Decode(values[i].EncodeValue().AsSpan());
            if (values[i].GetTag() == DataStorageSummary3.Tag)
                yield return DataStorageSummary3.Decode(values[i].EncodeValue().AsSpan());
            if (values[i].GetTag() == TerminalRelayResistanceEntropy.Tag)
                yield return TerminalRelayResistanceEntropy.Decode(values[i].EncodeValue().AsSpan());
            if (values[i].GetTag() == DeviceRelayResistanceEntropy.Tag)
                yield return DeviceRelayResistanceEntropy.Decode(values[i].EncodeValue().AsSpan());
            if (values[i].GetTag() == MinTimeForProcessingRelayResistanceApdu.Tag)
                yield return MinTimeForProcessingRelayResistanceApdu.Decode(values[i].EncodeValue().AsSpan());
            if (values[i].GetTag() == MaxTimeForProcessingRelayResistanceApdu.Tag)
                yield return MaxTimeForProcessingRelayResistanceApdu.Decode(values[i].EncodeValue().AsSpan());
            if (values[i].GetTag() == DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Tag)
                yield return DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Decode(values[i].EncodeValue().AsSpan());
        }
    }

    #endregion
}