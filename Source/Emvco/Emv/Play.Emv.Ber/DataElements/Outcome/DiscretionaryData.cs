using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Description: The Discretionary Data is a list of Kernel-specific data objects sent to the Terminal as a separate
///     field in the OUT DataExchangeSignal.
/// </summary>
public record DiscretionaryData : DataExchangeResponse, IEqualityComparer<DiscretionaryData>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xFF8106;

    #endregion

    #region Constructor

    public DiscretionaryData(params PrimitiveValue[] value) : base(value)
    { }

    #endregion

    #region Serialization

    public override DiscretionaryData Decode(TagLengthValue value) => Decode(value.EncodeValue().AsMemory());

    public static DiscretionaryData Decode(ReadOnlyMemory<byte> value)
    {
        List<PrimitiveValue> buffer = new();
        EncodedTlvSiblings siblings = _Codec.DecodeSiblings(value);

        if (siblings.TryGetValueOctetsOfSibling(ApplicationCapabilitiesInformation.Tag, out ReadOnlySpan<byte> applicationCapabilitiesInformation))
            buffer.Add(ApplicationCapabilitiesInformation.Decode(applicationCapabilitiesInformation));
        if (siblings.TryGetValueOctetsOfSibling(ApplicationCurrencyCode.Tag, out ReadOnlySpan<byte> applicationCurrencyCode))
            buffer.Add(ApplicationCurrencyCode.Decode(applicationCurrencyCode));
        if (siblings.TryGetValueOctetsOfSibling(BalanceReadAfterGenAc.Tag, out ReadOnlySpan<byte> balanceReadAfterGenAc))
            buffer.Add(BalanceReadAfterGenAc.Decode(balanceReadAfterGenAc));
        if (siblings.TryGetValueOctetsOfSibling(BalanceReadBeforeGenAc.Tag, out ReadOnlySpan<byte> balanceReadBeforeGenAc))
            buffer.Add(BalanceReadBeforeGenAc.Decode(balanceReadBeforeGenAc));
        if (siblings.TryGetValueOctetsOfSibling(DataStorageSummary3.Tag, out ReadOnlySpan<byte> dataStorageSummary3))
            buffer.Add(DataStorageSummary3.Decode(dataStorageSummary3));
        if (siblings.TryGetValueOctetsOfSibling(DataStorageSummaryStatus.Tag, out ReadOnlySpan<byte> dataStorageSummaryStatus))
            buffer.Add(DataStorageSummaryStatus.Decode(dataStorageSummaryStatus));
        if (siblings.TryGetValueOctetsOfSibling(ErrorIndication.Tag, out ReadOnlySpan<byte> errorIndication))
            buffer.Add(ErrorIndication.Decode(errorIndication));
        if (siblings.TryGetValueOctetsOfSibling(PostGenAcPutDataStatus.Tag, out ReadOnlySpan<byte> postGenAcPutDataStatus))
            buffer.Add(PostGenAcPutDataStatus.Decode(postGenAcPutDataStatus));
        if (siblings.TryGetValueOctetsOfSibling(PreGenAcPutDataStatus.Tag, out ReadOnlySpan<byte> preGenAcPutDataStatus))
            buffer.Add(PreGenAcPutDataStatus.Decode(preGenAcPutDataStatus));
        if (siblings.TryGetValueOctetsOfSibling(ThirdPartyData.Tag, out ReadOnlySpan<byte> thirdPartyData))
            buffer.Add(ThirdPartyData.Decode(thirdPartyData));
        if (siblings.TryGetValueOctetsOfSibling(TornRecord.Tag, out ReadOnlyMemory<byte> tornRecord))
            buffer.Add(TornRecord.Decode(tornRecord));
        if (siblings.TryGetValueOctetsOfSibling(Track1DiscretionaryData.Tag, out ReadOnlyMemory<byte> track1DiscretionaryData))
            buffer.Add(Track1DiscretionaryData.Decode(track1DiscretionaryData));
        if (siblings.TryGetValueOctetsOfSibling(Track2DiscretionaryData.Tag, out ReadOnlyMemory<byte> track2DiscretionaryData))
            buffer.Add(Track2DiscretionaryData.Decode(track2DiscretionaryData));

        return new DiscretionaryData(buffer.ToArray());
    }

    #endregion

    #region Equality

    public bool Equals(DiscretionaryData? x, DiscretionaryData? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(DiscretionaryData obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount() => GetByteCount();

    #endregion
}