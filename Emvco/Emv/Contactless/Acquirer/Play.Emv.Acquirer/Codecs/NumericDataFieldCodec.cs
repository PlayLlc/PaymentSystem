using Play.Codecs;
using Play.Emv.Codecs;

namespace Play.Emv.Acquirer.Codecs;

/// <summary>
///     Numeric data elements consist of two numeric digits (having values in the range Hex '0' – '9') per byte.
///     These digits are right justified and padded with leading hexadecimal zeroes. Other specifications sometimes
///     refer to this data format as Binary Coded Decimal (“BCD”) or unsigned packed.
///     Example: Amount, Authorized(Numeric) is defined as “n 12” with a length of six bytes.
///     A value of 12345 is stored in Amount, Authorized (Numeric) as Hex '00 00 00 01 23 45'.
/// </summary>
public class NumericDataFieldCodec : NumericEmvCodec, IInterchangeDataFieldCodec
{
    #region Static Metadata

    #region Metadata

    public static readonly PlayEncodingId Identifier = IInterchangeDataFieldCodec.GetEncodingId(typeof(NumericDataFieldCodec));

    #endregion

    #endregion

    #region Instance Members

    public PlayEncodingId GetIdentifier() => Identifier;

    #endregion
}