using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Ber.Lengths;
using Play.Codecs.Exceptions;
using Play.Core.Exceptions;
using Play.Core.Specifications;

namespace Play.Ber.Codecs;

public partial class BerCodec
{
    #region Instance Members

    /// <summary>
    ///     This method sorts the <paramref name="dataElements" /> according to the ranked value provided by the
    ///     <paramref name="index" />. This allows <see cref="ConstructedValue" /> objects to return their child objects
    ///     encoded in the same order every time
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
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

    // DEPRECATING: This method will eventually be deprecated in favor of using strongly typed arguments instead of generic constraints
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public _T? AsConstructed<_T>(Func<EncodedTlvSiblings, _T> decodeFunc, uint tag, EncodedTlvSiblings encodedTlvSiblings) where _T : ConstructedValue
    {
        if (!encodedTlvSiblings.TryGetValueOctetsOfSibling(tag, out ReadOnlyMemory<byte> rawValueContent))
            return null;

        return decodeFunc.Invoke(new EncodedTlvSiblings(_TagLengthFactory.GetTagLengthArray(rawValueContent.Span), rawValueContent));
    }

    #region Byte Count

    /// <summary>
    ///     Gets the byte count of the Value field of the TagLengthValue object this ConstructedValue represents
    /// </summary>
    /// <param name="children"></param>
    /// <returns></returns>
    /// <exception cref="OverflowException"></exception>
    public ushort GetValueByteCount(params IEncodeBerDataObjects?[] children)
    {
        return checked((ushort) children.Sum(a => a?.GetTagLengthValueByteCount(this) ?? 0));
    }

    #endregion

    #endregion

    #region Encode Value

    /// <summary>
    ///     This method is the last method to be called in a double dispatch pattern. The EncodeValue of the
    ///     <see cref="ConstructedValue" /> is invoked, causing the <see cref="ConstructedValue" /> to invoke this method
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public byte[] EncodeValue(ConstructedValue parent, Tag[] childIndex, params IEncodeBerDataObjects?[] children)
    {
        return EncodeValue(parent, GetIndexedDataElements(childIndex, children.Where(a => a != null).Select(x => x!).ToArray()));
    }

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private byte[] EncodeValue(TagLength parent, IReadOnlyList<IEncodeBerDataObjects> children, Span<byte> buffer)
    {
        if (parent.GetValueByteCount() != buffer.Length)
        {
            throw new CodecParsingException(
                $"The {nameof(BerCodec)} could not {nameof(EncodeValue)} because the length of the {nameof(buffer)} provided was inconsistent with the length of the {nameof(parent)}");
        }

        for (int i = 0, j = 0; i < children.Count; i++)
        {
            Span<byte> encoding = children[i].EncodeTagLengthValue(this).AsSpan();
            encoding.CopyTo(buffer[j..]);
            j += encoding.Length;
        }

        return buffer.ToArray();
    }

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private byte[] EncodeValue(ConstructedValue parent, params IEncodeBerDataObjects[] children)
    {
        if (!children.Any())
            return EncodeEmptyDataObject(parent);

        TagLength tagLength = new(parent.GetTag(), new Length(parent.GetValueByteCount(this)));

        // TODO: When benchmarking we might want to use stackalloc if the byte count is under our threshold value
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(tagLength.GetValueByteCount());
        Span<byte> buffer = spanOwner.Span;

        return EncodeValue(tagLength, children, buffer);
    }

    #endregion

    #region Encode Tag Length Value

    /// <summary>
    ///     This method is the last method to be called in a double dispatch pattern. The EncodeTagLengthValue of the
    ///     <see cref="ConstructedValue" /> is invoked, causing the <see cref="ConstructedValue" /> to invoke this method
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public byte[] EncodeTagLengthValue(ConstructedValue parent, Tag[] childIndex, params IEncodeBerDataObjects?[] children)
    {
        IEncodeBerDataObjects[] indexedDataElements = GetIndexedDataElements(childIndex, children.Where(a => a != null).Select(x => x!).ToArray());

        return EncodeTagLengthValue(parent, indexedDataElements);
    }

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private byte[] EncodeTagLengthValue(ConstructedValue parent, params IEncodeBerDataObjects[] children)
    {
        if (!children.Any())
            return EncodeEmptyDataObject(parent);

        TagLength tagLength = new(parent.GetTag(), new Length(parent.GetValueByteCount(this)));

        // TODO: When benchmarking we might want to use stackalloc if the byte count is under our threshold value
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(tagLength.GetTagLengthValueByteCount());
        Span<byte> buffer = spanOwner.Span;

        tagLength.Encode().CopyTo(buffer);

        EncodeValue(tagLength, children, buffer[tagLength.GetValueOffset()..]);

        return buffer.ToArray();
    }

    #endregion
}