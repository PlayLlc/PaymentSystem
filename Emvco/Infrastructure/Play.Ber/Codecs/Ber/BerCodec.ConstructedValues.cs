using System;
using System.Linq;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Ber.Lengths;
using Play.Core.Exceptions;
using Play.Core.Specifications;

namespace Play.Ber.Codecs;

public partial class BerCodec
{
    #region Instance Members

    /// <param name="index"></param>
    /// <param name="dataElements"></param>
    /// <returns></returns>
    public IEncodeBerDataObjects[] GetIndexedDataElements(Tag[] index, IEncodeBerDataObjects[] dataElements)
    {
        CheckCore.ForMaximumLength(dataElements, index.Length,
            $"The argument {nameof(index)} has fewer items than argument {nameof(dataElements)}. Please ensure that all {nameof(IEncodeBerDataObjects)} children have been indexed");

        IEncodeBerDataObjects[] result = new IEncodeBerDataObjects[dataElements.Count()];

        for (nint i = 0, j = 0; i < index.Length; i++)
        {
            if (dataElements.All(x => x.GetTag() != index[i]))
                continue;

            foreach (IEncodeBerDataObjects element in dataElements.Where(a => a.GetTag() == index[i]))
                result[j++] = element;
        }

        if (result.Length < dataElements.Count())
        {
            throw new BerParsingException(new ArgumentOutOfRangeException(nameof(index),
                $"The argument {nameof(index)} has is missing an item in the {nameof(dataElements)} argument. Please ensure that all {nameof(IEncodeBerDataObjects)} children have been indexed"));
        }

        return result;
    }

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Exceptions._Temp.BerFormatException"></exception>
    public T? AsConstructed<T>(Func<EncodedTlvSiblings, T> decodeFunc, uint tag, EncodedTlvSiblings encodedTlvSiblings)
        where T : ConstructedValue
    {
        if (!encodedTlvSiblings.TryGetValueOctetsOfChild(tag, out ReadOnlyMemory<byte> rawValueContent))
            return null;

        return decodeFunc.Invoke(new EncodedTlvSiblings(_TagLengthFactory.GetTagLengthArray(rawValueContent.Span), rawValueContent));
    }

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Exceptions._Temp.BerFormatException"></exception>
    public T? AsConstructed<T>(Func<BerCodec, EncodedTlvSiblings, T> decodeFunc, uint tag, EncodedTlvSiblings encodedTlvSiblings)
        where T : ConstructedValue
    {
        if (!encodedTlvSiblings.TryGetValueOctetsOfChild(tag, out ReadOnlyMemory<byte> rawValueContent))
            return null;

        return decodeFunc.Invoke(this, new EncodedTlvSiblings(_TagLengthFactory.GetTagLengthArray(rawValueContent.Span), rawValueContent));
    }

    /// <summary>
    ///     DecodeChildren
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Exceptions._Temp.BerFormatException"></exception>
    public EncodedTlvSiblings DecodeChildren(ReadOnlyMemory<byte> value)
    {
        TagLength tagLength = _TagLengthFactory.ParseFirst(value.Span);

        return DecodeSiblings(value[tagLength.GetValueOffset()..]);
    }

    public EncodedTlvSiblings DecodeSiblings(ReadOnlyMemory<byte> value) => new(_TagLengthFactory.GetTagLengthArray(value.Span), value);

    #endregion

    #region Byte Count

    /// <summary>
    ///     Gets the byte count of the Value field of the TagLengthValue object this ConstructedValue
    ///     represents
    /// </summary>
    /// <param name="children"></param>
    /// <returns></returns>
    public ushort GetValueByteCount(params IEncodeBerDataObjects?[] children)
    {
        return checked((ushort) children.Sum(a => a?.GetTagLengthValueByteCount(this) ?? 0));
    }

    public uint GetTagLengthValueByteCount(ConstructedValue parent, params IEncodeBerDataObjects?[] children)
    {
        uint contentOctetLength = checked((uint) children.Sum(a => a?.GetTagLengthValueByteCount(this) ?? 0));
        TagLength tagLength = new(parent.GetTag(), contentOctetLength);

        return checked((ushort) tagLength.GetTagLengthValueByteCount());
    }

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>

