﻿using System.Numerics;

using Play.Ber.InternalFactories;
using Play.Emv.Interchange.Codecs;
using Play.Emv.Interchange.Exceptions;
using Play.Interchange.Codecs;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record ReceivingInstitutionIdentificationCode : VariableDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 100</remarks>
    public static readonly DataFieldId DataFieldId = new(100);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 6;
    private const byte _LeadingOctetByteCount = 1;

    #endregion

    #region Constructor

    public ReceivingInstitutionIdentificationCode(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override ReceivingInstitutionIdentificationCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new ReceivingInstitutionIdentificationCode(result.Value);
    }

    #endregion
}