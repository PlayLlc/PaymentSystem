﻿using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Metadata;
using Play.Emv.Acquirer.Codecs;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.Codecs;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record Reserved57 : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 57</remarks>
    public static readonly DataFieldId DataFieldId = new(57);

    public static readonly PlayEncodingId EncodingId = AlphaNumericSpecialDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 999;
    private const byte _LeadingOctetByteCount = 2;

    #endregion

    #region Constructor

    public Reserved57(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override Reserved57 Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new Reserved57(result.Value);
    }

    #endregion
}