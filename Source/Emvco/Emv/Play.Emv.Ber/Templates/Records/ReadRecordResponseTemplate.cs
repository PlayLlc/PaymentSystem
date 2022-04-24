using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;

namespace Play.Emv.Ber.Templates;

public abstract class ReadRecordResponseTemplate : Template
{
    #region Static Metadata

    public static readonly Tag Tag = 0x70;

    #endregion

    #region Instance Values

    protected byte[] _Values;

    #endregion

    #region Constructor

    protected ReadRecordResponseTemplate()
    {
        _Values = Array.Empty<byte>();
    }

    protected ReadRecordResponseTemplate(byte[] values)
    {
        _Values = values;
    }

    #endregion

    #region Instance Members

    public byte[] AsByteArray() => _Values;

    /// <exception cref="BerParsingException"></exception>
    public static PrimitiveValue[] GetPrimitiveValuesFromRecords(IResolveKnownObjectsAtRuntime runtimeCodec, ReadOnlyMemory<byte> value)
    {
        List<PrimitiveValue> buffer = new();

        EncodedTlvSiblings siblings = _Codec.DecodeSiblings(value);

        foreach (Tag tag in siblings.GetTags())
        {
            if (!siblings.TryGetValueOctetsOfSibling(tag, out ReadOnlyMemory<byte> rawResult))
                continue;

            if (runtimeCodec.TryDecodingAtRuntime(tag, rawResult, out PrimitiveValue? result))
                buffer.Add(result!);
        }

        return buffer.ToArray();
    }

    #endregion
}