using Play.Ber.DataObjects;

namespace Play.Emv.Ber.DataObjects;

/// <summary>
///     The result from a <see cref="DataObjectList" /> requested by the ICC. The object contains a concatenated
///     byte array of BER encoded TLV objects to be sent to the ICC
/// </summary>
/// <remarks>Book 3 Section 5.4</remarks>
public class DataObjectListResult : IEqualityComparer<DataObjectListResult>, IEquatable<DataObjectListResult>
{
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
    /// AsByteArray
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Play.Ber.Exceptions.BerException"></exception>
    public byte[] AsByteArray()
    {
        return _Value.SelectMany(a => a.EncodeTagLengthValue()).ToArray();
    }

    /// <summary>
    ///     AsCommandTemplate
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Play.Ber.Exceptions.BerException"></exception>
    public CommandTemplate AsCommandTemplate()
    {
        List<byte> buffer = new();
        for (int i = 0; i < _Value.Length; i++)
            buffer.AddRange(_Value[i].EncodeValue());

        return new CommandTemplate(buffer.ToArray());
    }

    public TagLengthValue[] AsTagLengthValueArray() => _Value;
    public int ByteCount() => _Value.Length;

    #endregion

    #region Equality

    public override bool Equals(object? other) => other is DataObjectListResult dataObjectListResult && Equals(dataObjectListResult);

    public bool Equals(DataObjectListResult x, DataObjectListResult y)
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