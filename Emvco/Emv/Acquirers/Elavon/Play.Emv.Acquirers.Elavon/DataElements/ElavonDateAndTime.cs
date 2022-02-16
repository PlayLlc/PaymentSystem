using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Core.Extensions;
using Play.Emv.Acquirers.Elavon.DataElements;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;
using Play.Emv.Sessions;

namespace Play.Emv.Acquirers.Elavon.DataElements;

/// <summary>
///     The Elavon Date and Time will be returned to the POS or partner host system in the event of an approval of a sale
///     transaction. This data must be provided to the Elavon host in the event of a reversal of the aforementioned sale
///     transaction. This sub-field must be populated irrespective of whether single or dual message processing is used.
/// </summary>
public record ElavonDateAndTime : ElavonDataElement<SystemTraceAuditNumber>
{
    #region Static Metadata

    /// <value>Hex: 0x0003 Decimal: 3</value>
    public static readonly Tag Tag = 0x0003;

    public static readonly BerEncodingId BerEncodingId = NumericCodec.Identifier;
    private const byte _ByteLength = 6;

    #endregion

    #region Constructor

    public ElavonDateAndTime(SystemTraceAuditNumber value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static ElavonDateAndTime Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ElavonDateAndTime Decode(ReadOnlySpan<byte> value)
    {
        const byte charLength = 12;

        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<uint> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<uint>
            ?? throw new DataElementNullException(BerEncodingId);

        Check.Primitive.ForMaxCharLength(result.Value.GetNumberOfDigits(), charLength, Tag);

        return new ElavonDateAndTime(new SystemTraceAuditNumber(result.Value));
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(BerEncodingId, (uint) _Value, _ByteLength);

    #endregion

    #region Equality

    public bool Equals(ElavonDateAndTime? x, ElavonDateAndTime? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ElavonDateAndTime obj) => obj.GetHashCode();

    #endregion
}