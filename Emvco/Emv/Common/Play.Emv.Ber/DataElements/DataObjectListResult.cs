using System.Numerics;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.Extensions.Arrays;
using Play.Emv.Ber.Templates;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     The result from a <see cref="DataObjectList" /> requested by the ICC. The object contains a concatenated
///     byte array of BER encoded TLV objects to be sent to the ICC
/// </summary>
/// <remarks>Book 3 Section 5.4</remarks>
public class DataObjectListResult : IEqualityComparer<DataObjectListResult>, IEquatable<DataObjectListResult>
{
    #region Static Metadata

    private static readonly EmvCodec _Codec = EmvCodec.GetBerCodec();

    #endregion

    #region Instance Values

    private readonly TagLengthValue[] _Value;

    #endregion

    #region Constructor

    public DataObjectListResult(params TagLengthValue[] value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     AsByteArray
    /// </summary>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    public byte[] AsByteArray()
    {
        return _Value.SelectMany(a => a.EncodeTagLengthValue(_Codec)).ToArray();
    }

    /// <summary>
    ///     AsCommandTemplate
    /// </summary>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    public CommandTemplate AsCommandTemplate()
    {
        List<byte> buffer = new();
        for (int i = 0; i < _Value.Length; i++)
            buffer.AddRange(_Value[i].EncodeValue(_Codec));

        return new CommandTemplate(new BigInteger(buffer.ToArray()));
    }

    public TagLengthValue[] AsPrimitiveValues() => _Value;
    public int ByteCount() => (int) _Value.Sum(a => a.GetTagLengthValueByteCount(_Codec));
    public byte[] Encode() => _Value.Encode();

    #endregion

    #region Equality

    public override bool Equals(object? other) => other is DataObjectListResult dataObjectListResult && Equals(dataObjectListResult);

    public bool Equals(DataObjectListResult? x, DataObjectListResult? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public bool Equals(DataObjectListResult? other)
    {
        if (other is null)
            return false;

        if (_Value.Length != other._Value.Length)
            return false;

        for (nint i = 0; i < _Value.Length; i++)
        {
            if (_Value[i] != other._Value[i])
                return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int result = 833977;

            for (int i = 0; i < _Value.Length; i++)
                result += _Value[i].GetHashCode();

            return result;
        }
    }

    public int GetHashCode(DataObjectListResult obj) => obj.GetHashCode();

    #endregion
}