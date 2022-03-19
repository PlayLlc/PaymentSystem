using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements
{

    public abstract record DataElementSetOf : DataElement<PrimitiveValue[]>, IReadOnlyList<PrimitiveValue>
    {
        #region Instance Values

        public int Count => _Value.Length;

        #endregion

        #region Constructor
        public PrimitiveValue DecodeAtRuntime()
        {
            _Codec.TryDecodingPrimitiveValueAtRuntime()
        }
        protected DataElementSetOf(params PrimitiveValue[] values) : base(values)
        { }

        #endregion

        #region Instance Members

        public PrimitiveValue[] AsArray() => _Value;
        public IEnumerator<PrimitiveValue> GetEnumerator() => _Value.ToList().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        public static DataElementSetOf Decode(ReadOnlySpan<byte> value)
        {
            _Codec.
        }



        #region Serialization

        public new byte[] EncodeValue() => _Value.SelectMany(a => a.EncodeTagLengthValue(_Codec)).ToArray();

        public new byte[] EncodeTagLengthValue()
        {
            int byteCount = (int) _Value.Sum(a => a.GetTagLengthValueByteCount(_Codec));

            Span<byte> result = stackalloc byte[byteCount];

            for (int i = 0, j = 0; i < _Value.Length; i++)
            {
                _Value[i].EncodeTagLengthValue(_Codec).AsSpan().CopyTo(result[j..]);
                j += (int) _Value[i].GetTagLengthValueByteCount(_Codec);
            }

            return result.ToArray();
        }

        #endregion

        /// <exception cref="BerParsingException" accessor="get"></exception>
        public PrimitiveValue this[int index] =>
            _Value.Length != 0
                ? _Value[index]
                : throw new BerParsingException(new ArgumentOutOfRangeException(nameof(index),
                                                                                $"The {nameof(PrimitiveSetOf)} could not retrieve the requested {nameof(PrimitiveSetOf)} because the index value [{index}] was out of range"));
    }
}