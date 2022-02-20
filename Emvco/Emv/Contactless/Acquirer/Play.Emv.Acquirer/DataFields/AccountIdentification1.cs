using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Emv.Interchange.Codecs;
using Play.Emv.Interchange.Exceptions;
using Play.Interchange.Codecs;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

//public record AccountIdentification1 : VariableDataField<char[]>
//{
//    #region Static Metadata

//    /// <remarks>DecimalValue: 102</remarks>
//    public static readonly DataFieldId DataFieldId = 102;

//    public static readonly InterchangeEncodingId EncodingId = AlphaNumericSpecialDataFieldCodec.Identifier;
//    private const ushort _MaxByteLength = 28;
//    private const byte _LeadingOctetByteCount = 1;

//    #endregion

//    #region Constructor

//    public AccountIdentification1(char[] value) : base(value)
//    { }

//    #endregion

//    #region Instance Members

//    public override DataFieldId GetDataFieldId() => DataFieldId;
//    protected override ushort GetMaxByteCount() => _MaxByteLength;
//    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;
//    public override InterchangeEncodingId GetEncodingId() => EncodingId;

//    #endregion

//    #region Serialization

//    public override AccountIdentification1 Decode(ReadOnlyMemory<byte> value)
//    {
//        Check.DataField.ForMaximumLength(value, _MaxByteLength, DataFieldId);

//        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
//            ?? throw new InterchangeDataFieldNullException(EncodingId);

//        return new AccountIdentification1(result.Value);
//    }

//    #endregion
//}

//public record AcquiringInstitutionCountryCode : FixedDataField<ushort>
//{
//    #region Static Metadata

//    /// <remarks>DecimalValue: 19</remarks>
//    public static readonly DataFieldId DataFieldId = new(19);

//    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
//    private const ushort _ByteCount = 2;

//    #endregion

//    #region Constructor

//    public AcquiringInstitutionCountryCode(ushort original) : base(original)
//    { }

//    #endregion

//    #region Instance Members

//    public override DataFieldId GetDataFieldId() => DataFieldId;
//    public override InterchangeEncodingId GetEncodingId() => EncodingId;

//    #endregion

//    #region Serialization

//    public override AcquiringInstitutionCountryCode Decode(ReadOnlyMemory<byte> value)
//    {
//        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);

//        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value.Span).ToUInt16Result()
//            ?? throw new InterchangeDataFieldNullException(EncodingId);

//        return new AcquiringInstitutionCountryCode(result.Value);
//    }

//    #endregion
//}