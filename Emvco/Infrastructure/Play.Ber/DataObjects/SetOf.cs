using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Core.Exceptions;

namespace Play.Ber.DataObjects;

public abstract class SetOf : IEncodeBerDataObjects
{
    #region Instance Values

    private readonly uint _Tag;

    #endregion

    #region Constructor

    protected SetOf(Tag tag)
    {
        _Tag = tag;
    }

    #endregion

    #region Instance Members

    public Tag GetTag() => _Tag;
    public abstract uint GetTagLengthValueByteCount(BerCodec codec);
    public TagLengthValue AsTagLengthValue(BerCodec codec) => new(GetTag(), EncodeValue(codec));
    public ushort GetValueByteCount(BerCodec codec) => AsTagLengthValue(codec).GetValueByteCount();

    #endregion

    #region Serialization

    public abstract byte[] EncodeValue(BerCodec codec);
    public abstract byte[] EncodeTagLengthValue(BerCodec codec);

    #endregion
}

public class SetOf<T> : SetOf, IReadOnlyList<T> where T : IEncodeBerDataObjects, IRetrieveBerDataObjectMetadata
{
    #region Instance Values

    private readonly uint _Tag;
    private readonly T[] _Values;

    /// <summary>
    ///     The number of <see cref="T" /> items in this collection
    /// </summary>
    public int Count => _Values.Length;

    #endregion

    #region Constructor

    public SetOf(T[] values) : base(values.First().GetTag())
    {
        CheckCore.ForNullOrEmptySequence(values, nameof(values));

        _Values = values;
        _Tag = values.First().GetTag(); //.GetTag();
    }

    #endregion

    #region Instance Members

    public T[] AsArray() => _Values;
    public IEnumerator<T> GetEnumerator() => _Values.ToList().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public override uint GetTagLengthValueByteCount(BerCodec codec) => (uint) _Values.Sum(a => a.GetTagLengthValueByteCount(codec));

    #endregion

    #region Serialization

    public override byte[] EncodeValue(BerCodec codec) => _Values.SelectMany(a => a.EncodeTagLengthValue(codec)).ToArray();

    public override byte[] EncodeTagLengthValue(BerCodec codec)
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

    public T this[int index] =>
        _Values.Length != 0
            ? _Values[index]
            : throw new BerParsingException(new ArgumentOutOfRangeException(nameof(index),
                                                                            $"The {nameof(SetOf<T>)} could not retrieve the requested {typeof(T).Name} because the index value [{index}] was out of range"));
}