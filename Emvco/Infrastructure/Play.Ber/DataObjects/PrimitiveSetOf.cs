using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;

namespace Play.Ber.DataObjects;

public abstract record PrimitiveSetOf : PrimitiveValue, IReadOnlyList<PrimitiveValue>
{
    #region Instance Values

    private readonly PrimitiveValue[] _Values;
    public int Count => _Values.Length;

    #endregion

    #region Constructor

    protected PrimitiveSetOf(params PrimitiveValue[] values)
    {
        _Values = values;
    }

    #endregion

    #region Instance Members

    public PrimitiveValue[] AsArray() => _Values;
    public IEnumerator<PrimitiveValue> GetEnumerator() => _Values.ToList().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion

    #region Serialization

    public override byte[] EncodeValue(BerCodec codec) => _Values.SelectMany(a => a.EncodeTagLengthValue(codec)).ToArray();

    public new byte[] EncodeTagLengthValue(BerCodec codec)
    {
        int byteCount = (int) _Values.Sum(a => a.GetTagLengthValueByteCount(codec));

        Span<byte> result = stackalloc byte[byteCount];

        for (int i = 0, j = 0; i < _Values.Length; i++)
        {
            _Values[i].EncodeTagLengthValue(codec).AsSpan().CopyTo(result[j..]);
            j += (int) _Values[i].GetTagLengthValueByteCount(codec);
        }

        return result.ToArray();
    }

    #endregion

    /// <exception cref="BerParsingException" accessor="get"></exception>
    public PrimitiveValue this[int index] =>
        _Values.Length != 0
            ? _Values[index]
            : throw new BerParsingException(new ArgumentOutOfRangeException(nameof(index),
                                                                            $"The {nameof(PrimitiveSetOf)} could not retrieve the requested {nameof(PrimitiveSetOf)} because the index value [{index}] was out of range"));
}