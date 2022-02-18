using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Codecs.Exceptions;

namespace Play.Emv.Ber.DataObjects;

/// <summary>
///     This object encapsulates Primitive <see cref="TagLength" /> Values requested by the ICC. These are ordered by the
///     expected concatenated response values
/// </summary>
public abstract record DataObjectList : DataElement<byte[]>
{
    #region Instance Values

    private TagLength[]? _DataObjects;

    private TagLength[] DataObjects
    {
        get
        {
            if (_DataObjects != null)
                return _DataObjects;

            _DataObjects = _Codec.DecodeTagLengthPairs(_Value);

            return _DataObjects;
        }
    }

    #endregion

    #region Constructor

    /// <exception cref="BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    protected DataObjectList(ReadOnlySpan<byte> value) : base(value.ToArray())
    { }

    #endregion

    #region Instance Members

    public bool IsRequestedDataAvailable(IQueryTlvDatabase database)
    {
        TagLength[] requestedItems = DataObjects;

        foreach (TagLength item in DataObjects)
        {
            if (database.IsKnown(item.GetTag()) && !database.IsPresentAndNotEmpty(item.GetTag()))
                return false;
        }

        return true;
    }

    public Tag[] GetNeededData(IQueryTlvDatabase database)
    {
        List<Tag> result = new();

        foreach (TagLength item in DataObjects)
        {
            if (!database.IsPresentAndNotEmpty(item.GetTag()))
                result.Add(item.GetTag());
        }

        return result.ToArray();
    }

    public bool TryGetRequestedDataItems(IQueryTlvDatabase database, out TagLengthValue[] result)
    {
        if (!IsRequestedDataAvailable(database))
        {
            result = Array.Empty<TagLengthValue>();

            return false;
        }

        result = new TagLengthValue[DataObjects.Length];

        for (int i = 0; i < DataObjects.Length; i++)
        {
            if (!database.TryGet(DataObjects[i].GetTag(), out TagLengthValue? tagLengthValue))
            {
                result[i] = new UnknownPrimitiveValue(DataObjects[i]).AsTagLengthValue();

                continue;
            }

            result[i] = tagLengthValue;
        }

        return true;
    }

    /// <exception cref="BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <remarks>Book 3 Section 5.4</remarks>
    public virtual DataObjectListResult AsDataObjectListResult(TagLengthValue[] dataObjects)
    {
        ValidateCommandTemplate(dataObjects);
        TagLengthValue[] result = new TagLengthValue[DataObjects.Length];

        for (int i = 0; i < DataObjects.Length; i++)
        {
            if (dataObjects.All(a => a.GetTag() != DataObjects[i].GetTag()))
                result[i] = new UnknownPrimitiveValue(DataObjects[i].GetTag(), DataObjects[i].GetLength()).AsTagLengthValue();

            result[i] = dataObjects.First(a => a.GetTag() == DataObjects[i].GetTag());
        }

        return new DataObjectListResult(result.ToArray());
    }

    /// <exception cref="BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <remarks>Book 3 Section 5.4</remarks>
    public virtual DataObjectListResult AsDataObjectListResult(IQueryTlvDatabase database)
    {
        if (!TryGetRequestedDataItems(database, out TagLengthValue[] result))
            throw new InvalidOperationException();

        TagLengthValue[] buffer = new TagLengthValue[DataObjects.Length];

        for (int i = 0; i < DataObjects.Length; i++)
        {
            if (result[i].GetValueByteCount() == DataObjects[i].GetValueByteCount())
                continue;

            if (result[i].GetValueByteCount() > DataObjects[i].GetValueByteCount())
                buffer[i] = new TagLengthValue(result[i].GetTag(), result[i].EncodeValue()[..DataObjects[i].GetValueByteCount()]);
            else
            {
                SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(DataObjects[i].GetValueByteCount());
                Span<byte> contentBuffer = spanOwner.Span;
                result[i].EncodeValue().CopyTo(contentBuffer);
                buffer[i] = new TagLengthValue(DataObjects[i].GetTag(), contentBuffer);
            }
        }

        return new DataObjectListResult(result.ToArray());
    }

    public virtual CommandTemplate AsCommandTemplate(IQueryTlvDatabase database) => AsDataObjectListResult(database).AsCommandTemplate();
    public virtual CommandTemplate AsCommandTemplate(TagLengthValue[] values) => AsDataObjectListResult(values).AsCommandTemplate();

    public bool Contains(Tag tag)
    {
        return DataObjects.Any(a => a.GetTag() == tag);
    }

    public int GetByteCount()
    {
        return DataObjects.Sum(a => a.GetTagLengthByteCount());
    }

    public int GetCommandTemplateByteCount()
    {
        return DataObjects.Sum(a => a.GetLengthByteCount());
    }

    public TagLength[] GetRequestedItems() => DataObjects;

    private void ValidateCommandTemplate(TagLengthValue[] value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (DataObjects.All(a => a.GetTag() != value[i].GetTag()))
            {
                throw new EmvEncodingFormatException(new ArgumentOutOfRangeException(
                    $"The argument {nameof(value)} did not contain a value for the requested object with the tag: {DataObjects[i].GetTag()}"));
            }
        }
    }

    #endregion
}