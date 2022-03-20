using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.Templates;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     This object encapsulates Primitive <see cref="TagLength" /> Values requested by the ICC. These are ordered by the
///     expected concatenated response values
/// </summary>
public abstract record DataObjectList : DataElement<byte[]>
{
    #region Instance Values

    private TagLength[]? _DataObjects;

    /// <summary>
    ///     DataObjects
    /// </summary>
    /// <exception cref="BerParsingException">Get.</exception>
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

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    protected DataObjectList(ReadOnlySpan<byte> value) : base(value.ToArray())
    { }

    #endregion

    #region Instance Members

    public bool IsRequestedDataAvailable(IQueryTlvDatabase database)
    {
        foreach (TagLength item in DataObjects)
        {
            if (!database.IsKnown(item.GetTag()))
                return false;

            if (database.IsPresent(item.GetTag()))
                return false;
        }

        return true;
    }

    /// <exception cref="BerParsingException"></exception>
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

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public bool TryGetRequestedDataItems(IQueryTlvDatabase database, out PrimitiveValue[] result)
    {
        if (!IsRequestedDataAvailable(database))
        {
            result = Array.Empty<PrimitiveValue>();

            return false;
        }

        result = new PrimitiveValue[DataObjects.Length];

        for (int i = 0; i < DataObjects.Length; i++)
        {
            if (!database.TryGet(DataObjects[i].GetTag(), out PrimitiveValue? primitiveValue))
            {
                result[i] = new UnknownPrimitiveValue(DataObjects[i]);

                continue;
            }

            result[i] = primitiveValue!;
        }

        return true;
    }

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <remarks>Book 3 Section 5.4</remarks>
    public virtual DataObjectListResult AsDataObjectListResult(PrimitiveValue[] dataObjects)
    {
        ValidateCommandTemplate(dataObjects);
        PrimitiveValue[] result = new PrimitiveValue[DataObjects.Length];

        for (int i = 0; i < DataObjects.Length; i++)
        {
            if (dataObjects.All(a => a.GetTag() != DataObjects[i].GetTag()))
                result[i] = new UnknownPrimitiveValue(DataObjects[i].GetTag(), DataObjects[i].GetLength());

            result[i] = dataObjects.First(a => a.GetTag() == DataObjects[i].GetTag());
        }

        return new DataObjectListResult(result.ToArray());
    }

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <remarks>Book 3 Section 5.4</remarks>
    /// <exception cref="OverflowException"></exception>
    public virtual DataObjectListResult AsDataObjectListResult(IQueryTlvDatabase database)
    {
        if (!TryGetRequestedDataItems(database, out PrimitiveValue[] result))
            throw new InvalidOperationException();

        PrimitiveValue[] buffer = new PrimitiveValue[DataObjects.Length];

        for (int i = 0; i < DataObjects.Length; i++)
        {
            if (result[i].GetValueByteCount(_Codec) == DataObjects[i].GetValueByteCount())
                continue;

            if (result[i].GetValueByteCount(_Codec) > DataObjects[i].GetValueByteCount())
                buffer[i] = _Codec.DecodePrimitiveValueAtRuntime(result[i].EncodeValue(_Codec)[..DataObjects[i].GetValueByteCount()]);
            else
                buffer[i] = result[i];
        }

        return new DataObjectListResult(buffer);
    }

    public virtual CommandTemplate AsCommandTemplate(IQueryTlvDatabase database) => AsDataObjectListResult(database).AsCommandTemplate();
    public virtual CommandTemplate AsCommandTemplate(PrimitiveValue[] values) => AsDataObjectListResult(values).AsCommandTemplate();

    /// <summary>
    ///     Contains
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    public bool Contains(Tag tag)
    {
        return DataObjects.Any(a => a.GetTag() == tag);
    }

    /// <summary>
    ///     GetByteCount
    /// </summary>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    public int GetByteCount()
    {
        return DataObjects.Sum(a => a.GetTagLengthByteCount());
    }

    /// <summary>
    ///     GetCommandTemplateByteCount
    /// </summary>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    public int GetCommandTemplateByteCount()
    {
        return DataObjects.Sum(a => a.GetLengthByteCount());
    }

    public TagLength[] GetRequestedItems() => DataObjects;

    /// <summary>
    ///     ValidateCommandTemplate
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="BerParsingException"></exception>
    private void ValidateCommandTemplate(PrimitiveValue[] value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (DataObjects.All(a => a.GetTag() != value[i].GetTag()))
            {
                throw new
                    BerParsingException(new
                                            ArgumentOutOfRangeException($"The argument {nameof(value)} did not contain a value for the requested object with the tag: {DataObjects[i].GetTag()}"));
            }
        }
    }

    #endregion
}