using Play.Ber.DataObjects;
using Play.Ber.Identifiers;

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

    /// <exception cref="Play.Ber.Exceptions.BerParsingException"></exception>
    public static PrimitiveValue[] GetRecords(ReadOnlySpan<byte> value) => _Codec.DecodePrimitiveValuesAtRuntime(value).ToArray();

    #endregion
}