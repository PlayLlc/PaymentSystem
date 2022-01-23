﻿using System;
using System.Collections.Generic;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.Lengths;

namespace Play.Ber.InternalFactories;

/// <summary>
///     A Factory that provides an intermediate TLV object. The intermediate TLV object is
///     not a 'readonly ref struct' so we can use collections that allow the elements to
///     grow dynamically. With a readonly ref struct you can only create an array and we
///     so you have to know the length upfront and i'm lazy so i'm not going to create a
///     special array buffer
/// </summary>
internal sealed class TagLengthFactory
{
    #region Instance Members

    /// <exception cref="BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerInternalException"></exception>
    internal TagLength[] GetTagLengthArray(ReadOnlySpan<byte> value)
    {
        if (value.IsEmpty)
            return Array.Empty<TagLength>();

        int bufferIndex = 0;

        using SpanOwner<TagLength> tagLengthOwner = SpanOwner<TagLength>.Allocate(value.Length / 2);

        Span<TagLength> buffer = tagLengthOwner.Span;

        for (int spanIndex = 0; spanIndex < value.Length; bufferIndex++)
        {
            TagLength hello = ParseFirst(value[spanIndex..]);
            Tag tag = hello.GetTag();
            Length length = hello.GetLength();

            buffer[bufferIndex] = ParseFirst(value[spanIndex..]);
            spanIndex += buffer[bufferIndex].GetTagLengthValueByteCount();
        }

        return buffer[..bufferIndex].ToArray();
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    /// <exception cref="BerInternalException"></exception>
    internal Dictionary<Tag, TagLength[]> ParseDescendents(ReadOnlySpan<byte> value, TagLength parent)
    {
        if (parent.GetTag().GetDataObject() != DataObjectType.Constructed)
            throw new BerFormatException("A primitive object does not have a descendent tree to traverse");

        Dictionary<Tag, TagLength[]> descendents = new();
        ReadOnlySpan<byte> parentContentOctets = value[parent.ValueRange()];

        TagLength[] children = GetTagLengthArray(parentContentOctets);

        descendents.Add(parent.GetTag(), children);

        for (int i = 0, j = 0; i < children.Length; i++)
        {
            TagLength currentChild = children[i];

            if (currentChild.GetTag().GetDataObject() == DataObjectType.Constructed)
            {
                foreach (KeyValuePair<Tag, TagLength[]> keyValuePair in ParseDescendents(
                    parentContentOctets[j..currentChild.GetTagLengthValueByteCount()], currentChild))
                    descendents.Add(keyValuePair.Key, keyValuePair.Value);
            }

            j += currentChild.GetTagLengthValueByteCount();
        }

        return descendents;
    }

    /// <exception cref="BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerInternalException"></exception>
    internal TagLength ParseFirst(ReadOnlySpan<byte> value)
    {
        Tag tag = new(value);
        Length length = Length.Parse(value[tag.GetByteCount()..]);

        //Length length = new(value[tag.GetTagLengthValueByteCount()..]);

        return new TagLength(tag, length);
    }

    /// <exception cref="BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerInternalException"></exception>
    internal bool TryGetOffset(Tag tag, ReadOnlySpan<byte> value, out int startIndexResult)
    {
        if (value.IsEmpty)
            throw new BerException("An empty sequence was passed. No siblings could be parsed");

        if (value.Length < 2)
            throw new BerException("No siblings could be parsed. A TLV object needs at least 2 bytes for the tag and length");

        if (new Tag(value) == tag)
        {
            startIndexResult = -1;

            return true;
        }

        for (int rawValueOffset = 0; rawValueOffset < value.Length;)
        {
            TagLength currentTagLength = ParseFirst(value[rawValueOffset..]);

            if (currentTagLength.GetTag() == tag)
            {
                startIndexResult = rawValueOffset;

                return true;
            }

            rawValueOffset += currentTagLength.GetTagLengthValueByteCount();
        }

        startIndexResult = -1;

        return false;
    }

    /// <summary>
    ///     TryParseFirst
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    /// <exception cref="BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerInternalException"></exception>
    internal bool TryParseFirst(ReadOnlySpan<byte> value, out TagLength? result)
    {
        if (!Tag.IsValid(value))
        {
            result = null;

            return false;
        }

        result = ParseFirst(value);

        return true;
    }

    #endregion
}