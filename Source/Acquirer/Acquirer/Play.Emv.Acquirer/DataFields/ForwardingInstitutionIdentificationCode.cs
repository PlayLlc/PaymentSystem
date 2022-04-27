﻿using System.Numerics;

using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record ForwardingInstitutionIdentificationCode : VariableDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 33</remarks>
    public static readonly DataFieldId DataFieldId = new(33);

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const ushort _MaxByteCount = 6;
    private const byte _LeadingOctetByteCount = 1;

    #endregion

    #region Constructor

    public ForwardingInstitutionIdentificationCode(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    /// <summary>
    ///     Decode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
    /// <exception cref="InterchangeException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InterchangeDataFieldNullException"></exception>
    public override ForwardingInstitutionIdentificationCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new ForwardingInstitutionIdentificationCode(result.Value);
    }

    #endregion
}