using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.Exceptions;
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
    /// <exception cref="BerParsingException" accessor="get"></exception>
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

    public bool IsRequestedDataAvailable(IReadTlvDatabase database)
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
    /// <exception cref="TerminalDataException"></exception>
    public Tag[] GetNeededData(IReadTlvDatabase database)
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
    public bool TryGetRequestedDataItems(IReadTlvDatabase database, out PrimitiveValue[] result)
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

    // TODO: Should this DataObjectListResult pattern be updated? We have RelatedData objects, Command templates, etc. Should we be doing a covariant return type for each DOL?

    /// <exception cref="BerParsingException"></exception>
    /// <remarks>Book 3 Section 5.4</remarks>
    public virtual DataObjectListResult AsDataObjectListResult(PrimitiveValue[] dataObjects)
    {
        ValidateCommandTemplate(dataObjects);

        // HACK: You have a weird pattern going on. You first decode to DataObjectListResult, and then to a Command Template. This probably needs to be refactored to be more clear
        PrimitiveValue[] result = new PrimitiveValue[DataObjects.Length];

        for (int i = 0; i < DataObjects.Length; i++)
        {
            if (dataObjects.All(a => a.GetTag() != DataObjects[i].GetTag()))
                result[i] = new UnknownPrimitiveValue(DataObjects[i].GetTag(), DataObjects[i].GetLength());

            result[i] = dataObjects.First(a => a.GetTag() == DataObjects[i].GetTag());
        }

        return new DataObjectListResult(result.ToArray());
    }

    /// <remarks>Book 3 Section 5.4</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public virtual DataObjectListResult AsDataObjectListResult(IReadTlvDatabase database)
    {
        if (!TryGetRequestedDataItems(database, out PrimitiveValue[] result))
        {
            throw new
                TerminalDataException($"The method {nameof(AsDataObjectListResult)} could not be processed because a requested data item was not present in the database");
        }

        // HACK: You have a weird pattern going on. You first decode to DataObjectListResult, and then to a Command Template. This probably needs to be refactored to be more clear

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

    /// <summary>
    ///     Gets the byte count of the value field for the command template this data object list will create
    /// </summary>
    /// <returns></returns>
    public int GetValueByteCountOfCommandTemplate()
    {
        return _DataObjects?.Sum(a => a.GetValueByteCount()) ?? 0;
    }

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public virtual CommandTemplate AsCommandTemplate(IReadTlvDatabase database) => AsDataObjectListResult(database).AsCommandTemplate();

    /// <exception cref="BerParsingException"></exception>
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
    public int GetValueByteCount()
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