    //public uint GetTagLengthValueByteCount<T>(ConstructedValue value) where T : ConstructedValue => value.GetTagLengthValueByteCount(this);

    #endregion

    #region Encoding

    private byte[] EncodeValue(ConstructedValue value) => value.EncodeValue(this);

    /// <param name="parent"></param>
    /// <param name="childIndex"></param>
    /// <param name="children"></param>
    /// <returns></returns>
    public byte[] EncodeValue(ConstructedValue parent, Tag[] childIndex, params IEncodeBerDataObjects?[] children)
    {
        return EncodeValue(parent, GetIndexedDataElements(childIndex, children.Where(a => a != null).Select(x => x!).ToArray()));
    }

    public byte[] EncodeValue(ConstructedValue parent, params IEncodeBerDataObjects?[] children)
    {
        if (!children.Any())
            return EncodeEmptyDataObject(parent);

        TagLength tagLength = new(parent.GetTag(), new Length(parent.GetValueByteCount(this)));

        if (tagLength.GetValueByteCount() <= Specs.ByteArray.StackAllocateCeiling)
        {
            Span<byte> result = stackalloc byte[tagLength.GetValueByteCount()];

            for (int i = 0, j = 0; i < children.Length; i++)
            {
                ReadOnlySpan<byte> encoding = children[i]?.EncodeTagLengthValue(this);
                int encodingLength = encoding.Length;
                encoding.CopyTo(result[j..]);
                j += encoding.Length;
            }

            return result.ToArray();
        }
        else
        {
            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(tagLength.GetValueByteCount());
            Span<byte> result = spanOwner.Span;

            for (int i = 0, j = 0; i < children.Length; i++)
            {
                ReadOnlySpan<byte> encoding = children[i]?.EncodeTagLengthValue(this);
                encoding.CopyTo(result[j..]);
                j += encoding.Length;
            }

            return result.ToArray();
        }
    }

    /// <summary>
    ///     EncodeTagLengthValue
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="childIndex"></param>
    /// <param name="children"></param>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public byte[] EncodeTagLengthValue(ConstructedValue parent, Tag[] childIndex, params IEncodeBerDataObjects?[] children)
    {
        return EncodeTagLengthValue(parent, GetIndexedDataElements(childIndex, children.Where(a => a != null).Select(x => x!).ToArray()));
    }

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public byte[] EncodeTagLengthValue(ConstructedValue parent, params IEncodeBerDataObjects[] children)
    {
        if (!children.Any())
            return EncodeEmptyDataObject(parent);

        byte[]? contentOctets = parent.EncodeValue(this);
        Length length = new(parent.GetValueByteCount(this));

        TagLength tagLength = new(parent.GetTag(), new Length(parent.GetValueByteCount(this)));

        if (tagLength.GetTagLengthValueByteCount() <= Specs.ByteArray.StackAllocateCeiling)
        {
            Span<byte> result = stackalloc byte[tagLength.GetTagLengthValueByteCount()];
            tagLength.Encode().CopyTo(result);

            for (int i = 0, j = tagLength.GetValueOffset(); i < children.Length; i++)
            {
                Span<byte> encoding = children[i].EncodeTagLengthValue(this).AsSpan();
                encoding.CopyTo(result[j..]);
                j += encoding.Length;
            }

            return result.ToArray();
        }
        else
        {
            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(tagLength.GetTagLengthValueByteCount());
            Span<byte> result = spanOwner.Span;

            tagLength.Encode().CopyTo(result);

            for (int i = 0, j = tagLength.GetValueOffset(); i < children.Length; i++)
            {
                ReadOnlySpan<byte> encoding = EncodeValue(children[i]!);
                encoding.CopyTo(result[j..]);
                j += encoding.Length;
            }

            return result.ToArray();
        }
    }

    #endregion
}