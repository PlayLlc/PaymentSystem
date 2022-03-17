using System;
using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;

namespace Play.Emv.Kernel.Databases;

public class DatabaseValue : TagLengthValue, IEquatable<DatabaseValue>, IEqualityComparer<DatabaseValue>
{
    #region Constructor

    public DatabaseValue(Tag tag) : base(tag, Array.Empty<byte>())
    { }

    public DatabaseValue(TagLengthValue tagLengthValue) : base(tagLengthValue.GetTag(), tagLengthValue.EncodeValue())
    { }

    private DatabaseValue(Tag tag, ReadOnlySpan<byte> contentOctets) : base(tag, contentOctets)
    { }

    #endregion

    #region Instance Members

    public bool IsNull() => _ContentOctets.Length == 0;

    #endregion

    #region Equality

    public bool Equals(DatabaseValue? other)
    {
        if (other is null)
            return false;

        if (other._Tag != _Tag)
            return false;

        if (other._ContentOctets.Length != _ContentOctets.Length)
            return false;

        for (int i = 0; i < _ContentOctets.Length; i++)
        {
            if (_ContentOctets[i] != other._ContentOctets[i])
                return false;
        }

        return true;
    }

    public bool Equals(DatabaseValue? x, DatabaseValue? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override int GetHashCode()
    {
        const int hash = 17;

        unchecked
        {
            int result = (int) (hash * _Tag);
            for (int i = 0; i < _ContentOctets.Length; i++)
                result ^= _ContentOctets[i].GetHashCode();

            return result;
        }
    }

    public int GetHashCode(DatabaseValue obj) => obj.GetHashCode();
    public int GetHashCode(PrimitiveValue obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static implicit operator DatabaseValue(PrimitiveValue value) => new(value.AsTagLengthValue(EmvCodec.GetBerCodec()));

    #endregion
}